using UnityEngine;

namespace Game.Movement {
    public class PlayerRotation : MonoBehaviour{
        private void Update() {
            transform.LookAt(PlayerAimPosition.Instance.transform);
        }
    }
}
