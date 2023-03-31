using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenSpline : Tween {
        private static readonly Stack<TweenSpline> Pool = new ();

        public static TweenSpline Create(Transform target, bool local, float[] times, Vector2[] points) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenSpline();
            tween.Target = target;
            tween.Local = local;
            tween.Spline.Init(times, points);
            tween.Z = local ? target.localPosition.z : target.position.z;
            return tween;
        }

        private Transform Target;
        private bool Local;
        private readonly Spline2D Spline = new ();

        private float Z;

        private TweenSpline() { }

        public override void Kill() {
            Spline.Clear();
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            Vector2 point = Spline.CalcPoint(progress);
            var pos = new Vector3(point.x, point.y, Z);

            if (Local) {
                Target.localPosition = pos;
            } else {
                Target.position = pos;
            }
        }
    }
}
