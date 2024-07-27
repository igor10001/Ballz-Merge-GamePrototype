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

        Container.Bind<ProjectileLauncher>()
            .FromInstance(projectileLauncher)
            .AsSingle();
    }
}