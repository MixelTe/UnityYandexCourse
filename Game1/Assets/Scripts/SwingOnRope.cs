using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FixedJoint2D), typeof(Rigidbody2D))]
public class SwingOnRope : MonoBehaviour
{
    [SerializeField]
    private float _minSpeedOfSwing = 1;
    [SerializeField]
    private float _pushForce = 2;
    [SerializeField]
    private float _pushDelay = 0.5f;

    private FixedJoint2D _joint;
    private Rigidbody2D _rigidbody;
    private float _lastPushTime = 0;

    private void Start()
    {
        _joint = GetComponent<FixedJoint2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_joint.enabled && Time.timeSinceLevelLoad - _lastPushTime > _pushDelay)
        {
            var magnitude = _rigidbody.velocity.magnitude;
            if (magnitude <= _minSpeedOfSwing)
			{
                _lastPushTime = Time.timeSinceLevelLoad;
                var velocity = _rigidbody.velocity.normalized;
                var pushForce = velocity * (_pushForce * -1);
                var torqueForce = _pushForce * (velocity.x < 0 ? 1 : -1);

                _rigidbody.AddForce(pushForce, ForceMode2D.Impulse);
                _rigidbody.AddTorque(torqueForce, ForceMode2D.Impulse);
            }
        }
    }
}
