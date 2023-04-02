using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.AI {
    public class AIController : MonoBehaviour {
        private enum AIState {
            Patrolling,
            Chasing,
            Shooting
        }
        
        public float chaseDistance = 15f;
        public float shootDistance = 10f;
        public float fieldOfViewAngle = 110f;
        public float delayBeforeReturnToPatrolling = 2f;
        public float nextWaypointRadius = 30f;
        
        [Header("Weapon")]
        [SerializeField] private Transform _ignore;
        [SerializeField] private Weapon _weapon;

        private NavMeshAgent _navMeshAgent;
        private Vector3 _currentWaypoint;
        private Vector3 _lastSeenPlayerPosition;
        private AIState _currentState;
        private Transform _transform;
        private float _returnToPatrollingDelay;

        private static Transform PlayerTransform => Player.Instance.ModelTransform;
        private float DistanceToPlayer => Vector3.Distance(_transform.position, PlayerTransform.position);

        private void Awake() {
            if (_weapon) {
                _weapon.SetOwner(CreatureType.Bot);
                _weapon.Ignore(_ignore);
            }
        }

        private void Start() {
            _transform = transform;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            SetState(AIState.Patrolling);
        }

        private void Update() {
            switch (_currentState) {
                case AIState.Patrolling:
                    Patrol();
                    break;
                case AIState.Chasing:
                    Chase();
                    break;
                case AIState.Shooting:
                    Shoot();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GoToNextPoint() {
            var point = GetRandomPointAroundObject();
            _currentWaypoint = point;
            _navMeshAgent.SetDestination(point);
        }

        private void Patrol() {
            if (CanSeePlayer()) {
                SetState(AIState.Chasing);
                return;
            }
            if (Vector3.Distance(_transform.position, _currentWaypoint) < 1f) {
                GoToNextPoint();
            }
        }

        private void Chase() {
            if (!CanSeePlayer()) {
                if (Vector3.Distance(_transform.position, _lastSeenPlayerPosition) > 1f) {
                    _navMeshAgent.SetDestination(_lastSeenPlayerPosition);
                } else {
                    if (_returnToPatrollingDelay > 0) {
                        _returnToPatrollingDelay -= Time.deltaTime;
                        return;
                    }
                    SetState(AIState.Patrolling);
                }
                return;
            }
            
            _returnToPatrollingDelay = delayBeforeReturnToPatrolling;
            
            if (DistanceToPlayer > shootDistance) {
                _navMeshAgent.SetDestination(_lastSeenPlayerPosition);
            } else {
                SetState(AIState.Shooting);
            }
        }

        private void Shoot() {
            if (DistanceToPlayer > shootDistance || !CanSeePlayer()) {
                SetState(AIState.Chasing);
                return;
            }

            Vector3 direction = (PlayerTransform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _navMeshAgent.angularSpeed);
            
            if (IsLookAtPlayer(targetRotation)) {
                if (_weapon) {
                    _weapon.TryShoot();
                }
            }
        }

        private void SetState(AIState state) {
            _currentState = state;
            switch (state) {
                case AIState.Patrolling:
                    GoToNextPoint();
                    break;
                case AIState.Chasing:
                    break;
                case AIState.Shooting:
                    _navMeshAgent.ResetPath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private bool CanSeePlayer() {
            if (DistanceToPlayer > chaseDistance) {
                return false;
            }
            Vector3 direction = PlayerTransform.position - _transform.position;
            float angle = Vector3.Angle(direction, _transform.forward);

            if (angle > fieldOfViewAngle * 0.5f) {
                return false;
            }
            var result = Physics.Raycast(_transform.position, direction.normalized, out var hit, chaseDistance) 
                && hit.transform == PlayerTransform;
            if (result) {
                _lastSeenPlayerPosition = PlayerTransform.position;
            }
            return result;
        }
        
        private bool IsLookAtPlayer(Quaternion targetRotation) {
            return Mathf.Abs(Quaternion.Angle(transform.rotation, targetRotation)) < 0.1f;
        }
        
        private Vector3 GetRandomPointAroundObject() {
            var randomDirection = Random.insideUnitSphere * nextWaypointRadius;
            randomDirection += _transform.position;
            return NavMesh.SamplePosition(randomDirection, out var navMeshHit, nextWaypointRadius, NavMesh.AllAreas) 
                ? navMeshHit.position 
                : Vector3.zero;
        }
    }
}
