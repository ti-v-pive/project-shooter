using System;

namespace Tics
{
	public class Tic
	{
		private static readonly TicsList Main = new ();
		private static readonly TicsList Dead = new ();
		
		private static int CurrentFrame = 1; 
		private static int NextFrame = -1;

		internal static int NumCreated;
		internal static int NumCached;
		
		private static readonly bool[] PausedGroups = new bool[Enum.GetValues(typeof(TicGroup)).Length];
		
		public static void PauseGroup(TicGroup group) => PausedGroups[(int) group] = true;
		public static void UnpauseGroup(TicGroup group) => PausedGroups[(int) group] = false;

		public static Tic Create() {
			if (Dead.Empty) {
				return new Tic();
			}

			NumCached--;
			return Dead.Take();
		}
		
		internal static void Update(float deltaTime) {
			if (Main.Empty) {
				return;
			}

			var dt = (int)(1000 * deltaTime);
			if (dt > 50) {
				dt = 50; // for beauty only
			}

			CurrentFrame = NextFrame;
			NextFrame = -CurrentFrame;

			var tic = Main.First;
			while (tic != null) {
				var next = tic.Next;
				
				if (tic.Frame == CurrentFrame) {
					tic.Update(dt);
					
					if (tic.Frame == CurrentFrame) {
						tic.Frame = NextFrame;
					} else if (tic.Frame == 0) {
						MoveToDead(tic);
					}
				} else if (tic.Frame == 0) {
					MoveToDead(tic);
				}
				
				tic = next;
			}
		}

		private static void MoveToDead(Tic tic) {
			Main.Remove(tic);
			Dead.Add(tic);
			NumCached++;
		}
		
		internal Tic Prev;
		internal Tic Next;

		private Action OnComplete;
		private Action OnRepeat;
		private TicMan Parent;
		private string Name;
		private Tween Tween;
		
		private int Time;
		private int Duration;

		private int Repeats;
		private int RepeatsCount;
		private bool Reversed;
		private bool Yoyo;
		private EaseType Ease;
		
		private int Frame;

		public TicGroup Group { get; set; }
		
		private Tic() {
			NumCreated++;
		}

		internal void Stop() {
			OnComplete = null;
			OnRepeat = null;
			Parent = null;
			Name = null;
			
			Tween?.SetName(null);
			Tween?.Kill();
			Tween = null;

			Duration = 0;
			Time = 0;
			
			RepeatsCount = 0;
			Repeats = 0;
			
			Reversed = false;
			Yoyo = false;
			Ease = EaseType.None;

			Frame = 0;

			Group = TicGroup.Default;
		}

		internal Tic Start(TicMan parent, Tween tween, float duration) {
			Parent = parent;
			Tween = tween;
			Duration = (int)(1000 * duration);
			
			Frame = NextFrame;
			Main.Add(this);

			return this;
		}

		public Tic SetDelay(float delay) {
			Time = -(int)(1000 * delay);
			return this;
		}

		public Tic SetEase(EaseType ease) {
			Ease = ease;
			return this;
		}

		public Tic SetRepeat(int repeatCount, RepeatType type = RepeatType.Loop) {
			RepeatsCount = repeatCount;
			Yoyo = type == RepeatType.Yoyo;
			return this;
		}
		
		public Tic SetReversed(bool reversed) {
			Reversed = reversed;
			return this;
		}
		
		public Tic SetName(string name) {
			Name = name;
			Tween?.SetName(name);
			return this;
		}
		
		internal Tic SetOnComplete(Action onComplete) {
			OnComplete = onComplete;
			return this;
		}
		
		internal Tic SetOnRepeat(Action onRepeat) {
			OnRepeat = onRepeat;
			return this;
		}

		private void Update(int dt) {
			if (PausedGroups[(int) Group]) {
				return;
			}
			
			Time += dt;
		
			if (Time < 0) {
				return;
			}

			if (Duration < 0) {
				Tween?.Update(0);
				return;
			}

			if (Time < Duration) {
				Tween?.Update(CalcProgress());
				return;
			}

			Repeats++;

			if (Repeats < RepeatsCount || RepeatsCount < 0) {
				Repeat();
				return;
			}
			
			Tween?.Update(Reversed ? 0.0f : 1.0f);
			Complete();
		}

		private void Complete() {
			var parent = Parent;
			var callback = OnComplete;
			
			Stop();
			
			callback?.Invoke();
			parent?.OnChildComplete(this);
		}

		private void Repeat() {
			Time -= Duration;

			if (Yoyo) {
				Reversed = !Reversed;
			}
			
			Tween?.Update(CalcProgress());
			OnRepeat?.Invoke();
		}

		private float CalcProgress() {
			var progress = (float)Time / Duration;
			return EaseFunctions.Calculate(Ease, Reversed ? 1.0f - progress : progress);
		}

		public float CalcTimeLeft() {
			return Duration - Time;
		}
	}
}