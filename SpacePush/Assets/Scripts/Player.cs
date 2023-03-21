using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movable
{
    [SerializeField] private float _pushSpeed;
    [SerializeField] private float _spaceDensity;

	private void Start()
	{
		GameManager.Ins.PlayerInput.OnSwipe += OnSwipe;
	}

	private void Update()
    {
		_curSpeed = Mathf.Max(_curSpeed - _spaceDensity, 0);
		Move(true);
	}

	private void OnSwipe(Vector2 swipe)
	{
		_curRotation = Mathf.Atan2(-swipe.y, -swipe.x);
		_curSpeed = _pushSpeed;
	}
}
