using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Enemy
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private CircleCollider2D _collider2D;
	[SerializeField] private Sprite[] _variants;
	[SerializeField] private Vector2 _speed;
	[SerializeField] private Vector2 _size;

	private void Start()
	{
		_curSpeed = _speed.Random();
		_curRotation = Utils.Atan2(GameField.RandomPosInside() - transform.position.ToVector2());
		_renderer.sprite = _variants.Random();
		var size = _size.Random();
		_renderer.transform.localScale = new Vector3(size, size, size);
		_collider2D.radius = size / 3f;
	}
}
