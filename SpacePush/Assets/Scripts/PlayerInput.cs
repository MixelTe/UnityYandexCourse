using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> OnTap;
    public event Action<Vector2> OnSwipe;
    public event Action<Vector2> OnSwiping;
    public event Action<Vector2> OnEnd;

    public int TapRadius;
	private Vector3 _touchStart;

	public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var d = Input.mousePosition - _touchStart;
            OnSwiping?.Invoke(d);
        }
        if (Input.GetMouseButtonUp(0))
        {
            var d = Input.mousePosition - _touchStart;
            if (d.magnitude < TapRadius)
                OnTap?.Invoke(Input.mousePosition);
            else
                OnSwipe?.Invoke(d);
            OnEnd?.Invoke(d);
        }
    }
}
