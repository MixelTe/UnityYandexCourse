using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	[SerializeField] private float _modelSize;
	internal float _curRotation;
	internal float _curSpeed;

	private void Update()
	{
		Move(false);
	}

	internal void Move(bool loopInBoundary)
	{
		var speed = _curSpeed * Time.deltaTime;
		var movement = new Vector3(Mathf.Cos(_curRotation) * speed, Mathf.Sin(_curRotation) * speed);
		var newPos = transform.position + movement;
		var rotation = Quaternion.Euler(0, 0, _curRotation * Mathf.Rad2Deg);
		
		if (loopInBoundary) 
			newPos = GameField.LoopInBoundary(newPos, _modelSize);

		transform.SetPositionAndRotation(newPos, rotation);
	}
}
