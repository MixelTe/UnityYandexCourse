using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Ins;
	public Player Player;
	public PlayerInput PlayerInput;
	public CameraController Camera;
	public GameScore Score;
	public StatsDisplay StatsDisplay;
	public bool GameRunning { get; private set; } = true;

	private void Awake()
	{
		if (Ins == null) Ins = this;
		else Destroy(gameObject);
	}

	private void OnEnable()
	{
		Ins = this;
	}

	public void EndGame()
	{
		GameRunning = false;
		StatsDisplay.ShowGameOverScreen();
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
