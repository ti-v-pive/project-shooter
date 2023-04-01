using System;
using UnityEngine;

namespace Game {
    public abstract class Shooter : MonoBehaviour {
        [Tooltip("Скорость вылета пули")]
        [SerializeField] private float _force = 10;
        public float Force => _force;
        
        public abstract Vector3 Spread { get; }
        
        public event Action OnShootListeners;

        protected void Shoot() {
            OnShootListeners?.Invoke();
        }
    }
}
