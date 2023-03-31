using System;
using UnityEngine;

namespace Tics {
	public static class EaseFunctions {
		public static float Calculate(EaseType type, float t) {
			return type switch {
				EaseType.None => t,
				EaseType.Linear => Linear(t),
				EaseType.BackIn => BackIn(t),
				EaseType.BackOut => BackOut(t),
				EaseType.BackInOut => BackInOut(t),
				EaseType.QuadIn => QuadIn(t),
				EaseType.QuadOut => QuadOut(t),
				EaseType.QuadInOut => QuadInOut(t),
				EaseType.QuadOutIn => QuadOutIn(t),
				EaseType.CubeIn => CubeIn(t),
				EaseType.CubeOut => CubeOut(t),
				EaseType.CubeInOut => CubeInOut(t),
				EaseType.QuartIn => QuartIn(t),
				EaseType.QuartOut => QuartOut(t),
				EaseType.QuartInOut => QuartInOut(t),
				EaseType.QuintIn => QuintIn(t),
				EaseType.QuintOut => QuintOut(t),
				EaseType.QuintInOut => QuintInOut(t),
				EaseType.SineIn => SineIn(t),
				EaseType.SineOut => SineOut(t),
				EaseType.SineInOut => SineInOut(t),
				EaseType.ExpoIn => ExpoIn(t),
				EaseType.ExpoOut => ExpoOut(t),
				EaseType.ExpoInOut => ExpoInOut(t),
				EaseType.CircIn => CircIn(t),
				EaseType.CircOut => CircOut(t),
				EaseType.CircInOut => CircInOut(t),
				EaseType.ElasticIn => ElasticIn(t),
				EaseType.ElasticOut => ElasticOut(t),
				EaseType.ElasticInOut => ElasticInOut(t),
				EaseType.BounceIn => BounceIn(t),
				EaseType.BounceOut => BounceOut(t),
				EaseType.BounceInOut => BounceInOut(t),
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}
		
		// Easing constants.
		private const float PI_2 = Mathf.PI * 2f;
		private const float PI = Mathf.PI;
		private const float PI_05 = Mathf.PI / 2f;
		private const float B1 = 1f / 2.75f;
		private const float B2 = 2f / 2.75f;
		private const float B3 = 1.5f / 2.75f;
		private const float B4 = 2.5f / 2.75f;
		private const float B5 = 2.25f / 2.75f;
		private const float B6 = 2.625f / 2.75f;
		
		private static float Linear(float t) {
			return t;
		}

		/**
		 * Easing equation function for a quadratic (t^2) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuadIn(float t) {
			return t * t;
		}
		
		/**
		* Easing equation function for a quadratic (t^2) easing out: decelerating to zero velocity.
		*
		* @param t		Current time 
		* @return		The correct value.
		*/
		private static float QuadOut(float t) {
			return -t * (t - 2);
		}
		
		/**
		 * Easing equation function for a quadratic (t^2) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuadInOut(float t) {
			return t <= 0.5 ? t * t * 2 : 1 - (--t) * t * 2;
		}
		
		/**
		 * Easing equation function for a quadratic (t^2) easing out/in: deceleration until halfway, then acceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuadOutIn(float t) {
			if (t < 0.5) return QuadOut(t * 2) * 0.5f;
			return QuadIn((2f * t) - 1f) * 0.5f + 0.5f;
		}

		/**
		 * Easing equation function for a cubic (t^3) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float CubeIn(float t) {
			return t * t * t;
		}
		
		/**
		 * Easing equation function for a cubic (t^3) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float CubeOut(float t) {
			return 1 + (--t) * t * t;
		}
		
		/**
		 * Easing equation function for a cubic (t^3) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float CubeInOut(float t) {
			return t <= 0.5 ? t * t * t * 4 : 1 + (--t) * t * t * 4;
		}
		
		/**
		 * Easing equation function for a quartic (t^4) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuartIn(float t) {
			return t * t * t * t;
		}
		
		/**
		 * Easing equation function for a quartic (t^4) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuartOut(float t) {
			return 1 - (t-=1) * t * t * t;
		}
		
		/**
		 * Easing equation function for a quartic (t^4) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuartInOut(float t) {
			return t <= 0.5f ? t * t * t * t * 8f : (1f - (t = t * 2f - 2f) * t * t * t) / 2f + 0.5f;
		}
		
		/**
		 * Easing equation function for a quintic (t^5) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuintIn(float t) {
			return t * t * t * t * t;
		}
		
		/**
		 * Easing equation function for a quintic (t^5) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuintOut(float t) {
			return (t = t - 1) * t * t * t * t + 1;
		}
		
		/**
		 * Easing equation function for a quintic (t^5) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float QuintInOut(float t) {
			return ((t *= 2f) < 1f) ? t * t * t * t * t * 0.5f : ((t -= 2f) * t * t * t * t + 2f) * 0.5f;
		}
		
		/**
		 * Easing equation function for a sinusoidal (sin(t)) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float SineIn(float t) {
			return -Mathf.Cos(PI_05 * t) + 1;
		}
		
		/**
		 * Easing equation function for a sinusoidal (sin(t)) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float SineOut(float t) {
			return Mathf.Sin(PI_05 * t);
		}
		
		/**
		 * Easing equation function for a sinusoidal (sin(t)) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float SineInOut(float t) {
			return -Mathf.Cos(PI * t) * 0.5f + 0.5f;
		}
		
		/**
		 * Easing equation function for an exponential (2^t) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float ExpoIn(float t) {
			return Mathf.Pow(2f, 10f * (t - 1f));
		}
		
		/**
		 * Easing equation function for an exponential (2^t) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float ExpoOut(float t) {
			return -Mathf.Pow(2f, -10f * t) + 1f;
		}
		
		/**
		 * Easing equation function for an exponential (2^t) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float ExpoInOut(float t)
		{
			return t < 0.5f ? Mathf.Pow(2f, 10f * (t * 2f - 1f)) * 0.5f : (-Mathf.Pow(2f, -10f * (t * 2f - 1f)) + 2f) * 0.5f;
		}
		
		/**
		 * Easing equation function for a circular (sqrt(1-t^2)) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float CircIn(float t)
		{
			return -(Mathf.Sqrt(1 - t * t) - 1);
		}
		
		/**
		 * Easing equation function for a circular (sqrt(1-t^2)) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float CircOut(float t)
		{
			return Mathf.Sqrt(1 - (t - 1) * (t - 1));
		}
		
		/**
		 * Easing equation function for a circular (sqrt(1-t^2)) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time 
		 * @return		The correct value.
		 */
		private static float CircInOut(float t)
		{
			return t <= 0.5 ? (Mathf.Sqrt(1 - t * t * 4) - 1) * -0.5f : (Mathf.Sqrt(1 - (t * 2 - 2) * (t * 2 - 2)) + 1) * 0.5f;
		}
		
		/**
		 * Easing equation function for an elastic (exponentially decaying sine wave) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time 
		 * @param a		Amplitude.
		 * @param p		Period. (!= 0)
		 * @return		The correct value.
		 */
		private static float ElasticIn (float t, float a = 0f, float p = 0.3f) {	
			if (t == 0) return 0;
			if (t == 1) return 1;
			float s;
			if (a == 0f || (a < 1)) { 
				a = 1; 
				s = p * 0.25f; 
			}else {
				s = p / PI_2 * Mathf.Asin(1 / a);
			}
			return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin( (t - s) * PI_2 / p ));
		}
		
