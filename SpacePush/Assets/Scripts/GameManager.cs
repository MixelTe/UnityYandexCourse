using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Ins;
	public Player Player;
	public PlayerInput PlayerInput;
	public CameraController Camera;

	private void Awake()
	{
		if (Ins == null) Ins = this;
		else Destroy(gameObject);
	}

	private void OnEnable()
	{
		Ins = this;
	}
}
