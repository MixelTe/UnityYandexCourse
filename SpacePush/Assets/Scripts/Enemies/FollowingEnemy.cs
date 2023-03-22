using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : Enemy
{
	private static readonly List<FollowingEnemy> _allies = new();
	[SerializeField] private float _speed;
	[SerializeField] private float _rotationSpeed;
	[SerializeField] private float _stopDistance;
	[SerializeField] private float _partyDistance;
	[SerializeField] private float _partyStrongDistance;
	[SerializeField] private float _partyAvoidForce;
	[SerializeField] private float _partyAvoidForceStrong;

	private void Start()
	{
		_allies.Add(this);
		var d = DistanceToPlayer();
		_curSpeed = _speed;
		_curRotation = Mathf.Atan2(d.y, d.x);
	}

	private void Update()
	{
		var distance = DistanceToPlayer();
		if (distance.sqrMagnitude > _stopDistance * _stopDistance)
			RotateToPlayer(distance);

		AvoidAllies();
		Move();
	}

	private void OnDestroy()
	{
		_allies.Remove(this);
	}

	private void RotateToPlayer(Vector2 distance)
	{
		var targetRotation = Mathf.Atan2(distance.y, distance.x);

		var targetRotationLooped = Mathf.PI * 2 + targetRotation;
		var d1 = targetRotation - _curRotation;
		var d2 = targetRotationLooped - _curRotation;
		var d = Mathf.Abs(d1) < Mathf.Abs(d2) ? d1 : d2;

		var rotationSpeed = _rotationSpeed * Mathf.Deg2Rad;
		_curRotation += Mathf.Clamp(d, -rotationSpeed, rotationSpeed);
	}

	private Vector2 DistanceToPlayer()
	{
		var playerPos = GameManager.Ins.Player.transform.position;
		if (!_enteredField)
			return playerPos - transform.position;

		var boundary = GameField.GetBoundary();

		var dx = GetMinLoopDistance(transform.position.x, playerPos.x, boundary.width);
		var dy = GetMinLoopDistance(transform.position.y, playerPos.y, boundary.height);

		return new Vector2(dx, dy);
	}

	private float GetMinLoopDistance(float x1, float x2, float width)
	{
		var dx = x2 - x1;
		var dxLoop1 = dx - width;
		var dxLoop2 = dx + width;
		if (Mathf.Abs(dxLoop1) < Mathf.Abs(dx)) return dxLoop1;
		if (Mathf.Abs(dxLoop2) < Mathf.Abs(dx)) return dxLoop2;
		return dx;
	}

	private void AvoidAllies()
	{
		var boundary = GameField.GetBoundary();
		var partyDistanceSqr = _partyDistance * _partyDistance;
		var partyMinDistanceSqr = _partyStrongDistance * _partyStrongDistance;
		foreach (var ally in _allies)
		{
			var dx = GetMinLoopDistance(transform.position.x, ally.transform.position.x, boundary.width);
			var dy = GetMinLoopDistance(transform.position.y, ally.transform.position.y, boundary.height);
			var d = new Vector3(-dx, -dy);
			if (d.sqrMagnitude < partyMinDistanceSqr)
			{
				transform.position += d.normalized * _partyAvoidForceStrong;
			}
			else if (d.sqrMagnitude < partyDistanceSqr)
			{
				var force = d.sqrMagnitude / partyDistanceSqr * _partyAvoidForce;
				transform.position += d.normalized * force;
			}
		}
	}
}
