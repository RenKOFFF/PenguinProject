using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    [SerializeField] private float _timeToArrive;
    [SerializeField] private float _nextPointDistance = 0.1f;
    [SerializeField] private PenguinType _penguinType;
    [SerializeField] private Color _trackColor;

    [SerializeField] private GameObject _loseEffect;
    [SerializeField] private GameObject _winEffect;

    private List<Vector3> _movePoints;
    private float _moveSpeed;
    private Vector2 _currentMovePoint;
    private int _currentMovePointsIndex;

    private bool _isGameStarted;

    private SpriteRenderer _renderer;

    private Animator _animator;
    private string _isWalking = "isWalking";
    private string _isCrashed = "isCrashed";
    private string _isWon = "isWon";

    private Line _trackLine;

    private PenguinHome _home;

    public PenguinType PenguinType => _penguinType;
    public Color TrackColor => _trackColor;

    public event Action PenguinCrashed;
    public event Action PenguinHasArrived;


    private void OnEnable()
    {
        PenguinCrashed += OnPenguinCrashed;
        PenguinHasArrived += OnPenguinHasArrived;
        DrawController.Instance.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        PenguinCrashed -= OnPenguinCrashed;
        PenguinHasArrived -= OnPenguinHasArrived;
        DrawController.Instance.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted(bool isGameStarted)
    {
        _isGameStarted = isGameStarted;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_isGameStarted && _movePoints != null && _movePoints.Count > 0)
        {
            _animator.SetBool(_isWalking, true);
            _renderer.flipX = transform.position.x > _currentMovePoint.x;

            _currentMovePoint = _movePoints[0];
            
            transform.position = Vector2.MoveTowards(
                transform.position,
                _currentMovePoint,
                _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(_currentMovePoint, transform.position) < _nextPointDistance)
            {
                _movePoints.RemoveAt(0);
                _trackLine.DeletePointByIndex(0);
                
                if (_movePoints.Count == 0) SendWin();
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

        _moveSpeed = _movePoints.Count / _timeToArrive;
    }

    public void SetHome(PenguinHome currentPenguinHome)
    {
        _home = currentPenguinHome;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherPenguin = other.transform.GetComponent<Penguin>();
        if (otherPenguin)
        {
            SendLose();
            return;
        }
    }

    private void SendWin()
    {
        PenguinHasArrived?.Invoke();
    }

    private void SendLose()
    {
        PenguinCrashed?.Invoke();
    }

    private void OnPenguinHasArrived()
    {
        _isGameStarted = false;

        Instantiate(_winEffect, transform);
        _animator.SetTrigger(_isWon);

        _moveSpeed = 0;
        transform.position = _home.WinPoint.position;

        _trackLine.Clear();
        _movePoints.Clear();
    }

    private void OnPenguinCrashed()
    {
        _isGameStarted = false;
        _moveSpeed = 0;
        Instantiate(_loseEffect, transform);
        _animator.SetTrigger(_isCrashed);

        _trackLine.Clear();
        _movePoints.Clear();
    }
}

public enum PenguinType
{
    Male,
    Female,
    All
}