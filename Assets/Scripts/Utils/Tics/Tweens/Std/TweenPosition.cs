using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenPosition : Tween {
        private static readonly Stack<TweenPosition> Pool = new ();

        public static TweenPosition Create(Transform target, bool local, Vector2 to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenPosition();
            tween.Local = local;
            tween.Target = target;
            tween.To = to;
            tween.Z = target.localPosition.z;
            return tween;
        }

        private Transform Target;
        private bool Local;

        private Vector2 From;
        private Vector2 To;
        private float Z;

        private bool FromIsSet;

        private TweenPosition() { }

        public override void Kill() {
            Target = null;
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (!FromIsSet) {
                var from = Local ? Target.localPosition : Target.position;
                SetFrom(from);
            }

            float x = From.x + progress * (To.x - From.x);
            float y = From.y + progress * (To.y - From.y);

            if (Local) {
                Target.localPosition = new Vector3(x, y, Z);
            } else {
                Target.position = new Vector3(x, y, Z);
            }
        }

        public TweenPosition SetFrom(Vector2 value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
