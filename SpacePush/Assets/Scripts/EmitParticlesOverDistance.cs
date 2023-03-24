using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EmitParticlesOverDistance : MonoBehaviour
{
    [SerializeField] private float _rateOverDistance;
    [SerializeField] private float _maxDistance;

    private ParticleSystem _particleSystem;
    private Vector3 _lastPos;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        var pos = transform.position;
        var magnitude = (pos - _lastPos).magnitude;
        if (magnitude < _maxDistance)
            _particleSystem.Emit(Mathf.RoundToInt(magnitude * _rateOverDistance));
        _lastPos = pos;
    }
}
