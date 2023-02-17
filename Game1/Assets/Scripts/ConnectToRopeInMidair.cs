using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HingeJoint2D), typeof(Rigidbody2D))]
public class ConnectToRopeInMidair : MonoBehaviour
{
    [SerializeField] private GameObject _connectionPoint;
    private HingeJoint2D _joint;
    private RopeConnector _lastConnector;
    private Rigidbody2D _rigidbody;
    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_joint.enabled)
            _lastConnector = _joint.connectedBody.GetComponent<RopeConnector>();
        if (!_connectionPoint) throw new Exception("Connection Point in not assigned");
    }

    private void OnTriggerStay2D(Collider2D collision)
	{
        if (!_joint.enabled
            && collision.TryGetComponent<RopeConnector>(out var rope)
            && _lastConnector != rope)
		{
            _joint.enabled = true;
            _lastConnector = rope;
            _rigidbody.rotation = rope.Rigidbody2D.rotation;
            _rigidbody.position = rope.ConnectionPosition - _connectionPoint.transform.localPosition;
            _joint.connectedBody = rope.Rigidbody2D;
        }
	}
}
