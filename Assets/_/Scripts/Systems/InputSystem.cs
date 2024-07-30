using Leopotam.Ecs;
using UnityEngine;

public class InputSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;

    public void Run()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    CreateDragStartEvent(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10)));
                    break;
                case TouchPhase.Moved:
                    CreateDragContinueEvent(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10)));
                    break;
                case TouchPhase.Ended:
                    CreateDragEndEvent(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10)));
                    break;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                CreateDragStartEvent(Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10));
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                CreateDragContinueEvent(Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                CreateDragEndEvent(Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10));
            }
        }
    }

    private void CreateDragStartEvent(Vector3 position)
    {
        var entity = _world.NewEntity();
        ref var dragStart = ref entity.Get<DragStartComponent>();
        dragStart.startPosition = position;
    }

    private void CreateDragContinueEvent(Vector3 position)
    {
        var entity = _world.NewEntity();
        ref var dragContinue = ref entity.Get<DragContinueComponent>();
        dragContinue.worldPosition = position;
    }

    private void CreateDragEndEvent(Vector3 position)
    {
        var entity = _world.NewEntity();
        ref var dragEnd = ref entity.Get<DragEndComponent>();
        dragEnd.endPosition = position;
    }
}
