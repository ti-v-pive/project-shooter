using UnityEngine;

namespace Game {
    public class ShooterSingle : Shooter {
        [SerializeField] private float _spread;

        public override Vector3 Spread => new (Random.Range(-_spread, _spread), 0, 0);

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Shoot();
            }
        }
    }
}
