using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.AI {
    public class RandomNavMeshPoint : MonoBehaviour {
        private static NavMeshTriangulation _nav;
        private static Mesh _mesh;
        private static float _totalArea;
        
        private void Awake() {
            Init();
        }

        private void Init() {
            _nav = NavMesh.CalculateTriangulation();
            _mesh = new Mesh {
                vertices = _nav.vertices,
                triangles = _nav.indices
            };

            _totalArea = 0.0f;
            for (int i = 0; i < _mesh.triangles.Length / 3; i++) {
                _totalArea += GetTriangleArea(i);
            }
        }
        
        public static Vector3 GetRandomPointOnNavMesh() {
            int triangle = GetRandomTriangleOnNavMesh();
            return GetRandomPointOnTriangle(triangle);
        }
        
        public static Vector3 GetConnectedPointOnNavMesh(Vector3 startingPoint) {
            int triangle = GetRandomConnectedTriangleOnNavMesh(startingPoint);
            return GetRandomPointOnTriangle(triangle);
        }
        
        private static int GetRandomTriangleOnNavMesh() {
            float rnd = Random.Range(0, _totalArea);
            int nTriangles = _mesh.triangles.Length / 3;
            for (int i = 0; i < nTriangles; i++) {
                rnd -= GetTriangleArea(i);
                if (rnd <= 0)
                    return i;
            }
            return 0;
        }
        
        private static int GetRandomConnectedTriangleOnNavMesh(Vector3 p) {
            int nTriangles = _mesh.triangles.Length / 3;
            float tArea = 0.0f;
            List<int> connectedTriangles = new List<int>();
            NavMeshPath path = new NavMeshPath();
            for (int i = 0; i < nTriangles; i++) {
                path.ClearCorners();
                if (!NavMesh.CalculatePath(p, _mesh.vertices[_mesh.triangles[3 * i + 0]], NavMesh.AllAreas, path)) {
                    continue;
                }
                if (path.status != NavMeshPathStatus.PathComplete) {
                    continue;
                }
                tArea += GetTriangleArea(i);
                connectedTriangles.Add(i);
            }

            float rnd = Random.Range(0, tArea);

            foreach (int i in connectedTriangles) {
                rnd -= GetTriangleArea(i);
                if (rnd <= 0)
                    return i;
            }
            return 0;
        }
        
        private static Vector3 GetRandomPointOnTriangle(int idx) {
            Vector3[] v = new Vector3[3];


            v[0] = _mesh.vertices[_mesh.triangles[3 * idx + 0]];
            v[1] = _mesh.vertices[_mesh.triangles[3 * idx + 1]];
            v[2] = _mesh.vertices[_mesh.triangles[3 * idx + 2]];

            Vector3 a = v[1] - v[0];
            Vector3 b = v[2] - v[1];
            Vector3 c = v[2] - v[0];

            Vector3 result = v[0] + Random.Range(0f, 1f) * a + Random.Range(0f, 1f) * b;

            float alpha = ((v[1].z - v[2].z) * (result.x - v[2].x) + (v[2].x - v[1].x) * (result.z - v[2].z)) /
                ((v[1].z - v[2].z) * (v[0].x - v[2].x) + (v[2].x - v[1].x) * (v[0].z - v[2].z));
            float beta = ((v[2].z - v[0].z) * (result.x - v[2].x) + (v[0].x - v[2].x) * (result.z - v[2].z)) /
                ((v[1].z - v[2].z) * (v[0].x - v[2].x) + (v[2].x - v[1].x) * (v[0].z - v[2].z));
            float gamma = 1.0f - alpha - beta;

            if (!(alpha < 0) && !(beta < 0) && !(gamma < 0)) {
                return result;
            }
            Vector3 center = v[0] + c / 2;
            center -= result;
            result += 2 * center;

            return result;
        }
        
        private static float GetTriangleArea(int idx) {
            Vector3[] v = new Vector3[3];


            v[0] = _mesh.vertices[_mesh.triangles[3 * idx + 0]];
            v[1] = _mesh.vertices[_mesh.triangles[3 * idx + 1]];
            v[2] = _mesh.vertices[_mesh.triangles[3 * idx + 2]];

            Vector3 a = v[1] - v[0];
            Vector3 b = v[2] - v[1];
            Vector3 c = v[2] - v[0];

            float ma = a.magnitude;
            float mb = b.magnitude;
            float mc = c.magnitude;

            float s = (ma + mb + mc) / 2;
            var area = Mathf.Sqrt(s * (s - ma) * (s - mb) * (s - mc));

            return area;
        }
    }
}