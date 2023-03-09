using Zenject;
using UnityEngine;
using System.Collections;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<string>().WithId("PlayerPrefsCoinsKey").FromInstance("Coins");
        Container.Bind<CoinsModel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DataSaver>().AsSingle();
    }
}
