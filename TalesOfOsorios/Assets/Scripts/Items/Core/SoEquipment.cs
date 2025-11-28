using UnityEngine;

namespace Items.Core
{
    [CreateAssetMenu(fileName = "SoEquipment", menuName = "Scriptable Objects/Items/SoEquipment")]
    public class SoEquipment : SoItem
    {
        [SerializeField] private float armor;

        public float Armor => armor;
    }
}
