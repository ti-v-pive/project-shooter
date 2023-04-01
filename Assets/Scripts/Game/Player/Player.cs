using UnityEngine;

namespace Game {
    public class Player : MonoBehaviour {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerWeaponsManager _weapons;
        
        public PlayerHealth Health => _health;
        public PlayerWeaponsManager Weapons => _weapons;
    }
}
