using System;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class DrawController : MonoBehaviour
{
    [Header("Line Settings")] [SerializeField]
    private Line _linePrefab;

    [Min(0), SerializeField] private float _distanceBetweenPoints = .1f;

    [Header("Other Settings")] [SerializeField]
    private Transform _linesParent;

    private Dictionary<Penguin, Line> _selectedPenguins = new();

    private Penguin _currentPenguin;
    private PenguinHome _currentPenguinHome;

    private Camera _camera;
    private Line _currentLine;

    public float DistanceBetweenPoints => _distanceBetweenPoints;
    public static DrawController Instance;
    public event Action<bool> GameStarted;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _camera = Camera.main;

        if (_linesParent == null)
            _linesParent = transform;
    }

    private void OnEnable()
    {
        GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameStarted -= OnGameStarted;
    }
    
    private void Update()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward);
            if (!hit)
            {
                ClearCurrentLineAndPenguinsData();
                return;
            }

            _currentPenguin = hit.transform.GetComponent<Penguin>();

            if (_currentPenguin)
            {
                if (!_selectedPenguins.ContainsKey(_currentPenguin))
                {
                    _currentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity, _linesParent);
                    _currentLine.SetColor(_currentPenguin.TrackColor);
                    _selectedPenguins.Add(_currentPenguin, _currentLine);
                }
                else
                {
                    ClearCurrentLineAndPenguinsData();
                }
            }
            else
            {
                ClearCurrentLineAndPenguinsData();
                return;
            }
        }

        if (_currentLine && Input.GetButton("Fire1"))
            _currentLine.SetPosition(mousePosition);

        if (Input.GetButtonUp("Fire1"))
        {
            mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward);
            if (!hit)
            {
                ClearCurrentLineAndPenguinsData();
                return;
            }

            _currentPenguinHome = hit.transform.GetComponent<PenguinHome>();
            if (_currentPenguinHome &&
                (_currentPenguinHome.PenguinType == PenguinType.All ||
                 _currentPenguinHome.PenguinType == _currentPenguin.PenguinType))
            {
                _currentPenguin.SetPath(_currentLine);
                _currentPenguin.SetHome(_currentPenguinHome);

                if (_selectedPenguins.Count == GameManager.Instance.PenguinsCount)
                    GameStarted?.Invoke(true);
            }
            else ClearCurrentLineAndPenguinsData();
        }
    }
    
    private void OnGameStarted(bool onGameStarted)
    {
        enabled = false;
    }

    private void ClearCurrentLineAndPenguinsData()
    {
        if (_currentLine) Destroy(_currentLine.gameObject);

        if (_currentPenguin)
        {
            _selectedPenguins.TryGetValue(_currentPenguin, out Line line);
            if (line != null)
                Destroy(line.gameObject);

            _selectedPenguins.Remove(_currentPenguin);
        }

        _currentPenguin = null;
        _currentPenguinHome = null;
    }
}