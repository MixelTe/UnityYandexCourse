using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Movable
{
	internal bool _enteredField = false;

	internal void Move()
	{
		if (!_enteredField)
			_enteredField = GameField.GetBoundary().Contains(transform.position);

		Move(_enteredField);
	}
}
