using System;
using UnityEngine;
using Cinemachine;
using UniRx;

namespace Game.Animation {
    public class CameraShakeOnShoot : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float shakeDuration = 0.3f;
        [SerializeField] private float shakeAmplitude = 1.2f;
        [SerializeField] private float shakeFrequency = 2.0f;

        private CinemachineBasicMultiChannelPerlin _noise;

        private void Awake() {
            _noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update() {
            if (!Input.GetMouseButtonDown(0)) {
                return;
            }
            StartShake();
            Observable.Timer(TimeSpan.FromSeconds(shakeDuration)).Subscribe(_ => StopShake());
        }

        private void StartShake() {
            _noise.m_AmplitudeGain = shakeAmplitude;
            _noise.m_FrequencyGain = shakeFrequency;
        }

        private void StopShake() {
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }
    }
}