using UnityEngine;

namespace _Game.Scripts
{
    public class PenguinHome : MonoBehaviour
    {
        [SerializeField] private PenguinType _penguinType;
        [SerializeField] private Transform _winPoint;
        
        public PenguinType PenguinType => _penguinType;
        public Transform WinPoint => _winPoint;
    }
}