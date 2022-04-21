using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * EquipmentObject
 *
 * Attach this object to any equipment -- wardrobe etc -- that will be equipped onto another object or character,
 * either at run time or edit time.
 *
 * Provided by Infinity PBR - www.InfinityPBR.com
 */

namespace InfinityPBR
{
    public class EquipmentObject : MonoBehaviour
    {
        public SkinnedMeshRenderer skinnedMeshRenderer; // SkinnedMeshRenderer for this object
        public Transform boneRoot; // BoneRoot -- Must match the parent this object is equipped on!!!

        public void SelfDestruct()
        {
#if UNITY_EDITOR
            DestroyImmediate(this);
#else
            Destroy(this);
#endif
        }

        public void Start()
        {
            Debug.LogWarning($"Uh oh! The EquipmentObject component implies that {gameObject.name} is meant to " +
                             $"be equipped onto a character. That process should have deleted this script. Did you forget " +
                             $"to run the \"Equip Object\" option in Window/Infinity PBR/Equip Object?");
        }
    }
}

