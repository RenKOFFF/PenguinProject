using System;
using _Game.Scripts;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    [Header("Line Settings")] [SerializeField]
    private Line _linePrefab;

    [Min(0), SerializeField] private float _distanceBetweenPoints = .1f;

    [Header("Other Settings")] [SerializeField]
    private Transform _linesParent;

    private Penguin _currentPenguin;
    private PenguinHome _currentPenguinHome;

    private Camera _camera;
    private Line _currentLine;

    public float DistanceBetweenPoints => _distanceBetweenPoints;
    public static DrawController Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _camera = Camera.main;

        if (_linesParent == null)
            _linesParent = transform;
    }

    private void Update()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward);
            if (!hit) return;

            _currentPenguin = hit.transform.GetComponent<Penguin>();
            if (_currentPenguin)
                _currentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity, _linesParent);
            else return;
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
            }
            else ClearCurrentLineAndPenguinsData();
        }
    }

    private void ClearCurrentLineAndPenguinsData()
    {
        if (_currentLine) Destroy(_currentLine.gameObject);
        _currentPenguin = null;
        _currentPenguinHome = null;
    }
}