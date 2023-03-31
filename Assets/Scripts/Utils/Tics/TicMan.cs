using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tics {
    public class TicMan {
        private static int NumCreated;
        private static int NumCached;
        private static TicMan First;

        /**
		 * Call this every frame
		 */
        public static void Update() {
            Tic.Update(Time.deltaTime);
        }

        public static string GetLogMessage() {
            return "Tweens:Tics  active/cached/total\n" + (NumCreated - NumCached) + "/" + NumCached + "/" +
                   NumCreated + " : " + (Tic.NumCreated - Tic.NumCached) + "/" + Tic.NumCached + "/" +
                   Tic.NumCreated;
        }

        public static void PauseGroup(TicGroup group) {
            if (group == TicGroup.Default) {
                throw new Exception("Pausing the default tween group is not allowed!!!");
            }

            Tic.PauseGroup(group);
        }

        public static void UnpauseGroup(TicGroup group) {
            Tic.UnpauseGroup(group);
        }
        
        public static TicMan Create(Action onComplete = null, TicGroup group = TicGroup.Default) {
            if (First == null) {
                return new TicMan {Group = group};
            }

            var tween = First;
            First = First.Next;
            NumCached--;
            tween.Group = group;
            tween.OnComplete = onComplete;
            return tween;
        }

        private TicGroup Group;
        private readonly List<Tic> Children = new();
        private Action OnComplete;
        private TicMan Next;

        public bool IsActive => Children.Count > 0;

        private TicMan() {
            // Use GameTween.Create()
            NumCreated++;
        }

        public void Kill() {
            Reset();
            Cache();
        }

        private void Cache() {
            Next = First;
            First = this;
            NumCached++;
        }

        public void Reset() {
            Reset(null);
        }

        public void Reset(Action onComplete) {
            foreach (var child in Children) {
                child.Stop();
            }

            Children.Clear();

            OnComplete = onComplete;
        }

        private Tic Start(Tween tween, float duration) {
            /*if (duration == 0 && tween != null){
                Log("GameTween bad duration = 0. Fix this!"); // maybe turn on?
            }*/
            var tic = Tic.Create().Start(this, tween, duration);
            tic.Group = Group;
            Children.Add(tic);
            return tic;
        }

        internal void OnChildComplete(Tic child) {
            Children.Remove(child);

            if (Children.Count <= 0) {
                Complete();
            }
        }

        private void Complete() {
            var callback = OnComplete;
            Reset();
            callback?.Invoke();
        }

        public float CalcTimeLeft() {
            float left = 0;
            foreach (var child in Children) {
                var leftChild = child.CalcTimeLeft();
                if (left < leftChild) {
                    left = leftChild;
                }
            }

            return left;
        }

        public void Wait(float delay) => Start(null, 0).SetDelay(delay);
        public void DoAfter(float delay, Action func) => Start(null, 0).SetDelay(delay).SetOnComplete(func);
        public void DoEvery(float duration, Action func) => Start(null, duration).SetRepeat(-1).SetOnRepeat(func);

        public Tic DoInfin(Action onUpdate) => Start(TweenOnUpdate.Create(onUpdate), -1);
        public Tic DoUpdate(Action onUpdate, float duration) => Start(TweenOnUpdate.Create(onUpdate), duration);

        public Tic DoProgress(Action<float> onProgress, float duration) =>
            Start(TweenOnProgress.Create(onProgress), duration);

        public Tic FromTo(Action<float> setter, float from, float to, float duration) =>
            Start(TweenFloat.Create(setter, from, to), duration);

        public Tic FromTo(Action<Vector2> setter, Vector2 from, Vector2 to, float duration) =>
            Start(TweenVector2.Create(setter, from, to), duration);

        public Tic FromTo(Action<Vector3> setter, Vector3 from, Vector3 to, float duration) =>
            Start(TweenVector3.Create(setter, from, to), duration);

        public Tic MoveLocal(Transform target, Vector2 to, float duration) => 
            Start(TweenPosition.Create(target, true, to), duration);

        public Tic MoveLocal(Transform target, Vector2 from, Vector2 to, float duration) => 
            Start(TweenPosition.Create(target, true, to).SetFrom(from), duration);

        public Tic MoveLocal(Transform target, Vector2 to, EaseType easeX, EaseType easeY, float duration) =>
            Start(TweenPositionEase.Create(target, true, to, easeX, easeY), duration);

        public Tic MoveLocal(Transform target, Vector2 from, Vector2 to, EaseType easeX, EaseType easeY, float duration) => 
            Start(TweenPositionEase.Create(target, true, to, easeX, easeY).SetFrom(from), duration);

        public Tic MoveLocal(Transform target, float[] times, Vector2[] points, float duration) =>
            Start(TweenSpline.Create(target, true, times, points), duration);

        public Tic MoveLocal(Transform target, float xFrom, float xTo, float[] yTimes, float[] yPoints, EaseType yEase,
            float duration) => Start(TweenSplineY.Create(target, true, xFrom, xTo, yTimes, yPoints, yEase), duration);

        public Tic MoveGlobal(Transform target, Vector2 to, float duration) =>
            Start(TweenPosition.Create(target, false, to), duration);

        public Tic MoveGlobal(Transform target, float[] times, Vector2[] points, float duration) =>
            Start(TweenSpline.Create(target, false, times, points), duration);

        public Tic MoveGlobal(Transform target, Vector2 from, Vector2 to, float duration) =>
            Start(TweenPosition.Create(target, false, to).SetFrom(from), duration);

        public Tic MoveGlobal(Transform target, Vector2 from, Vector2 to, EaseType easeX, EaseType easeY,
            float duration) => Start(TweenPositionEase.Create(target, false, to, easeX, easeY).SetFrom(from), duration);

        public Tic MoveGlobal(Transform target, Vector2 to, EaseType easeX, EaseType easeY, float duration) =>
            Start(TweenPositionEase.Create(target, false, to, easeX, easeY), duration);

        public Tic MoveGlobal(Transform target, float xFrom, float xTo, float[] yTimes, float[] yPoints, EaseType yEase, float duration) => 
            Start(TweenSplineY.Create(target, false, xFrom, xTo, yTimes, yPoints, yEase), duration);

        public Tic Alpha(SpriteRenderer target, float to, float duration) =>
            Start(TweenAlphaSpriteRenderer.Create(target, to), duration);

        public Tic Alpha(Image target, float to, float duration) =>
            Start(TweenAlphaImageRenderer.Create(target, to), duration);

        public Tic Alpha(CanvasGroup target, float to, float duration) =>
            Start(TweenAlphaCanvasGroup.Create(target, to), duration);

        public Tic Alpha(TMP_Text target, float to, float duration) =>
            Start(TweenAlphaTmp.Create(target, to), duration);

        public Tic Alpha(SpriteRenderer target, float from, float to, float duration) =>
            Start(TweenAlphaSpriteRenderer.Create(target, to).SetFrom(from), duration);

        public Tic Alpha(TMP_Text target, float from, float to, float duration) =>
            Start(TweenAlphaTmp.Create(target, to).SetFrom(from), duration);

        public Tic Scale(Transform target, Vector2 from, Vector2 to, float duration) =>
            Start(TweenScale.Create(target, to).SetFrom(from), duration);

        public Tic Scale(Transform target, float from, float to, float duration) =>
            Start(TweenScale.Create(target, new Vector2(to, to)).SetFrom(new Vector2(from, from)), duration);

        public Tic Scale(Transform target, Vector2 to, float duration) =>
            Start(TweenScale.Create(target, to), duration);

        public Tic Scale(Transform target, float to, float duration) =>
            Start(TweenScale.Create(target, new Vector2(to, to)), duration);

        public Tic Rotate(Transform target, Vector3 to, float duration) =>
            Start(TweenRotate.Create(target, to), duration);

        public Tic Rotate(Transform target, float to, float duration) =>
            Start(TweenRotate.Create(target, new Vector3(0, 0, to)), duration);

        public Tic Rotate(Transform target, Vector3 from, Vector3 to, float duration) =>
            Start(TweenRotate.Create(target, from, to), duration);

        public Tic Rotate(Transform target, float from, float to, float duration) =>
            Start(TweenRotate.Create(target, new Vector3(0, 0, from), new Vector3(0, 0, to)), duration);
        
        public Tic MoveBezierLocal(Transform target, Vector2 from, Vector2 to, float duration) =>
            Start(TweenBezier.Create(target, true, from, to), duration);

        public Tic MoveBezierLocal(Transform target, Vector2 from, Vector2 by, Vector2 to, float duration) =>
            Start(TweenBezier.Create(target, true, from, by, to), duration);

        public Tic MoveBezierLocal(Transform target, Vector2 from, Vector2 by1, Vector2 by2, Vector2 to, float duration) => 
            Start(TweenBezier.Create(target, true, from, by1, by2, to), duration);

        public Tic MoveBezierGlobal(Transform transform, Vector3 from, Vector3 to, float duration) =>
            Start(TweenBezier.Create(transform, false, from, to), duration);

        public Tic MoveBezierGlobal(Transform transform, Vector3 from, Vector3 by, Vector3 to, float duration) =>
            Start(TweenBezier.Create(transform, false, from, by, to), duration);

        public Tic MoveBezierGlobal(Transform transform, Vector3 from, Vector3 by1, Vector3 by2, Vector3 to, float duration) => 
            Start(TweenBezier.Create(transform, false, from, by1, by2, to), duration);
    }
}
