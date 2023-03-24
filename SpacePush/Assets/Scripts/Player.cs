using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movable
{
    [SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private ParticleSystem _explosionParticles;
	[Header("Movement")]
	[SerializeField] private float _pushSpeed;
	[SerializeField] private float _pushDelay;
    [SerializeField] private float _spaceDensity;
	[Header("Health")]
	[SerializeField] private int _maxHealth;
	[SerializeField] private float _damageDelay;
	[SerializeField] private float _damagePower;
	[SerializeField] private float _blinkSpeed;
	[Header("Dash")]
	[SerializeField, Rename("Field")] private Collider2D _forceField;
	[SerializeField, Rename("Max Charges")] private float _dashMaxCharges;
	[SerializeField, Rename("Cooldown Time")] private float _dashCooldownTime;
	[SerializeField, Rename("Uncharged Power Mul")] private float _dashUnchargedPowerMul;
	[Header("Power wave")]
	[SerializeField, Rename("Effect")] private ParticleSystem _powerWave;
    [SerializeField, Rename("Field")] private CircleCollider2D _waveField;
	[SerializeField, Rename("Time")] private float _splashTime;
	[SerializeField, Rename("Start Radius")] private float _splashStartRadius;
	[SerializeField, Rename("Radius")] private float _splashRadius;
	[SerializeField, Rename("Max Charges")] private float _splashMaxCharges;
	[SerializeField, Rename("Cooldown Time")] private float _splashCooldownTime;

	public float PushDelay { get => _pushDelay; }
	public float DashCharges { get => _dashCharges / _dashMaxCharges; }
	public float SplashCharges { get => _splashCharges / _splashMaxCharges; }

	public PlayerState State { get; private set; } = PlayerState.Idle;
	private int _health;
	private float _damageDelayCur;
	private float _dashCharges;
	private float _splashCharges;

	private void Start()
	{
		_health = _maxHealth;
		_dashCharges = _dashMaxCharges;
		_splashCharges = _splashMaxCharges;
	}

	private void OnEnable()
	{
		_forceField.enabled = false;
		_waveField.enabled = false;
		GameManager.Ins.PlayerInput.OnSwipe += OnSwipe;
        GameManager.Ins.PlayerInput.OnSwiping += OnSwiping;
		GameManager.Ins.PlayerInput.OnTap += OnTap;
	}

	private void OnDisable()
	{
		GameManager.Ins.PlayerInput.OnSwipe -= OnSwipe;
		GameManager.Ins.PlayerInput.OnSwiping -= OnSwiping;
		GameManager.Ins.PlayerInput.OnTap -= OnTap;
	}

	private void Update()
    {
		_curSpeed = Mathf.Max(_curSpeed - _spaceDensity * Time.deltaTime, 0);
		_damageDelayCur = Mathf.Max(_damageDelayCur - Time.deltaTime, 0);
		_dashCharges = Mathf.Min(_dashCharges + Time.deltaTime / _dashCooldownTime, _dashMaxCharges);
		_splashCharges = Mathf.Min(_splashCharges + Time.deltaTime / _splashCooldownTime, _splashMaxCharges);

		if (_curSpeed == 0 && State == PlayerState.Dash)
		{
			State = PlayerState.Idle;
			_forceField.enabled = false;
		}

		Move(true);
	}

	private void OnSwipe(Vector2 swipe)
	{
		if (State != PlayerState.Idle && State != PlayerState.Dash) return;
		StartCoroutine(Push(swipe));
	}
	private void OnSwiping(Vector2 d)
	{
		if (State != PlayerState.Idle) return;
		var magnitude = d.magnitude;
		if (magnitude < GameManager.Ins.PlayerInput.TapRadius) return;

		_curRotation = Utils.Atan2(-d);
		var shakeMul = Mathf.Min(magnitude / GameManager.Ins.PlayerInput.SwipeRadius, 1);
		GameManager.Ins.Camera.Shake(0.25f * shakeMul, true);
	}

	private IEnumerator Push(Vector2 swipe)
	{
		State = PlayerState.Charge;
		_dashCharges = Mathf.Max(_dashCharges - 1, 0);
		yield return new WaitForSeconds(_pushDelay);
		State = PlayerState.Dash;
		GameManager.Ins.Camera.Shake();
		_forceField.enabled = true;
		_curRotation = Utils.Atan2(-swipe);

		var speedMul = 1f;
		if (_dashCharges < 1)
		{
			GameManager.Ins.StatsDisplay.PopDash();
			speedMul = _dashUnchargedPowerMul;
		}
		_curSpeed = swipe.magnitude * _pushSpeed * speedMul;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (State == PlayerState.Dash) return;
		if (State == PlayerState.Splash) return;
		if (collision.TryGetComponent<Enemy>(out var enemy))
		{
			_curSpeed = _damagePower;
			_curRotation = Utils.Atan2(transform.position - enemy.transform.position);
			
			if (_damageDelayCur > 0) return;

			_damageDelayCur = _damageDelay;
			_health--;
			GameManager.Ins.StatsDisplay.UpdateHealth((float)_health / _maxHealth);
			StartCoroutine(Blink());

			if (_health <= 0)
			{
				_health = 0;
				Instantiate(_explosionParticles, transform.position, Quaternion.identity);
				gameObject.SetActive(false);
				GameManager.Ins.EndGame();
			}
		}
	}

	private IEnumerator Blink()
	{
		for (float t = 0; t < 1; t += Time.deltaTime / _damageDelay)
		{
			var v = Mathf.Abs(Mathf.Sin(t * _blinkSpeed)) * 0.8f;
			_renderer.color = Color.HSVToRGB(0, v, 1);
			yield return null;
		}
		_renderer.color = Color.white;
	}

	private void OnTap(Vector2 pos)
	{
		if (State != PlayerState.Idle && State != PlayerState.Dash) return;
		if (_splashCharges < 1) 
		{
			GameManager.Ins.StatsDisplay.PopSplash();
			return;
		}
		StartCoroutine(Splash());
	}

	private IEnumerator Splash()
	{
		State = PlayerState.Splash;
		_splashCharges -= 1;
		GameManager.Ins.Camera.Shake();
		Instantiate(_powerWave, transform.position, Quaternion.identity, transform);

		_waveField.enabled = true;
		for (float t = 0; t < 1; t += Time.deltaTime / _splashTime)
		{
			_waveField.radius = Mathf.Lerp(_splashStartRadius, _splashRadius, t);
			yield return null;
		}
		_waveField.enabled = false;
		State = PlayerState.Idle;
	}
}

public enum PlayerState
{
	Idle,
	Charge,
	Dash,
	Splash
}
