using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenScale : Tween {
        private static readonly Stack<TweenScale> Pool = new ();

        public static TweenScale Create(Transform target, Vector2 to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenScale();
            tween.Target = target;
            tween.To = to;
            tween.Z = target.localScale.z;
            return tween;
        }

        private Transform Target;
        private Vector2 From;
        private Vector2 To;
        private float Z;

        private bool FromIsSet;

        private TweenScale() { }

        public override void Kill() {
            Target = null;
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (!FromIsSet) {
                SetFrom(Target.localScale);
            }

            float progressX = From.x + progress * (To.x - From.x);
            float progressY = From.y + progress * (To.y - From.y);
            Target.localScale = new Vector3(progressX, progressY, Z);
        }

        public TweenScale SetFrom(Vector3 value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
