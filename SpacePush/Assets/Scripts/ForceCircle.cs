using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCircle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Material _particleMaterial;
    [SerializeField] private float _fullColorDistance;
    [SerializeField] private float _maxStretch;
    [SerializeField] private float _xStretchMul;
    [SerializeField] private float _yStretchMul;

    private void Start()
    {
        _particleSystem.transform.localScale = Vector3.zero;
        GameManager.Ins.PlayerInput.OnSwiping += OnSwiping;
        GameManager.Ins.PlayerInput.OnEnd += OnEnd;
    }

    private void OnSwiping(Vector2 d)
    {
        if (GameManager.Ins.Player.State != PlayerState.Idle) return;

        var magnitude = d.magnitude;
        if (magnitude > GameManager.Ins.PlayerInput.TapRadius)
		{
            var color = _particleMaterial.color;
            if (magnitude < _fullColorDistance)
                color.a = magnitude / _fullColorDistance;
			else 
                color.a = 1;

            var scaleX = Mathf.Min((magnitude - GameManager.Ins.PlayerInput.TapRadius) / _maxStretch, 1) * _xStretchMul + 1;
            var scaleY = 1 + (1 - scaleX) * _yStretchMul;
            _particleSystem.transform.localScale = new Vector3(scaleX, scaleY, 1);
            _particleSystem.transform.localPosition = new Vector3(-(scaleX - 1), 0, 0);
        }
    }

    private void OnEnd(Vector2 d)
    {
        if (d.magnitude > GameManager.Ins.PlayerInput.TapRadius)
            StartCoroutine(Push());
    }

    private IEnumerator Push()
	{
        var startScaleX = _particleSystem.transform.localScale.x;
        if (startScaleX != 0)
		{
            for (float t = 0; t < 1; t += Time.deltaTime / GameManager.Ins.Player.PushDelay)
		    {
                var scaleX = Mathf.Lerp(startScaleX, 0, t);
                var scaleY = scaleX > 0.4 ? 
                    1 + (1 - scaleX) * _yStretchMul
                    : scaleX;
                _particleSystem.transform.localPosition = new Vector3(-(scaleX - 1), 0, 0);
                _particleSystem.transform.localScale = new Vector3(scaleX, scaleY, 1);

                var color = _particleMaterial.color;
                var fadeStart = 0.8f;
                if (t > fadeStart)
                    color.a = 1 - (t - fadeStart) / (1 - fadeStart);
                yield return null;
            }
		}
    }
}
