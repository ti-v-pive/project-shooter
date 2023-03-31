using System;
using System.Collections.Generic;

namespace Tics {
    public class TweenOnProgress : Tween {
        private static readonly Stack<TweenOnProgress> Pool = new ();

        private TweenOnProgress() { }

        public static TweenOnProgress Create(Action<float> setProgress) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenOnProgress();
            tween.OnProgress = setProgress;
            return tween;
        }

        private Action<float> OnProgress;

        public override void Kill() {
            OnProgress = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            OnProgress?.Invoke(progress);
        }
    }
}
