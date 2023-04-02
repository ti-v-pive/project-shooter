using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Game.AI {
    public class WaypointManager : MonoBehaviourSingleton<WaypointManager> {
        public int numberOfWaypoints = 50;
        public GameObject waypointPrefab;

        private List<GameObject> _waypoints;

        private void Start() {
            _waypoints = new List<GameObject>();

            for (int i = 0; i < numberOfWaypoints; i++) {
                Vector3 randomPoint = RandomNavMeshPoint.GetRandomPointOnNavMesh();
                var waypoint = Instantiate(waypointPrefab, randomPoint, Quaternion.identity);
                waypoint.name = "Waypoint_" + i;
                _waypoints.Add(waypoint);
            }
        }

        public List<Transform> GetClosestWaypoints(GameObject target, int amountOfPoints) {
            if (_waypoints == null || _waypoints.Count == 0) {
                Debug.LogError("No waypoints found in the scene.");
                return null;
            }

            if (amountOfPoints > 0) {
                return _waypoints.OrderBy(waypoint => Vector3.Distance(target.transform.position, waypoint.transform.position))
                    .Take(amountOfPoints)
                    .Select(w => w.transform)
                    .ToList();
            }
            Debug.LogError("Amount of points must be greater than 0.");
            return null;

        }
    }
}