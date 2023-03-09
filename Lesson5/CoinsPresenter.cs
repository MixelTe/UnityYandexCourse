using UnityEngine;
using UnityEngine.UI;

public class CoinsPresenter : MonoBehaviour
{
	[SerializeField] private CoinsModel _coins;
	[SerializeField] private Text _render;
	[SerializeField] private Animator _animator;

	private void OnEnable()
	{
		_coins.Changed += OnChange;
		UpdateText();
	}

	private void OnDisable()
	{
		_coins.Changed -= OnChange;
	}

	public void OnChange()
	{
		UpdateText();
		_animator.SetTrigger("OnPickupCoin");
	}

	public void UpdateText()
	{
		_render.text = $"Coins: {_coins.Amount}";
	}
}