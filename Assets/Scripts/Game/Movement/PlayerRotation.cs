using UnityEngine;

namespace Game.Movement {
    public class PlayerRotation : MonoBehaviour {
        private Transform _transform;
        
        private void Update() {
            _transform ??= transform;
            _transform.LookAt(PlayerAimPosition.Instance.transform);
            
            var rotation = _transform.rotation;
            _transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
        }
    }
}
