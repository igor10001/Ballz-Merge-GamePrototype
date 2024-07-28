using UnityEngine;

public class LinePrediction : MonoBehaviour
{
    /*[Header("Linerenderer Colors")]
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

    public void StartPrediction(Vector3 startPos)
    {
        startPosition = startPos;

    }

    public void UpdatePrediction(Vector3 worldPosition)
    {
        Vector3 direction = worldPosition - startPosition;
        direction.Normalize();

        if (Mathf.Abs(Mathf.Atan2(direction.x, direction.y)) < 1.35f)
        {
            lineRenderer.startColor = correctLineColor;
            lineRenderer.endColor = correctLineColor;
        }
        else
        {
            lineRenderer.startColor = wrongLineColor;
            lineRenderer.endColor = wrongLineColor;
        }

        lineRenderer.SetPosition(1, worldPosition - startPosition);
    }

    public void EndPrediction()
    {
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
    }*/
}