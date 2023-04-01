using UnityEngine;
using Random = UnityEngine.Random;

namespace Game {
    public class Weapon : MonoBehaviour {
        [SerializeField] private Collider _collider;
        
        [Header("Bullet")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawner;

        [Header("Shoot")]
        [SerializeField] private float _shootForce;
        [SerializeField] private float _shootSpread;

        // temp
        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Shoot();
            }

            /*if (Input.GetKey(KeyCode.W)) {
                transform.Rotate(-1, 0, 0);
                return;
            }
            
            if (Input.GetKey(KeyCode.S)) {
                transform.Rotate(1, 0, 0);
                return;
            }
            
            if (Input.GetKey(KeyCode.A)) {
                transform.Rotate(0, -1, 0);
                return;
            }
            
            if (Input.GetKey(KeyCode.D)) {
                transform.Rotate(0, 1, 0);
                return;
            }*/
        }

        private void Shoot() {
            var direction = transform.forward + GetSpread();
            
            var bullet = Instantiate(_bulletPrefab, _bulletSpawner.position, transform.rotation);
            bullet.SetForce(direction, _shootForce);

            if (_collider) {
                Physics.IgnoreCollision(bullet.Collider, _collider);
            }
        }

        private Vector3 GetSpread() => new (Random.Range(-_shootSpread, _shootSpread), 0, 0);
    }
}
