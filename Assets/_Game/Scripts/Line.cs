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

    public Vector3[] GetPoints()
    {
        Vector3[] points = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(points);
        return points;
    }

    public void DeletePointByIndex(int currentMovePointsIndex)
    {
        if (currentMovePointsIndex <= _lineRenderer.positionCount - 1)
        {
            var points = GetPoints();
            _lineRenderer.positionCount = 0;

            for (int i = 0; i < points.Length; i++)
            {
                if (i != currentMovePointsIndex)
                {
                    SetPosition(points[i]);
                }
            }
        }
        if (_lineRenderer.positionCount == 0) Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
}