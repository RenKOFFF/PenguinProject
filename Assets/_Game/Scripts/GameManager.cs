using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private Penguin[] _penguins;

    public int PenguinsCount => _penguins.Length;
    public static GameManager Instance;
    private int _arrivedPenguinsCount;

    public event Action GameWon; 
    public event Action GameLose; 

    private void OnEnable()
    {
        foreach (var penguin in _penguins)
        {
            if (penguin)
            {
                penguin.PenguinHasArrived += OnPenguinsArrived;
                penguin.PenguinCrashed += OnPenguinCrashed;
            }
        }
    }


    private void OnDisable()
    {
        foreach (var penguin in _penguins)
        {
            if (penguin)
            {
                penguin.PenguinHasArrived -= OnPenguinsArrived;
                penguin.PenguinCrashed -= OnPenguinCrashed;
            }
        }
    }


    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
    
    private void OnPenguinCrashed()
    {
        GameLose?.Invoke();
        Debug.Log("Lose");
    }
    
    private void OnPenguinsArrived()
    {
        _arrivedPenguinsCount++;
        if (_arrivedPenguinsCount == PenguinsCount)
        {
            GameWon?.Invoke();
            Debug.Log("Win");

        }
    }
}
