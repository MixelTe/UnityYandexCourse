using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
	public int Score { get; private set; } = 0;

	private void Start()
	{
		GameManager.Ins.StatsDisplay.UpdateScore(false);
	}

	public void AddScore(int v)
	{
		Score += v;
		GameManager.Ins.StatsDisplay.UpdateScore(true);
	}
}
