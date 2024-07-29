// InputHandler.cs

using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public delegate void OnDragStartHandler(Vector3 startPosition);
    public delegate void OnDragHandler(Vector3 worldPosition);
    public delegate void OnDragEndHandler(Vector3 endPosition);

    public event OnDragStartHandler OnDragStart;
    public event OnDragHandler OnDrag;
    public event OnDragEndHandler OnDragEnd;

    

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0) // Check if there's at least one touch
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnDragStart?.Invoke(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10)));
                    break;
                case TouchPhase.Moved:
                    OnDrag?.Invoke(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10)));
                    break;
                case TouchPhase.Ended:
                    OnDragEnd?.Invoke(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10)));
                    break;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                OnDragStart?.Invoke(Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10));
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                OnDrag?.Invoke(Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                OnDragEnd?.Invoke(Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10));
            }
        }
    }
}
