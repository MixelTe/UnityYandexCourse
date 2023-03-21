using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> OnTap;
    public event Action<Vector2> OnSwipe;

    [SerializeField] private int _tapRadius;
	private Vector3 _touchStart;

	public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            var d = Input.mousePosition - _touchStart;
            if (d.magnitude < _tapRadius)
                OnTap?.Invoke(Input.mousePosition);
            else
                OnSwipe?.Invoke(d);
        }
    }
}
