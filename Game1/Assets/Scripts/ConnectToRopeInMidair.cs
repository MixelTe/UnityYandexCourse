using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FixedJoint2D), typeof(Rigidbody2D))]
public class ConnectToRopeInMidair : MonoBehaviour
{
    private FixedJoint2D _joint;
    private GameObject _lastConnector;
    private Rigidbody2D _rigidbody;
    private void Start()
    {
        _joint = GetComponent<FixedJoint2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_joint.enabled)
            _lastConnector = _joint.connectedBody.gameObject;
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
        if (!_joint.enabled
            && collision.CompareTag("RopeConnector")
            && _lastConnector != collision.gameObject)
		{
            var collisionRB = collision.gameObject.GetComponent<Rigidbody2D>();
            _joint.enabled = true;
            _lastConnector = collision.gameObject;
            _rigidbody.rotation = collisionRB.rotation;
            _joint.connectedBody = collisionRB;
        }
	}
}
