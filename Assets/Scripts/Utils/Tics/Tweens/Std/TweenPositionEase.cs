using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenPositionEase : Tween {
        private static readonly Stack<TweenPositionEase> Pool = new ();

        public static TweenPositionEase Create(Transform target, bool local, Vector2 to, EaseType easeX,
            EaseType easeY) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenPositionEase();
            tween.Target = target;
            tween.Local = local;
            tween.Z = local ? target.localPosition.z : target.position.z;
            tween.To = to;
            tween.EaseX = easeX;
            tween.EaseY = easeY;
            return tween;
        }

        private Transform Target;
        private bool Local;

        private Vector2 From;
        private Vector2 To;
        private float Z;

        private EaseType EaseX;
        private EaseType EaseY;

        private bool FromIsSet;

        private TweenPositionEase() { }

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

            var progressX = EaseFunctions.Calculate(EaseX, progress);
            var progressY = EaseFunctions.Calculate(EaseY, progress);

            var pos = new Vector3(
                From.x + progressX * (To.x - From.x),
                From.y + progressY * (To.y - From.y),
                Z);

            if (Local) {
                Target.localPosition = pos;
            } else {
                Target.position = pos;
            }
        }

        public TweenPositionEase SetFrom(Vector2 value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
