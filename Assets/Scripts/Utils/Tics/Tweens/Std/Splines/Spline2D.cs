using UnityEngine;

namespace Tics {
    public class Spline2D {
        private float[] Times;
        private Vector2[] Points;

        private int Length;
        private float Time0;
        private float Period;

        private float[] TimeSteps;
        private Vector2[] Koeffs;

        public void Clear() {
            Times = null;
            Points = null;
            Length = 0;
            Time0 = 0;
            Period = 0;
            TimeSteps = null;
            Koeffs = null;
        }

        public void Init(float[] times, Vector2[] points) {
            Times = times;
            Points = points;

            Length = points.Length;
            Time0 = times[0];
            Period = times[Length - 1] - times[0];

            InitTimeStepsKoeffs();
            InitKoeffs();
        }

        private void InitTimeStepsKoeffs() {
            TimeSteps = new float[Length - 1];

            for (int i = 0; i < Length - 1; i++) {
                TimeSteps[i] = 1.0f / (Times[i + 1] - Times[i]);
            }
        }

        private void InitKoeffs() {
            Koeffs = new Vector2[Length];

            var distance = Vector2.Distance(Points[Length - 1], Points[0]);
            bool cycled = distance < 0.001f;
            Calculate(cycled);
        }

        private void Calculate(bool cycled) {
            Vector2 g1;
            Vector2 g2;
            Vector2 g3;

            if (cycled) {
                Points[Length - 1] = Points[0];
                g1 = Points[0] - Points[Length - 2];
                g2 = Points[1] - Points[0];
                g3 = g2 - g1;
                Koeffs[0] = g1 + 0.5f * g3;
                Koeffs[Length - 1] = Koeffs[0];
            } else {
                Koeffs[0] = Points[1] - Points[0];
                Koeffs[Length - 1] = Points[Length - 1] - Points[Length - 2];
            }

            for (int i = 1; i < Length - 1; i++) {
                g1 = Points[i] - Points[i - 1];
                g2 = Points[i + 1] - Points[i];
                g3 = g2 - g1;
                Koeffs[i] = g1 + 0.5f * g3;
            }
        }

        public Vector2 CalcPoint(float t) {
            if (t <= 0.0f) {
                return Points[0];
            }

            if (t >= 1.0f) {
                return Points[Length - 1];
            }

            t = Time0 + t * Period;

            int tessSector = Length;
            while (t <= Times[--tessSector]) { }

            float tessLocalTime = (t - Times[tessSector]) * TimeSteps[tessSector];
            return GetLocalPoint(tessSector, tessLocalTime);
        }

        private Vector2 GetLocalPoint(int i, float t) {
            return SplineInterpolation(Points[i], Points[i + 1], Koeffs[i], Koeffs[i + 1], t);
        }

        private Vector2 SplineInterpolation(Vector2 x1, Vector2 x2, Vector2 r1, Vector2 r2, float t) {
            float t2 = t * t;
            float t3 = t2 * t;
            return x1 * (2 * t3 - 3 * t2 + 1)
                   + r1 * (t3 - 2 * t2 + t)
                   + x2 * (-2 * t3 + 3 * t2)
                   + r2 * (t3 - t2);
        }
    }
}
