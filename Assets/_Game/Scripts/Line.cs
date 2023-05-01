using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    public void SetPosition(Vector2 position)
    {
        if (!CanAppendPosition(position)) return;

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
    }

    private bool CanAppendPosition(Vector2 position)
    {
        if (_lineRenderer.positionCount == 0) return true;
        
        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), position) >
               DrawController.Instance.DistanceBetweenPoints;
    }
}
