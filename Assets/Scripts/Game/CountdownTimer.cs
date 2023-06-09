﻿using TMPro;
using UnityEngine;
using Utils;

namespace Game {
    public class CountdownTimer : MonoBehaviourSingleton<CountdownTimer> {
        public float timeRemaining = 60f;
        public TMP_Text countdownText;

        private bool _isActive;

        protected override void Awake() {
            base.Awake();
            _isActive = true;
        }

        private void Update() {
            if (!Main.IsGameStarted) {
                return;
            }
            if (!_isActive) {
                return;
            }

            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeRemaining / 60f);
                int seconds = Mathf.FloorToInt(timeRemaining % 60f);
                countdownText.text = $"{minutes:00}:{seconds:00}";
            } else {
                _isActive = false;

                timeRemaining = 0f;
                countdownText.text = "00:00";

                Main.Instance.Lose();
            }
        }
    }
}


