using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FixedJoint2D))]
public class DetachRopeOnClick : MonoBehaviour
{
    private FixedJoint2D _joint;
    private void Start()
    {
        _joint = GetComponent<FixedJoint2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
            _joint.enabled = false;
        }
	}
}
