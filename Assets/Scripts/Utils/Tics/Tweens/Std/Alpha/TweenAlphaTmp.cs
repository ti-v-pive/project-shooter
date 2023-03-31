using System.Collections.Generic;
using TMPro;

namespace Tics {
    public class TweenAlphaTmp : Tween {
        private static readonly Stack<TweenAlphaTmp> Pool = new ();

        public static TweenAlphaTmp Create(TMP_Text target, float to) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenAlphaTmp();
            tween.Target = target;
            tween.To = to;
            return tween;
        }

        private TMP_Text Target;
        private float From;
        private float To;
        private bool FromIsSet;

        private TweenAlphaTmp() { }

        public override void Kill() {
            Target = null;
            FromIsSet = false;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            if (!FromIsSet) {
                SetFrom(Target.alpha);
            }

            Target.alpha = From + progress * (To - From);
        }

        public TweenAlphaTmp SetFrom(float value) {
            From = value;
            FromIsSet = true;
            return this;
        }
    }
}
