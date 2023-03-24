using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [Header("Meteor")]
    [SerializeField, Rename("Prefab")] private Meteor _meteor;
    [SerializeField, Rename("Spawn Rate")] private float _meteorSpawnRate;
    [Header("Following Enemy")]
    [SerializeField, Rename("Prefab")] private FollowingEnemy _followingEnemy;
    [SerializeField, Rename("Max Count")] private int _followingEnemyMaxCount;
    [SerializeField, Rename("Spawn Interval")] private float _followingEnemySpawnInterval;
    [Header("Waves")]
    [SerializeField] private float _waveTime;

    private float _timeFromLastMeteorSpawn = 0;
    private float _timeFromLastWave = 0;
    private int _wave = 0;

    void Update()
    {
        if (!GameManager.Ins.GameRunning) return;

        _timeFromLastMeteorSpawn += Time.deltaTime;
        if (_timeFromLastMeteorSpawn > _meteorSpawnRate)
		{
            _timeFromLastMeteorSpawn = 0;
            Instantiate(_meteor, GameField.RandomPosOutside(), Quaternion.identity, transform);
        }
        _timeFromLastWave += Time.deltaTime;
        if (_timeFromLastWave > _waveTime)
        {
            _timeFromLastWave = 0;
            StartWave();
        }
    }

    private void StartWave()
	{
        _wave++;
        SpawnFollowingEnemies();
    }

    private void SpawnFollowingEnemies()
    {
        var followingEnemyCount = Mathf.Min(_wave * 2, _followingEnemyMaxCount);
        var spawnSide = GameField.RandomSide();
        var spawnPos = GameField.RandomPosOutside(spawnSide);
        foreach (var pos in TrianglePositions(spawnPos, spawnSide, followingEnemyCount, _followingEnemySpawnInterval))
        {
            Instantiate(_followingEnemy, pos, Quaternion.identity, transform);
        }
    }

    private IEnumerable<Vector2> TrianglePositions(Vector2 startPos, Vector2 spawnSide, int count, float space)
	{
        var row = 0;
        var itemInRow = 0;
        var maxItemsInRow = 1;
		for (int i = 0; i < count; i++)
		{
            if (itemInRow >= maxItemsInRow)
			{
                itemInRow = 0;
                row++;
                maxItemsInRow = 1 + row * 2;
			}
            itemInRow++;

            var d1 = row;
            var d2 = itemInRow - maxItemsInRow / 2;

            var x = startPos.x + space * (d1 * spawnSide.x + d2 * (1 - Mathf.Abs(spawnSide.x)));
            var y = startPos.y + space * (d1 * spawnSide.y + d2 * (1 - Mathf.Abs(spawnSide.y)));

            yield return new Vector2(x, y);
        }
	}
}
