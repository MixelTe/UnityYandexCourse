using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

	private void Update()
    {
        var boundary = GameField.GetBoundary().Inflate(0.5f);
        var size = _spriteRenderer.sprite.rect;
        var xMul = boundary.width / size.width;
        var yMul = boundary.height / size.height;
        var mul = Mathf.Max(xMul, yMul) * _spriteRenderer.sprite.pixelsPerUnit;

        transform.localScale = new Vector2(mul, mul);
    }
}
