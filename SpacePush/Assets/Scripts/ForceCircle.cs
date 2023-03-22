using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCircle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Material _particleMaterial;
    [SerializeField] private float _xPos;
    [SerializeField] private float _fullColorDistance;
    [SerializeField] private Vector2 _stretchMul;
    [SerializeField] private float _minScale;
    [SerializeField] private float _returnScaleTime;
    private bool _swipeStarted = false;

    private void Start()
    {
        _particleSystem.transform.localScale = Vector3.one * _minScale;
        _particleSystem.transform.localPosition = Vector3.right * _xPos;
    }
	private void OnEnable()
	{
        GameManager.Ins.PlayerInput.OnSwiping += OnSwiping;
        GameManager.Ins.PlayerInput.OnEnd += OnEnd;
    }
	private void OnDisable()
	{
        GameManager.Ins.PlayerInput.OnSwiping += OnSwiping;
        GameManager.Ins.PlayerInput.OnEnd += OnEnd;
    }

	private void OnSwiping(Vector2 d)
    {
        if (GameManager.Ins.Player.State != PlayerState.Idle) return;
        _swipeStarted = true;

        var magnitude = d.magnitude;
        if (magnitude < GameManager.Ins.PlayerInput.TapRadius)
		{
            var t = magnitude / GameManager.Ins.PlayerInput.TapRadius;
            var scale = Mathf.Lerp(_minScale, 1, t);
            _particleSystem.transform.localScale = Vector3.one * scale;
        }
        else
		{
            var t = Mathf.Min((magnitude - GameManager.Ins.PlayerInput.TapRadius) / GameManager.Ins.PlayerInput.SwipeRadius, 1);
            var scaleX = t * _stretchMul.x + 1;
            var scaleY = 1 + (1 - scaleX) * _stretchMul.y;
            _particleSystem.transform.localScale = new Vector3(scaleX, scaleY, 1);
            _particleSystem.transform.localPosition = new Vector3(_xPos + (scaleX - 1), 0, 0);
        }
    }

    private void OnEnd(Vector2 d)
    {
        if (!_swipeStarted) return;
        _swipeStarted = false;
        var magnitude = d.magnitude;
        if (magnitude > GameManager.Ins.PlayerInput.TapRadius)
            StartCoroutine(Push(magnitude));
        else
            StartCoroutine(Collapse());
    }

    private IEnumerator Push(float magnitude)
	{
        var T = Mathf.Min((magnitude - GameManager.Ins.PlayerInput.TapRadius) / GameManager.Ins.PlayerInput.SwipeRadius, 1);
        var startScaleX = _particleSystem.transform.localScale.x;
        var maxYscale = 0f;
        var maxYscaleTime = -1f;
        for (float t = 0; t < 1; t += Time.deltaTime / GameManager.Ins.Player.PushDelay)
        {
            var scaleX = Mathf.Lerp(startScaleX, _minScale, t);
            var scaleYd = (1 - scaleX) * _stretchMul.y;
            var scaleY = scaleYd < 0 ? 1 + scaleYd : (1 + scaleYd) * T + scaleX * (1 - T);
            if (t > 0.8f)
            {
                if (maxYscaleTime < 0) 
                {
                    maxYscale = scaleY;
                    maxYscaleTime = t; 
                }
                scaleY = Mathf.Lerp(maxYscale, _minScale, (t - maxYscaleTime) / (1 - maxYscaleTime));
            }

            _particleSystem.transform.localScale = new Vector3(scaleX, scaleY, 1);
            _particleSystem.transform.localPosition = new Vector3(_xPos + scaleX, 0, 0);
            yield return null;
        }
        _particleSystem.transform.localScale = Vector3.one * _minScale;
        _particleSystem.transform.localPosition = Vector3.right * _xPos;
    }

    private IEnumerator Collapse()
    {
        var startScale = _particleSystem.transform.localScale.x;
        for (float t = 0; t < 1; t += Time.deltaTime / _returnScaleTime)
        {
            var scale = Mathf.Lerp(startScale, _minScale, t);
            _particleSystem.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        _particleSystem.transform.localScale = Vector3.one * _minScale;
        _particleSystem.transform.localPosition = Vector3.right * _xPos;
    }

    private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + Vector3.right * _xPos, _minScale / 2 * 4);
    }
}
