using System;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    [SerializeField] private Line _linePrefab;
    [Min(0), SerializeField] private float _distanceBetweenPoints = .1f;
    
    [SerializeField] private Transform _linesParent;
    
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
            _currentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity, _linesParent);
        
        if (Input.GetButton("Fire1")) _currentLine.SetPosition(mousePosition);
    }
}
