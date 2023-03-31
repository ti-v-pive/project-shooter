using System;
using System.Collections.Generic;

namespace Tics {
    public class TweenOnUpdate : Tween {
        private static readonly Stack<TweenOnUpdate> Pool = new ();

        private TweenOnUpdate() { }

        public static TweenOnUpdate Create(Action onUpdate) {
            var tween = Pool.Count > 0 ? Pool.Pop() : new TweenOnUpdate();
            tween.OnUpdate = onUpdate;
            return tween;
        }

        private Action OnUpdate;

        public override void Kill() {
            OnUpdate = null;
            Pool.Push(this);
        }

        public override void Update(float progress) {
            OnUpdate?.Invoke();
        }
    }
}
