using System;
using UnityEngine;
using Zenject;
 

    public class BallzMergeInstaller : MonoInstaller
    {
        public ProjectileLauncher projectileLauncher;
        public override void InstallBindings()
        {
           // Container.Bind<ProjectileLauncher>().FromInstance(projectileLauncher).AsSingle();
        }
    }
