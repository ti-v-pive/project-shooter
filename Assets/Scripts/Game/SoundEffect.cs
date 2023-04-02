using UnityEngine;

namespace Game {
    public class SoundEffect : MonoBehaviour {
        [SerializeField] private float _startTime;
        
        private void Awake() {
            var audioSource = GetComponent<AudioSource>();
            if (!audioSource) {
                return;
            }

            var clip = audioSource.clip;
            if (!clip) {
                return;
            }
            
            audioSource.Play();
            audioSource.time = _startTime;
            //audioSource.PlayScheduled(AudioSettings.dspTime + 0.1f);
            
            Destroy(gameObject, clip.length);
        }
        
        public void PlayNew() {
            Instantiate(this);
        }
    }
}
