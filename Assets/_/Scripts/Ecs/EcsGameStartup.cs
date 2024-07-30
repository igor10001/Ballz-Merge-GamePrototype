using Leopotam.Ecs;
using UnityEngine;
using Zenject;

public class EcsGameStartup : MonoBehaviour
{
    private EcsWorld ecsWorld;
    private EcsSystems systems;
    [SerializeField] private ProjectileLauncher _projectileLauncher;
    private void Start()
    {
        ecsWorld = new EcsWorld();
        systems = new EcsSystems(ecsWorld); 
    	
        systems
            .Add(new InputSystem()) 
            .Add(new DragEventHandlingSystem())
            .Inject(_projectileLauncher)
            .Init(); 
    }
 
    private void Update()
    {
                systems?.Run(); 
    }
 
    private void OnDestroy()
    {
        systems?.Destroy(); 
        systems = null;
        ecsWorld?.Destroy(); 
        ecsWorld = null;
    }
}