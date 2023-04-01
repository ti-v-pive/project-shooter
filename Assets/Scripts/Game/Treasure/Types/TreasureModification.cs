using UnityEngine;

namespace Game {
    public class TreasureModification : Treasure {
        [SerializeField] private ModificationType _type;
        [SerializeField] private float _duration;
        
        public override void Accept() {
            Main.Instance.Modifications.StartModification(_type, _duration);
        }
    }
}
