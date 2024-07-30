using Leopotam.Ecs;
using Zenject;

public class DragEventHandlingSystem : IEcsRunSystem
{
    private EcsFilter<DragStartComponent> _dragStartFilter;
    private EcsFilter<DragContinueComponent> _dragContinueFilter;
    private EcsFilter<DragEndComponent> _dragEndFilter;

    [Inject] private ProjectileLauncher _projectileLauncher;

    public void Run()
    {
        foreach (var i in _dragStartFilter)
        {
            ref var dragStart = ref _dragStartFilter.Get1(i);
            _projectileLauncher.HandleDragStart(dragStart.startPosition);
            _dragStartFilter.GetEntity(i).Destroy();
        }

        foreach (var i in _dragContinueFilter)
        {
            ref var dragContinue = ref _dragContinueFilter.Get1(i);
            _projectileLauncher.HandleDrag(dragContinue.worldPosition);
            _dragContinueFilter.GetEntity(i).Destroy();
        }

        foreach (var i in _dragEndFilter)
        {
            ref var dragEnd = ref _dragEndFilter.Get1(i);
            _projectileLauncher.HandleDragEnd(dragEnd.endPosition);
            _dragEndFilter.GetEntity(i).Destroy();
        }
    }
}