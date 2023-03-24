using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _handle;

    private void Start()
    {
        _center.gameObject.SetActive(false);
        _handle.gameObject.SetActive(false);
    }

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
            _center.gameObject.SetActive(true);
            _handle.gameObject.SetActive(true);
            _center.position = Input.mousePosition;
            _handle.position = Vector3.zero;
        }
        if (Input.GetMouseButton(0))
        {
            var d = Input.mousePosition - _center.position;
            var magnitude = d.magnitude;
            d = d.normalized * Mathf.Min(magnitude, GameManager.Ins.PlayerInput.SwipeRadius + GameManager.Ins.PlayerInput.TapRadius);
            if (magnitude < GameManager.Ins.PlayerInput.TapRadius)
			{
                var scale = magnitude / GameManager.Ins.PlayerInput.TapRadius;
                _center.localScale = Vector3.one * scale;
            }
			else
			{
                var scaleY = Mathf.Min((magnitude - GameManager.Ins.PlayerInput.TapRadius) / GameManager.Ins.PlayerInput.SwipeRadius, 1);
                _center.localScale = new Vector3(1, 1 + scaleY, 1);
            }
            _center.rotation = Quaternion.Euler(0, 0, Utils.Atan2(d) * Mathf.Rad2Deg + 90);
            _handle.position = _center.position + new Vector3(d.x, d.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _center.gameObject.SetActive(false);
            _handle.gameObject.SetActive(false);
        }
    }
}
