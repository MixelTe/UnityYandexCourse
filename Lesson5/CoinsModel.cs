using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinsModel : MonoBehaviour
{
	public event Action Changed;
	private const string PlayerPrefsKey = "Coins";
	private int Amount { get; private set; }

	public CoinsModel()
	{
		Amount = PlayerPrefs.GetInt(PlayerPrefsKey, 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Contains("Coin"))
			OnPickupCoin();
	}

	public void OnPickupCoin()
	{
		Amount++;
		PlayerPrefs.SetInt(PlayerPrefsKey, Amount);
		Changed?.Invoke();
	}

	public bool TryDiscard(int price)
	{
		if (Amount < price)
			return false;

		Amount -= price;

		PlayerPrefs.SetInt(PlayerPrefsKey, Amount);
		Changed?.Invoke();

		return true;
	}
}
