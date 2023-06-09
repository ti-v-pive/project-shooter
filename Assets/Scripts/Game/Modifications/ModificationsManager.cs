﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class ModificationsManager {
        private readonly Dictionary<ModificationType, float> _endTimes = new();

        public void Clear() {
            _endTimes.Clear();
        }

        public void StartModification(ModificationType type, float duration) {
            var endTime = Math.Max(Time.time + duration, GetEndTime(type));
            SetEndTime(type, endTime);
        }

        public bool IsActive(ModificationType type) => TimeLeft(type) > 0;
        public float TimeLeft(ModificationType type) => GetEndTime(type) - Time.time;
        
        public bool IsShield => IsActive(ModificationType.Shield);
        public bool IsDoubleDamage => IsActive(ModificationType.DoubleDamage);

        private float GetEndTime(ModificationType type) => _endTimes.ContainsKey(type) ? _endTimes[type] : 0;
        private void SetEndTime(ModificationType type, float endTime) {
            if (_endTimes.ContainsKey(type)) {
                _endTimes[type] = endTime;
            } else {
                _endTimes.Add(type, endTime);
            }
        }
    }
}
