using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HingeJoint2D), typeof(Rigidbody2D))]
public class SwingOnRope : MonoBehaviour
{
    [SerializeField] private float _minSpeedOfSwing = 1;
    [SerializeField] private float _pushForce = 2;
    [SerializeField] private float _pushDelay = 0.5f;

    private HingeJoint2D _joint;
    private Rigidbody2D _rigidbody;
    private float _lastPushTime = 0;

    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var rope = _joint.connectedBody;
        if (_joint.enabled && rope && Time.timeSinceLevelLoad - _lastPushTime > _pushDelay)
        {
            var ropeSpeed = rope.velocity.x;
            var swingDirection = Mathf.Sign(rope.gameObject.transform.localPosition.x);
            if (ropeSpeed * swingDirection <= _minSpeedOfSwing)
			{
                _lastPushTime = Time.timeSinceLevelLoad;
                var pushForce = _pushForce * swingDirection * -1;
                var pushVec = new Vector2(pushForce, 0);

                _rigidbody.AddRelativeForce(pushVec, ForceMode2D.Force);
			}
        }
    }
}
