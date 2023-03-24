using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
	[Header("Game UI")]
	[SerializeField, Rename("Panel")] private GameObject _ui;
	[SerializeField] private PoppingText _score;
	[SerializeField] private Slider _health;
	[SerializeField] private PoppingSlider _dash;
	[SerializeField] private PoppingSlider _splash;
	[Header("Game Over screen")]
	[SerializeField, Rename("Panel")] private GameObject _gameOverScreen;
	[SerializeField, Rename("Score")] private PoppingText _finalScore;

	public void UpdateScore(bool pop) =>
		_score.SetText(GameManager.Ins.Score.Score, pop);

	public void UpdateHealth(float v) =>
		_health.value = v;

	public void PopDash() =>
		_dash.Pop();

	public void PopSplash() =>
		_splash.Pop();

	public void ShowGameOverScreen()
	{
		_ui.SetActive(false);
		_gameOverScreen.SetActive(true);
		_finalScore.SetText(GameManager.Ins.Score.Score, true);
	}

	private void Update()
	{
		var player = GameManager.Ins.Player;
		_dash.SetValue(player.DashCharges);
		_splash.SetValue(player.SplashCharges);
	}
}
