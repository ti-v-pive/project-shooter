using System.Collections.Generic;
using UnityEngine;

namespace Tics {
    public class TweenAlphaSpriteRenderer : Tween {
        private static readonly Stack<TweenAlphaSpriteRenderer> Pool = new ();

        public static TweenAlphaSpriteRenderer Create(SpriteRenderer target, float to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenAlphaSpriteRenderer();
            tween.Target = target;
            tween.To = to;
            return tween;
        }

        private SpriteRenderer Target;
        private float From;
        private float To;
        private bool FromIsSet;

        private TweenAlphaSpriteRenderer() { }

        public override void Kill() {
            Target = null;
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (Target == null) {
                Debug.Log("TweenAlphaSpriteRenderer target == null, name = " + Name);
                return;
            }

            var color = Target.color;

            if (!FromIsSet) {
                SetFrom(color.a);
            }

            float t = From + progress * (To - From);
            Target.color = new Color(color.r, color.g, color.b, t);
        }

        public TweenAlphaSpriteRenderer SetFrom(float value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
