using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
	[SerializeField] private PoppingText _score;
	[SerializeField] private Slider _health;

	public void UpdateScore(bool pop) => 
		_score.UpdateText(GameManager.Ins.Score.Score, pop);

	public void UpdateHealth(float v) =>
		_health.value = v;
}
