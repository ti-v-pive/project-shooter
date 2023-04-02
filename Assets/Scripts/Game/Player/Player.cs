using UnityEngine;
using Utils;

namespace Game {
    public class Player : MonoBehaviourSingleton<Player> {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerWeaponsManager _weapons;
        [SerializeField] private Collider _collider;
        
        public PlayerHealth Health => _health;
        public PlayerWeaponsManager Weapons => _weapons;
        public Collider Collider => _collider;
        public Transform ModelTransform => _collider.transform;
    }
}