		/**
		 * Easing equation function for an elastic (exponentially decaying sine wave) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time
		 * @param a		Amplitude.
		 * @param p		Period.(!= 0)
		 * @return		The correct value.
		 */
		private static float ElasticOut (float t, float a = 0f, float p = 0.3f) {
			if (t == 0) return 0;
			if (t == 1) return 1;
			float s;
			if (a < 1) {
				a = 1;
				s = p * 0.25f;
			}else {
				s = p / PI_2 * Mathf.Asin(1 / a);
			}
			return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin( (t - s) * PI_2 / p ) + 1);
		}
		
		/**
		 * Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time
		 * @param a		Amplitude.
		 * @param p		Period. (!= 0)
		 * @return		The correct value.
		 */
		private static float ElasticInOut (float t, float a = 0f, float p = 0.45f) {
			if (t == 0) return 0;
			if ((t /= 0.5f) == 2f) return 1;
			float s;
			if (a == 0f || (a < 1)) {
				a = 1;
				s = p * 0.25f;
			}else {
				s = p / PI_2 * Mathf.Asin(1 / a);
			}
			if (t < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin( (t - s) * PI_2 / p ));
			return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin( (t - s) * PI_2 / p ) * 0.5f + 1;
		}
		
		/**
		 * Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time
		 * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
		 * @return		The correct value.
		 */
		private static float BackIn(float t, float s = 1.70158f)
		{
			return t * t * ((s + 1) * t - s);
		}
		
