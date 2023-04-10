using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
    public class BoxColliderGreenGizmos : UnityEditor.Editor {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawGizmosForBoxCollider(BoxCollider boxCollider, GizmoType gizmoType) {
            Gizmos.color = Color.green.WithAlpha(0.2f);
            Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}