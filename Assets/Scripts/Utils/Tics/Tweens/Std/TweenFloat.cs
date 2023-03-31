using System;
using System.Collections.Generic;

namespace Tics {
    public class TweenFloat : Tween {
        private static readonly Stack<TweenFloat> Pool = new ();

        private TweenFloat() { }

        public static TweenFloat Create(Action<float> setter, float from, float to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenFloat();
            tween.Target = setter;
            tween.From = from;
            tween.To = to;
            return tween;
        }

        private Action<float> Target;
        private float From;
        private float To;

        public override void Kill() {
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            float t = From + progress * (To - From);
            Target.Invoke(t);
        }
    }
}
