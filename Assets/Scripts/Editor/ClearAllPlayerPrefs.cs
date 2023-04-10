using UnityEditor;
using UnityEngine;

namespace Game.Editor {
    public class ClearAllPlayerPrefs {
        [MenuItem("Tools/Clear All PlayerPrefs")]
        public static void ClearAll() {
            PlayerPrefs.DeleteAll();
        }
    }
}