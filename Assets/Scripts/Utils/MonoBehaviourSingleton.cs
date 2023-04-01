using UnityEngine;

namespace Utils {
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }
                _instance = FindObjectOfType<T>();
                if (_instance != null) {
                    return _instance;
                }
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T) + " (Singleton)";
                return _instance;
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            } else if (_instance != this) {
                Destroy(gameObject);
            }
        }
    }
}