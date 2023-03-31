using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenVector2 : Tween {
        private static readonly Stack<TweenVector2> Pool = new ();

        public static TweenVector2 Create(Action<Vector2> setter, Vector2 from, Vector2 to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenVector2();
            tween.Target = setter;
            tween.From = from;
            tween.To = to;
            return tween;
        }

        private Action<Vector2> Target;
        private Vector2 From;
        private Vector2 To;

        public override void Kill() {
            Target = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            float x = From.x + progress * (To.x - From.x);
            float y = From.y + progress * (To.y - From.y);
            Target.Invoke(new Vector2(x, y));
        }
    }
}
