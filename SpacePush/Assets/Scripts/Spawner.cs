using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnItem[] _spawnItems;
    [SerializeField] private float _spawnRate;

    private float _timeFromLastSpawn = 0;

	private void Start()
	{
		var spawnChanceMult = 1 / _spawnItems.Select(el => el.chance).Sum();
		for (int i = 0; i < _spawnItems.Length; i++)
		{
			_spawnItems[i].chance *= spawnChanceMult;
		}
	}

	void Update()
    {
        _timeFromLastSpawn += Time.deltaTime;
        if (_timeFromLastSpawn > _spawnRate)
		{
            _timeFromLastSpawn = 0;
            var enemy = GetRandomEnemy();
            Instantiate(enemy, GameField.RandomPosOutside(), Quaternion.identity, transform);
        }
    }

	public Enemy GetRandomEnemy()
	{
		var rand = Random.value;
		var v = 0f;
		foreach (var item in _spawnItems)
		{
			v += item.chance;
			if (rand <= v) return item.prefab;
		}
        return _spawnItems[^1].prefab;
    }
}

[System.Serializable]
struct SpawnItem
{
    public Enemy prefab;
    public float chance;
}
