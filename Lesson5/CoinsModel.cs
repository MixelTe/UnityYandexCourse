using System;
using UnityEngine;

public class CoinsModel : MonoBehaviour
{
	public event Action Changed;
	private int Amount { get; private set; }

	public CoinsModel([Inject] DataSaver dataSaver)
	{
		Amount = dataSaver.GetCoins();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Contains("Coin"))
			OnPickupCoin();
	}

	public void OnPickupCoin()
	{
		Amount++;
		Changed?.Invoke();
	}

	public bool TryDiscard(int price)
	{
		if (Amount < price)
			return false;

		Amount -= price;
		Changed?.Invoke();

		return true;
	}
}
