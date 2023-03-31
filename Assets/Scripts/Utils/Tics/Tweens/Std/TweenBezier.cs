using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenBezier : Tween {
        private static readonly Stack<TweenBezier> Pool = new ();

        public static Vector3 CalcBy(Vector2 from, Vector2 to, float position, float offset) {
            Vector2 from0 = Vector2.zero;
            Vector2 to0 = to - from;

            Vector2 mid0 = Vector2.Lerp(from0, to0, position);
            Vector2 mid = new Vector2(from.x, from.y) + mid0;

            Vector2 perpendicular = Vector2.Perpendicular(mid0.normalized) * offset;

            Vector2 by = mid + perpendicular;
            return by;
        }

        public static TweenBezier Create(Transform target, bool local, Vector2 p1, Vector2 p2) {
            var center = CalcBy(p1, p2, 0.5f, 0.5f * Vector2.Distance(p1, p2));
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenBezier();
            tween.Init(target, local, false, p1, center, p2, Vector2.zero);
            return tween;
        }

        public static TweenBezier Create(Transform target, bool local, Vector2 p1, Vector2 p2, Vector2 p3) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenBezier();
            tween.Init(target, local, false, p1, p2, p3, Vector2.zero);
            return tween;
        }

        public static TweenBezier Create(Transform target, bool local, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenBezier();
            tween.Init(target, local, true, p1, p2, p3, p4);
            return tween;
        }

        private Transform Target;
        private bool Local;

        private readonly Vector2[] Points = new Vector2[4];
        private bool Cubic;

        private float Z;

        private TweenBezier() { }

        private void Init(Transform target, bool local, bool cubic, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Target = target;
            Local = local;
            Z = local ? target.localPosition.z : target.position.z;
            Cubic = cubic;
            Points[0] = p1;
            Points[1] = p2;
            Points[2] = p3;
            Points[3] = p4;
        }

        public override void Kill() {
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            Vector3 pos = Cubic ? CalcCubic(progress) : CalcQuad(progress);
            pos.z = Z;

            if (Local) {
                Target.localPosition = pos;
            } else {
                Target.position = pos;
            }
        }

        public Vector3 CalcPos(float progress) {
            return Cubic ? CalcCubic(progress) : CalcQuad(progress);
        }

        private Vector2 CalcQuad(float t) {
            return (1 - t) * (1 - t) * Points[0]
                   + 2 * t * (1 - t) * Points[1]
                   + t * t * Points[2];
        }

        private Vector2 CalcCubic(float t) {
            return (1 - t) * (1 - t) * (1 - t) * Points[0]
                   + 3 * t * (1 - t) * (1 - t) * Points[1]
                   + 3 * t * t * (1 - t) * Points[2]
                   + t * t * t * Points[3];
        }
    }
}
