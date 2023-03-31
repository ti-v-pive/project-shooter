using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenSplineY : Tween {
        private static readonly Stack<TweenSplineY> Pool = new Stack<TweenSplineY>();

        public static TweenSplineY Create(Transform target, bool local, float xFrom, float xTo, float[] yTimes,
            float[] yPoints, EaseType yEase) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenSplineY();
            tween.Target = target;
            tween.Local = local;
            tween.FromX = xFrom;
            tween.ToX = xTo;
            tween.SplineY.Init(yTimes, yPoints);
            tween.EaseY = yEase;
            tween.Z = local ? target.localPosition.z : target.position.z;
            return tween;
        }

        private Transform Target;
        private bool Local;

        private float FromX;
        private float ToX;

        private readonly Spline1D SplineY = new ();
        private EaseType EaseY;

        private float Z;

        private TweenSplineY() { }

        public override void Kill() {
            SplineY.Clear();
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            var x = FromX + progress * (ToX - FromX);

            var yProgress = EaseFunctions.Calculate(EaseY, progress);
            var y = SplineY.CalcPoint(yProgress);

            var pos = new Vector3(x, y, Z);

            if (Local) {
                Target.localPosition = pos;
            } else {
                Target.position = pos;
            }
        }
    }
}
