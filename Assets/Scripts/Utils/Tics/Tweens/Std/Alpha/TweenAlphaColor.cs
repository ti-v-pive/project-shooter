using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenAlphaColor : Tween {
        private static readonly Stack<TweenAlphaColor> Pool = new ();

        public static TweenAlphaColor Create(Color target, float to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenAlphaColor();
            tween.Target = target;
            tween.To = to;
            return tween;
        }

        private Color Target;
        private float From;
        private float To;
        private bool FromIsSet;

        private TweenAlphaColor() { }

        public override void Kill() {
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (!FromIsSet) {
                SetFrom(Target.a);
            }

            float t = From + progress * (To - From);
            Target = new Color(Target.r, Target.g, Target.b, t);
        }

        public TweenAlphaColor SetFrom(float value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
