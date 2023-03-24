using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	[SerializeField] private float _shakeSlowingSpeed;
	[SerializeField] private float _shakeSpeed;
	[SerializeField] private float _shakePower;
	private Camera _camera;
	private Vector3 _pos;
	private float _shakeAngle;
	private float _shakeMagnitude;
	private float _shakeTime;

	private void Awake()
    {
        _camera = GetComponent<Camera>();
		_pos = transform.position;
	}

    private void Update()
    {
		if (_shakeMagnitude < 0) return;
		_shakeMagnitude = Mathf.Max(_shakeMagnitude - _shakeSlowingSpeed, 0);
		_shakeTime += Time.deltaTime * _shakeSpeed;
		//_shakeTime %= Mathf.PI * 2;
		var v = Mathf.Sin(_shakeTime) * _shakeMagnitude;
		var x = Mathf.Cos(_shakeAngle) * v + _pos.x;
		var y = Mathf.Sin(_shakeAngle) * v + _pos.y;
		transform.position = new Vector3(x, y, _pos.z);
	}

	public void Shake(float power = 1, bool continued = false)
	{
		_shakeMagnitude = Mathf.Max(_shakePower * power, _shakeMagnitude);
		if (!continued)
		{
			_shakeAngle = Random.value * Mathf.PI * 2;
			_shakeTime = 0;
		}
	}

	public Rect GetBoundary()
	{
		var h = _camera.orthographicSize;
		var w = _camera.aspect * h;
		return new Rect(_pos.x - w, _pos.y - h, w * 2, h * 2);
	}
}
