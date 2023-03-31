using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenVector3 : Tween {
        private static readonly Stack<TweenVector3> Pool = new ();

        public static TweenVector3 Create(Action<Vector3> setter, Vector3 from, Vector3 to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenVector3();

            tween.Target = setter;
            tween.From = from;
            tween.To = to;
            return tween;
        }

        private Action<Vector3> Target;
        private Vector3 From;
        private Vector3 To;

        public override void Kill() {
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            float x = From.x + progress * (To.x - From.x);
            float y = From.y + progress * (To.y - From.y);
            float z = From.z + progress * (To.z - From.z);

            Target.Invoke(new Vector3(x, y, z));
        }
    }
}
