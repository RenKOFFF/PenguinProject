using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _nextPointDistance = 0.1f;
    [SerializeField] private PenguinType _penguinType;

    private Vector3[] _movePoints;
    private Vector2 _currentMovePoints;
    private int _currentMovePointsIndex;

    public PenguinType PenguinType => _penguinType;

    private void Update()
    {
        if (_movePoints != null)
        {
            if (_currentMovePointsIndex == 0)
                _currentMovePoints = _movePoints[_currentMovePointsIndex];

            transform.position = Vector2.MoveTowards(
                transform.position,
                _currentMovePoints,
                _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(_currentMovePoints, transform.position) < _nextPointDistance)
                if (_currentMovePointsIndex + 1 < _movePoints.Length - 1)
                    _currentMovePoints = _movePoints[++_currentMovePointsIndex];
        }
    }

    public void SetPath(Line currentLine)
    {
        _movePoints = currentLine.GetPoints();
    }

    /*public void ClearPath()
    {
        _movePoints = null;
    }*/
}

public enum PenguinType
{
    Male,
    Female,
    All
}