using UnityEngine;

namespace _Game.Scripts
{
    public class PenguinHome : MonoBehaviour
    {
        [SerializeField] private PenguinType _penguinType;
        
        public PenguinType PenguinType => _penguinType;
    }
}