		/**
		 * Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time
		 * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
		 * @return		The correct value.
		 */
		private static float BackOut(float t, float s = 1.70158f)
		{
			t -= 1;
			return t * t * ((s + 1) * t + s) + 1;
		}
		
		/**
		 * Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: acceleration until halfway, then deceleration.
		 *
		 * @param t		Current time
		 * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
		 * @return		The correct value.
		 */
		private static float BackInOut(float t, float s = 1.70158f)
		{
			t *= 2f;
			s *= 1.525f;
			if (t < 1) {
				return 0.5f * (t * t * ((s + 1) * t - s));
			}
			else {
				t -= 2;
				return 0.5f * (t * t * ((s + 1) * t + s) + 2);
			}
		}
		
		/**
		 * Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: accelerating from zero velocity.
		 *
		 * @param t		Current time
		 * @return		The correct value.
		 */
		private static float BounceIn(float t)
		{
			t = 1 - t;
			if (t < B1) return 1 - 7.5625f * t * t;
			if (t < B2) return 1 - (7.5625f * (t - B3) * (t - B3) + 0.75f);
			if (t < B4) return 1 - (7.5625f * (t - B5) * (t - B5) + 0.9375f);
			return 1 - (7.5625f * (t - B6) * (t - B6) + 0.984375f);
		}
		
		/**
		 * Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: decelerating from zero velocity.
		 *
		 * @param t		Current time
		 * @return		The correct value.
		 */
		private static float BounceOut(float t)
		{
			if (t < B1) return 7.5625f * t * t;
			if (t < B2) return 7.5625f * (t - B3) * (t - B3) + 0.75f;
			if (t < B4) return 7.5625f * (t - B5) * (t - B5) + 0.9375f;
			return 7.5625f * (t - B6) * (t - B6) + 0.984375f;
		}
		
		/**
		 * Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: deceleration until halfway, then acceleration.
		 *
		 * @param t		Current time
		 * @return		The correct value.
		 */
		private static float BounceInOut(float t)
		{
			if (t < 0.5f)
			{
				t = 1 - t * 2;
				if (t < B1) return (1 - 7.5625f * t * t) * 0.5f;
				if (t < B2) return (1 - (7.5625f * (t - B3) * (t - B3) + 0.75f)) * 0.5f;
				if (t < B4) return (1 - (7.5625f * (t - B5) * (t - B5) + 0.9375f)) * 0.5f;
				return (1 - (7.5625f * (t - B6) * (t - B6) + 0.984375f)) * 0.5f;
			}
			t = t * 2 - 1;
			if (t < B1) return (7.5625f * t * t) * 0.5f + 0.5f;
			if (t < B2) return (7.5625f * (t - B3) * (t - B3) + 0.75f) * 0.5f + 0.5f;
			if (t < B4) return (7.5625f * (t - B5) * (t - B5) + 0.9375f) * 0.5f + 0.5f;
			return (7.5625f * (t - B6) * (t - B6) + 0.984375f) * 0.5f + 0.5f;
		}
	}
}