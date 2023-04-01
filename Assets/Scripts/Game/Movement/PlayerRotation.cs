using UnityEngine;

namespace Game.Movement {
    public class PlayerRotation : MonoBehaviour {
        private Transform _transform;
        
        private void Update() {
            _transform ??= transform;
            _transform.LookAt(PlayerAimPosition.Instance.transform);
            
            var rotation = _transform.rotation;
            rotation.z = 0;
            _transform.rotation = rotation;
        }
    }
}
