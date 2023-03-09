using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSaver : MonoBehaviour
{
	[Inject(Id = "PlayerPrefsCoinsKey")]
	private const string PlayerPrefsCoinsKey = "";
	[Inject]
	private CoinsModel _coinsModel;

	public int GetCoins()
	{
		return PlayerPrefs.GetInt(PlayerPrefsCoinsKey, 0);
	}

	private void OnEnable()
	{
		_coinsModel.Changed += OnCoinsChange;
	}

	private void OnDisable()
	{
		_coinsModel.Changed -= OnCoinsChange;
	}

	private void OnCoinsChange()
	{
		PlayerPrefs.SetInt(PlayerPrefsCoinsKey, _coinsModel.Amount);
	}
}
