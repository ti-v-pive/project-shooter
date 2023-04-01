using UnityEngine;

namespace Game {
    public class Initialization : MonoBehaviour {
        private void Awake() {
            
        }

        private void OnApplicationFocus(bool hasFocus) {
            if (hasFocus) {
                Cursor.visible = false;
            }
        }
    }
}