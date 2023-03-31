using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tics {
    public class TweenAlphaImageRenderer : Tween {
        private static readonly Stack<TweenAlphaImageRenderer> Pool = new ();

        public static TweenAlphaImageRenderer Create(Image target, float to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenAlphaImageRenderer();
            tween.Target = target;
            tween.To = to;
            return tween;
        }

        private Image Target;
        private float From;
        private float To;
        private bool FromIsSet;

        private TweenAlphaImageRenderer() { }

        public override void Kill() {
            Target = null;
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            var color = Target.color;

            if (!FromIsSet) {
                SetFrom(color.a);
            }

            float t = From + progress * (To - From);
            Target.color = new Color(color.r, color.g, color.b, t);
        }

        public TweenAlphaImageRenderer SetFrom(float value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
