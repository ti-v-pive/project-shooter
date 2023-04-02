using UnityEngine;

namespace Game.Animation {
    public class AnimatedDottedLine : MonoBehaviour {
        public GameObject startObject;
        public GameObject endObject;
        public Material _material;
        public float animationSpeed = 1f;
        public float dotSpacing = 0.1f;
        public float lineWidth = 0.1f;

        private LineRenderer _lineRenderer;
        private float _offset;

        private void Start() {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.material = _material;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;
            _lineRenderer.numCapVertices = 5;
        }

        private void Update() {
            if (startObject == null || endObject == null) {
                return;
            }
            Vector3 startPoint = startObject.transform.position;
            Vector3 endPoint = endObject.transform.position;
            Vector3 direction = endPoint - startPoint;

            if (Physics.Raycast(startPoint, direction, out var hit, direction.magnitude)) {
                if (hit.collider.gameObject != endObject) {
                    endPoint = hit.point;
                }
            }

            _lineRenderer.SetPosition(0, startPoint);
            _lineRenderer.SetPosition(1, endPoint);

            AnimateDottedLine(startPoint, endPoint);
        }

        private void AnimateDottedLine(Vector3 startPoint, Vector3 endPoint) {
            _offset += Time.deltaTime * animationSpeed;
            if (_offset > dotSpacing) {
                _offset -= dotSpacing;
            }

            _lineRenderer.material.mainTextureOffset = new Vector2(_offset, 0);
            _lineRenderer.material.mainTextureScale = new Vector2((Vector3.Distance(startPoint, endPoint) / dotSpacing), 1);
        }
    }
}