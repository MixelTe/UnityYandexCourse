using System;
using System.Collections;
using UnityEngine;

public class Resources : MonoBehaviour
{

    public int Coins { get; private set; }
    [SerializeField] private UICounter _counter;

    public event Action<int> OnChangeCoins;
    public event Action<Vector3> OnCollectCoins;
    public event Action<int, Vector3> OnHitCube;

    private void Start()
    {
        OnChangeCoins?.Invoke(Coins);
    }

    public void HitCube(int value, Vector3 worldPosition)
    {
        OnHitCube.Invoke(value, worldPosition);
    }

    public void CollectCoins(int value, Vector3 worldPosition) {
        OnCollectCoins.Invoke(worldPosition);
        StartCoroutine(AddCoinsAfterDelay(value, 1f));
    }

    private IEnumerator AddCoinsAfterDelay(int value, float delay) {
        yield return new WaitForSeconds(delay);
        Coins += value;
        OnChangeCoins?.Invoke(Coins);
        _counter.Display();
    }

    public bool TryBuy(int price) {
        if (Coins >= price)
        {
            Coins -= price;
            _counter.Display();
            OnChangeCoins.Invoke(Coins);
            return true;
        }
        else {
            return false;
        }
    }


}
