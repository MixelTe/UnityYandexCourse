using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FixedJoint2D))]
public class SwingOnRope : MonoBehaviour
{
    [SerializeField]
    private float _minSpeedOfSwing = 1;
    [SerializeField]
    private float _pushForce = 2;
    [SerializeField]
    private float _pushDelay = 0.5f;

    private FixedJoint2D _joint;
    private float _lastPushTime = 0;

    private void Start()
    {
        _joint = GetComponent<FixedJoint2D>();
    }

    private void Update()
    {
        var rope = _joint.connectedBody;
        if (_joint.enabled && rope && Time.timeSinceLevelLoad - _lastPushTime > _pushDelay)
        {
            var ropeSpeed = Mathf.Abs(rope.velocity.x);
            if (ropeSpeed <= _minSpeedOfSwing)
			{
                _lastPushTime = Time.timeSinceLevelLoad;
                var ropeLocalPos = rope.gameObject.transform.localPosition;
                var pushForce = _pushForce * Mathf.Sign(ropeLocalPos.x) * -1;
                var pushVec = new Vector2(pushForce, 0);

                SwingRope(rope, pushVec);
			}
        }
    }

    private void SwingRope(Rigidbody2D rope, Vector2 force)
	{
        rope.AddRelativeForce(force, ForceMode2D.Impulse);

        var joint = rope.GetComponent<FixedJoint2D>();
        if (joint && joint.connectedBody)
            SwingRope(joint.connectedBody, force / 2);
    }
}
