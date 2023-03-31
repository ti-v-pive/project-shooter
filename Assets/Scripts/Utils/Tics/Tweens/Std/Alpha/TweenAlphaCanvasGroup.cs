using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenAlphaCanvasGroup : Tween {
        private static readonly Stack<TweenAlphaCanvasGroup> Pool = new ();

        public static TweenAlphaCanvasGroup Create(CanvasGroup target, float to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenAlphaCanvasGroup();
            tween.Target = target;
            tween.To = to;
            return tween;
        }

        private CanvasGroup Target;
        private float From;
        private float To;
        private bool FromIsSet;

        private TweenAlphaCanvasGroup() { }

        public override void Kill() {
            Target = null;
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (!FromIsSet) {
                SetFrom(Target.alpha);
            }

            Target.alpha = From + progress * (To - From);
        }

        public TweenAlphaCanvasGroup SetFrom(float value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
