using UniRx;
using UnityEditor.AI;
using UnityEngine;

namespace Game.AI {
    public class NaveMeshRebuildSignal {}
    
    public class NavMeshUpdater : MonoBehaviour {
        private void Awake() {
            MessageBroker.Default.Receive<StaticObjectDestroy>().Subscribe(_ => OnStaticObjectDestroy());
        }
        
        private static void OnStaticObjectDestroy() {
            NavMeshBuilder.ClearAllNavMeshes();
            NavMeshBuilder.BuildNavMesh();
            
            MessageBroker.Default.Publish(new NaveMeshRebuildSignal());
            Debug.Log("NavMesh rebuild");
        }
    }
}