using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    [SerializeField] private float _moveSpeedCoefficient;
    [SerializeField] private float _nextPointDistance = 0.1f;
    [SerializeField] private PenguinType _penguinType;
    [SerializeField] private Color _trackColor;

    [SerializeField] private GameObject _loseEffect;
    [SerializeField] private GameObject _winEffect;

    private List<Vector3> _movePoints;
    private Vector2 _currentMovePoints;
    private int _currentMovePointsIndex;

    private Animator _animator;
    private string _isWalking = "isWalking";
    private string _isCrashed = "isCrashed";
    private string _isWon = "isWon";

    private Line _trackLine;

    public PenguinType PenguinType => _penguinType;
    public Color TrackColor => _trackColor;

    public event Action PenguinCrashed;
    public event Action PenguinHasArrived;


    private void OnEnable()
    {
        PenguinCrashed += OnPenguinCrashed;
        PenguinHasArrived += OnPenguinHasArrived;
    }

    private void OnDisable()
    {
        PenguinCrashed -= OnPenguinCrashed;
        PenguinHasArrived -= OnPenguinHasArrived;
    }

    private void OnPenguinHasArrived()
    {
        Instantiate(_winEffect);
        _animator.SetTrigger(_isWon);
    }

    private void OnPenguinCrashed()
    {
        Instantiate(_loseEffect);
        _animator.SetTrigger(_isCrashed);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_movePoints != null && _movePoints.Count > 0)
        {
            _animator.SetBool(_isWalking, true);

            _currentMovePoints = _movePoints[0];

            transform.position = Vector2.MoveTowards(
                transform.position,
                _currentMovePoints,
                _moveSpeedCoefficient * Time.deltaTime);

            if (Vector2.Distance(_currentMovePoints, transform.position) < _nextPointDistance)
            {
                _movePoints.RemoveAt(0);
                _trackLine.DeletePointByIndex(0);
            }
        }
        else
        {
            _animator.SetBool(_isWalking, false);
        }
    }

    public void SetPath(Line currentLine)
    {
        _movePoints = currentLine.GetPoints().ToList();
        _trackLine = currentLine;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPenguin = other.GetComponent<Penguin>();
        if (otherPenguin)
        {
            SendLoseThisAndOtherPenguin(otherPenguin);
            return;
        }
        
        var penguinHome = other.GetComponent<PenguinHome>();
        if (penguinHome)
        {
            if (penguinHome.PenguinType == PenguinType)
            {
                SendWinThis();
            }
        }
        
    }

    private void SendWinThis()
    {
        PenguinHasArrived?.Invoke();
    }

    private void SendLoseThisAndOtherPenguin(Penguin otherPenguin)
    {
        PenguinCrashed?.Invoke();
        otherPenguin.PenguinCrashed?.Invoke();
    }
}

public enum PenguinType
{
    Male,
    Female,
    All
}