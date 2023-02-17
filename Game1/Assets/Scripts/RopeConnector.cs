using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class RopeConnector : MonoBehaviour
{
    [SerializeField] private GameObject _connectionPoint;

    [HideInInspector] public Rigidbody2D Rigidbody2D;
	public Vector3 ConnectionPosition => _connectionPoint.transform.position;

	private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (!_connectionPoint) throw new Exception("Connection Point in not assigned");
    }
}
