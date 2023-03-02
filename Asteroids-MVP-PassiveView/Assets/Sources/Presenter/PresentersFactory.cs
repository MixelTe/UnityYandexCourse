using UnityEngine;
using Asteroids.Model;
using System;

public class PresentersFactory : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Presenter _laserGunBulletTemplate;
    [SerializeField] private Presenter _defaultGunBulletTemplate;
    [SerializeField] private Presenter _asteroidTemplate;
    [SerializeField] private Presenter _asteroidPartTemplate;
    [SerializeField] private Presenter _nlo1Template;
    [SerializeField] private Presenter _nlo2Template;

    public void CreateBullet(Bullet bullet)
    {
        if(bullet is LaserGunBullet)
            CreatePresenter(_laserGunBulletTemplate, bullet);
        else
            CreatePresenter(_defaultGunBulletTemplate, bullet);
    }

    public void CreateAsteroidParts(AsteroidPresenter asteroid)
    {
        for (int i = 0; i < 4; i++)
            CreatePresenter(_asteroidPartTemplate, asteroid.Model.CreatePart());
    }

    public void CreateNlo(Nlo nlo, NloTeam team)
    {
        var template = team == NloTeam.First ? _nlo1Template : _nlo2Template;
        CreatePresenter(template, nlo);
    }

    public void CreateAsteroid(Asteroid asteroid)
    {
        AsteroidPresenter presenter = CreatePresenter(_asteroidTemplate, asteroid) as AsteroidPresenter;
        presenter.Init(this);
    }

    private Presenter CreatePresenter(Presenter template, Transformable model)
    {
        Presenter presenter = Instantiate(template);
        presenter.Init(model, _camera);

        return presenter;
    }
}
