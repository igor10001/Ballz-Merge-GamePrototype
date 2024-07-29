using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LinePrediction : MonoBehaviour
{
    [Header("Linerenderer Colors")]
    public Color correctLineColor;
    public Color wrongLineColor;

    private LineRenderer lineRenderer;
    private Vector3 startPosition;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing!");
        }
    }

    public void SetCorectColor()
    {
        lineRenderer.startColor = correctLineColor;
        lineRenderer.endColor = correctLineColor;
    }

    public void SetWrongColor()
    {
        lineRenderer.startColor = correctLineColor;
        lineRenderer.endColor = correctLineColor;
    }

    public void ContinueDrag(Vector3 endPos, Vector3 startPos)
    {
        lineRenderer.SetPosition(1, endPos - startPos);
    }

    public void EndDrag()
    {
        lineRenderer.SetPosition(1, Vector3.zero);
    }
}

   