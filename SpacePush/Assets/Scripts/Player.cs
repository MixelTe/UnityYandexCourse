using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movable
{
    [SerializeField] private Collider2D _forceField;
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
	public float PushDelay { get => _pushDelay; }

	public PlayerState State { get; private set; } = PlayerState.Idle;
	private int _health;
	private float _damageDelayCur;

	private void Start()
	{
		_health = _maxHealth;
	}

	private void OnEnable()
	{
		_forceField.enabled = false;
		GameManager.Ins.PlayerInput.OnSwipe += OnSwipe;
        GameManager.Ins.PlayerInput.OnSwiping += OnSwiping;
	}

	private void OnDisable()
	{
		GameManager.Ins.PlayerInput.OnSwipe -= OnSwipe;
		GameManager.Ins.PlayerInput.OnSwiping -= OnSwiping;
	}

	private void Update()
    {
		_curSpeed = Mathf.Max(_curSpeed - _spaceDensity, 0);
		_damageDelayCur = Mathf.Max(_damageDelayCur - Time.deltaTime, 0);

		if (_curSpeed == 0 && State == PlayerState.Dash)
		{
			State = PlayerState.Idle;
			_forceField.enabled = false;
		}

		Move(true);
	}

	private void OnSwipe(Vector2 swipe)
	{
        if (State != PlayerState.Idle) return;
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
		yield return new WaitForSeconds(_pushDelay);
		State = PlayerState.Dash;
		GameManager.Ins.Camera.Shake();
		_forceField.enabled = true;
		_curRotation = Utils.Atan2(-swipe);
		_curSpeed = swipe.magnitude * _pushSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (State == PlayerState.Dash) return;
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
}

public enum PlayerState
{
	Idle,
	Charge,
	Dash
}
