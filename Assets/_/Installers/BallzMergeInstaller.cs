using UnityEngine;
using Zenject;

public class BallzMergeInstaller : MonoInstaller
{
    [SerializeField] private ProjectileLauncher projectileLauncher;

    [SerializeField]
    private GridController _gridController;

    [SerializeField] private GameUIController _gameUiController;

    public override void InstallBindings()
    {
       
      
        Container.Bind<IEventAggregator>().To<EventAggregator>().AsSingle();
        Container.Bind<ProjectileLauncher>()
            .FromInstance(projectileLauncher)
            .AsSingle();
        Container.Bind<Ball>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridController>()
            .FromInstance(_gridController)
            .AsSingle();
        Container.Bind<GameUIController>()
            .FromInstance(_gameUiController)
            .AsSingle();
        
    }
}