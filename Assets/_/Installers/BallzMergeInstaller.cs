using UnityEngine;
using Zenject;

public class BallzMergeInstaller : MonoInstaller
{
    [SerializeField] private ProjectileLauncher projectileLauncher; 

    public override void InstallBindings()
    {
       
        if (projectileLauncher == null)
        {
            Debug.LogError("ProjectileLauncher reference is missing in BallzMergeInstaller.");
            return;
        }
        Container.Bind<IEventAggregator>().To<EventAggregator>().AsSingle();
        Container.Bind<ProjectileLauncher>()
            .FromInstance(projectileLauncher)
            .AsSingle();
        Container.Bind<Ball>().FromComponentInHierarchy().AsSingle();
        
    }
}