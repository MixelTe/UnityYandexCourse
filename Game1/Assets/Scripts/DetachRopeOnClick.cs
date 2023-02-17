using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HingeJoint2D))]
public class DetachRopeOnClick : MonoBehaviour
{
    private HingeJoint2D _joint;
    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
            _joint.enabled = false;
        }
	}
}
