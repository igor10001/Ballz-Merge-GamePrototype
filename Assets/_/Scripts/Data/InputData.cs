

using UnityEngine;

public struct DragStartComponent
{
    public Vector3 startPosition;
}

public struct DragContinueComponent
{
    public Vector3 worldPosition;
}

public struct DragEndComponent
{
    public Vector3 endPosition;
}