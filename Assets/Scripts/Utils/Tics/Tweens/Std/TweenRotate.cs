using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenRotate : Tween {
        private static readonly Stack<TweenRotate> Pool = new ();

        public static TweenRotate Create(Transform target, Vector3 from, Vector3 to) {
            TweenRotate tween = Pool.Count > 0 ? Pool.Pop() : new TweenRotate();
            tween.Target = target;
            tween.From = from;
            tween.To = to;
            tween.FromIsSet = true;
            return tween;
        }

        public static TweenRotate Create(Transform target, Vector3 to) {
            TweenRotate tween = Pool.Count > 0 ? Pool.Pop() : new TweenRotate();
            tween.Target = target;
            tween.To = to;
            tween.FromIsSet = false;
            return tween;
        }

        private Transform Target;
        private Vector3 From;
        private Vector3 To;
        private bool FromIsSet;
        private Vector4 TempVector4;

        public override void Kill() {
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (!FromIsSet) {
                From = Target.localEulerAngles;
                FromIsSet = true;
            }

            TempVector4.x = From.x + progress * (To.x - From.x);
            TempVector4.y = From.y + progress * (To.y - From.y);
            TempVector4.z = From.z + progress * (To.z - From.z);
            Target.localEulerAngles = TempVector4;
        }
    }
}
