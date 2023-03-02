using UnityEngine;
using Asteroids.Model;

public class SpawnExample : MonoBehaviour
{
    [SerializeField] private PresentersFactory _factory;
    [SerializeField] private Root _init;

    private int _index;
    private float _secondsPerIndex = 0.07f;

    private void Update()
    {
        int newIndex = (int)(Time.time / _secondsPerIndex);

        if(newIndex > _index)
        {
            _index = newIndex;
            OnTick();
        }
    }

    private void OnTick()
    {
        float chance = Random.Range(0, 100);

        if (chance < 95)
        {
            var nlo1 = new Nlo(GetRandomPositionOutsideScreen(), Config.NloSpeed);
            var nlo2 = new Nlo(GetRandomPositionOutsideScreen(), Config.NloSpeed);
            nlo1.SetTarget(nlo2);
            nlo2.SetTarget(nlo1);
            _factory.CreateNlo(nlo1, NloTeam.First);
            _factory.CreateNlo(nlo2, NloTeam.Second);
        }
        else
        {
            Vector2 position = GetRandomPositionOutsideScreen();
            Vector2 direction = GetDirectionThroughtScreen(position);

            _factory.CreateAsteroid(new Asteroid(position, direction, Config.AsteroidSpeed));
        }
    }

    private Vector2 GetRandomPositionOutsideScreen()
    {
        return Random.insideUnitCircle.normalized + new Vector2(0.5F, 0.5F);
    }

    private static Vector2 GetDirectionThroughtScreen(Vector2 postion)
    {
        return (new Vector2(Random.value, Random.value) - postion).normalized;
    }
}
