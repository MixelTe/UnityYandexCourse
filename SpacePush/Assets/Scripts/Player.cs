using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movable
{
    [SerializeField] private Collider2D _forceField;
    [SerializeField] private float _pushSpeed;
	[SerializeField] private float _pushDelay;
    [SerializeField] private float _spaceDensity;
	public PlayerState State { get; private set; } = PlayerState.Idle;
	public float PushDelay { get => _pushDelay; }

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
}

public enum PlayerState
{
	Idle,
	Charge,
	Dash
}
