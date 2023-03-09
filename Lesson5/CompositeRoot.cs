using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
	private const string PlayerPrefsCoinsKey = "Coins";
	private CoinsModel _coinsModel;

    private void Awake()
    {
		var coins = PlayerPrefs.GetInt(PlayerPrefsCoinsKey, 0);
		_coinsModel = new CoinsModel(coins);
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
