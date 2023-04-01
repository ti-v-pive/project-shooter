using UnityEngine;

namespace Game {
    public class TreasureGrab : MonoBehaviour {
        private void OnCollisionEnter(Collision collision) {
            var target = collision.gameObject?.GetComponent<Treasure>();
            if (!target) {
                return;
            }
            
            target.Accept();
            target.Die();
        }
    }
}
