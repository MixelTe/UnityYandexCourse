using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    [SerializeField] private Text _render;
    [SerializeField] private Animator _animator;

    private int _amount;

    private void Awake()
    {
        _amount = PlayerPrefs.GetInt("Coins", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Coin"))
            OnPickupCoin();
    }

    public void OnPickupCoin()
    {
        _amount++;
        _render.text = $"Coins: {_amount}";
        _animator.SetTrigger("OnPickupCoin");
        PlayerPrefs.SetInt("Coins", _amount);
    }

    public bool TryDiscard(int price)
    {
        if (_amount < price)
            return false;

        _amount -= price;

        _render.text = $"Coins: {_amount}";
        _animator.SetTrigger("OnPickupCoin");
        PlayerPrefs.SetInt("Coins", _amount);

        return true;
    }
}
