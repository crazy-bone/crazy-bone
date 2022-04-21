using UnityEngine;
using UnityEditor;

namespace InfinityPBR
{
    public static class EquipObjectMenu {

        [MenuItem("Window/Infinity PBR/Equip Object")] // Provides a menu item
        public static void Equip()
        {
            if (!Selection.activeGameObject)
            {
#if UNITY_EDITOR
                Debug.Log("No Object Selected!");
#endif
                return;
            }
            
            EquipObject.Equip(Selection.activeGameObject);
        }
    }
}
