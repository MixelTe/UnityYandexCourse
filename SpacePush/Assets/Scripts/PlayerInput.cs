using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> OnTap;
    public event Action<Vector2> OnStart;
    public event Action<Vector2> OnSwipe;
    public event Action<Vector2> OnSwiping;
    public event Action<Vector2> OnEnd;

    public int TapRadius;
    public int SwipeRadius;
    private Vector3 _touchStart;

	public void Update()
    {
        if (!GameManager.Ins.GameRunning) return;

        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = Input.mousePosition;
            OnStart?.Invoke(_touchStart);
        }
        if (Input.GetMouseButton(0))
        {
            var d = Input.mousePosition - _touchStart;
            OnSwiping?.Invoke(d);
        }
        if (Input.GetMouseButtonUp(0))
        {
            var d = Input.mousePosition - _touchStart;
            var magnitude = d.magnitude;
            if (magnitude < TapRadius)
                OnTap?.Invoke(Input.mousePosition);
			else
			{
                var v = Mathf.Min((magnitude - TapRadius) / SwipeRadius, 1);
                OnSwipe?.Invoke(d.normalized * v);
			}
            OnEnd?.Invoke(d);
        }
    }
}
