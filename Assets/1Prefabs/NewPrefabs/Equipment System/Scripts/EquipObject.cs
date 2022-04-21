using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace InfinityPBR
{
    public static class EquipObject
    {
        // rootBoneName needs to be the name of the Bone Root for this character! It may be different for each
        // character, depending on how it was set up.
        public static string rootBoneName = "BoneRoot"; // Not used if Transform rootBoneTransform is provided to EquipCharacter()
        
        //private static GameObject _target; // The target to match
        private static SkinnedMeshRenderer _targetRenderer; // Renderer we are targetting
        private static string _subRootBoneName; // Bone name
        private static GameObject _thisBoneRoot; // Current bone root
        
        public static void Equip(GameObject targetGameObject, 
            Transform rootBoneTransform = null, 
            SkinnedMeshRenderer targetSkinnedMeshRenderer = null)
        {
            TryDebugMessage("Starting Equip Workflow.");
            
            if (targetGameObject == null)
            {
                Debug.LogError($"Error: targetGameObject is null! Aborting.");
                return;
            }

            // If rootBoneTransform has not been provided then search for it.
            if (rootBoneTransform == null)
                rootBoneTransform = GetChildTransform(targetGameObject, rootBoneName); // Find the rootBoneTransform

            // If rootBoneTransform is null, then we must abort.
            if (rootBoneTransform == null)
            {
                Debug.LogError($"Error: A rootBoneTransform was not provided, and we did not find a transform called {rootBoneName} when searching for the root bone transform.");
                return;
            }
            
            // If skinnedMeshRenderer has not been provided then search for it.
            if (targetSkinnedMeshRenderer == null)
                targetSkinnedMeshRenderer = GetFirstSkinnedMeshRenderer(targetGameObject); // Find the skinnedMeshRenderer
            
            // If skinnedMeshRenderer is null, then we must abort.
            if (targetSkinnedMeshRenderer == null)
            {
                Debug.LogError($"Error: No SkinnedMeshRenderer was assigned, and one could not be found.");
                return;
            }
            
            // Get a list of all the equipment we need to equip
            var equipmentDictionary = GetEquipmentList(targetGameObject);
            TryDebugMessage($"There are {equipmentDictionary.Count} objects to equip.");
            
            // Equip each equipment object we found
            foreach (var equipment in equipmentDictionary)
                EquipObjectToParent(equipment.Key, equipment.Value, rootBoneTransform, targetSkinnedMeshRenderer);

            TryDebugMessage("Character Equip Complete!!");
        }

        /// <summary>
        /// This will equip an object to the parent
        /// </summary>
        /// <param name="equipmentGameObject">The game object being equipped</param>
        /// <param name="equipmentObject">The EquipmentObject component attached to the game object being equipped</param>
        /// <param name="rootBoneTransform"></param>
        /// <param name="targetSkinnedMeshRenderer"></param>
        public static void EquipObjectToParent(GameObject equipmentGameObject, EquipmentObject equipmentObject, Transform rootBoneTransform, SkinnedMeshRenderer targetSkinnedMeshRenderer)
        {
            if (equipmentObject == null) return;
            if (equipmentObject.boneRoot == null)
            {
                Debug.Log($"No bone root assigned on object {equipmentGameObject.name}! Make sure the bone is selected on the prefab.");
                return;
            }
            
            // Confirm the two have the same root bone name
            if (equipmentObject.boneRoot.name != rootBoneTransform.name)
            {
                Debug.LogError($"Error: Root bone names do not match. Parent is {rootBoneTransform.name} while equipment is {equipmentObject.boneRoot.name}");
                return;
            }
            
            MakeItNotAPrefab(equipmentGameObject); // If the object is a prefab, make it not a prefab, so we can delete things
            
            AddEquipmentBonesToParent(equipmentObject, rootBoneTransform, targetSkinnedMeshRenderer); // Add any NEW bones to the parent, from the equipment.
            
            MigrateBoneLinks(equipmentObject, targetSkinnedMeshRenderer); // Fit the equipment bones to the parent

            DeleteOldBonesAndRemoveComponent(equipmentObject); // Delete the bone objects from the equipment

            Transform[] childBoneArray = equipmentObject.skinnedMeshRenderer.bones;
            //Debug.Log($"END: childBoneArray has {childBoneArray.Length} bones");
            //Debug.Log($"And the first one is {childBoneArray[0].name}");
        }

        private static void DeleteOldBonesAndRemoveComponent(EquipmentObject equipmentObject)
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(equipmentObject.boneRoot.gameObject);
#else
            GameObject.Destroy(equipmentObject.boneRoot.gameObject);
#endif
            equipmentObject.SelfDestruct(); // Remove the component
        }

        private static void MakeItNotAPrefab(GameObject equipmentGameObject)
        {
#if UNITY_EDITOR
            if (PrefabUtility.IsAnyPrefabInstanceRoot(equipmentGameObject))
            {
                Debug.Log($"Will try to unpack prefab {equipmentGameObject.name}");
                PrefabUtility.UnpackPrefabInstance(equipmentGameObject, 
                    PrefabUnpackMode.Completely, 
                    InteractionMode.AutomatedAction);
            }
#endif
        }

        /// <summary>
        /// This will relink the SkinnedMeshRenderer bons and rootBone to the parent object
        /// </summary>
        /// <param name="equipmentObject"></param>
        /// <param name="targetSkinnedMeshRenderer"></param>
        private static void MigrateBoneLinks(EquipmentObject equipmentObject, SkinnedMeshRenderer targetSkinnedMeshRenderer)
        {
            Transform[] equipmentObjectBoneArray = equipmentObject.skinnedMeshRenderer.bones;
            //Debug.Log($"equipmentObjectBoneArray has {equipmentObjectBoneArray.Length} bones");
            
            var equipmentObjectBoneMap = GetBoneMap(equipmentObject.skinnedMeshRenderer);
            //Debug.Log($"equipmentObjectBoneMap has {equipmentObjectBoneMap.Count} bones");
            
            
            var parentBoneMap = GetBoneMap(targetSkinnedMeshRenderer); // Populate a Dictionary<string, Transform> holding all the current bones in the SkinnedMeshRenderer
            //Debug.Log($"Parentmap has {parentBoneMap.Count} bones -- Target SMR has {targetSkinnedMeshRenderer.bones.Length} bones");

            
            
            for (int i = 0; i < equipmentObjectBoneArray.Length; i++)
            {
                if (!parentBoneMap.ContainsKey(equipmentObjectBoneArray[i].name)) {
                    Debug.LogWarning($"Warning: Could not find a bone in the parent called {equipmentObjectBoneArray[i].name}. This should not happen...");
                    continue; // ...Technically this should not happen...
                }
                
                //Debug.Log($"Will make {equipmentObjectBoneArray[i].name} match {parentBoneMap[equipmentObjectBoneArray[i].name].name}");
                equipmentObjectBoneArray[i] = parentBoneMap[equipmentObjectBoneArray[i].name];
                
                //Debug.Log($"Check 1: {equipmentObjectBoneArray[i].name} should match {parentBoneMap[equipmentObjectBoneArray[i].name].name}");
                //Debug.Log($"Check 2: {equipmentObjectBoneArray[i].gameObject.GetInstanceID()} should match {parentBoneMap[equipmentObjectBoneArray[i].name].gameObject.GetInstanceID()}");
            }

            equipmentObject.skinnedMeshRenderer.bones = equipmentObjectBoneArray; // Set new values to the bones
            
            //Debug.Log($"Rootbone: {equipmentObject.skinnedMeshRenderer.rootBone} will now be {parentBoneMap[equipmentObject.skinnedMeshRenderer.rootBone.name].name}");
            equipmentObject.skinnedMeshRenderer.rootBone =
                parentBoneMap[equipmentObject.skinnedMeshRenderer.rootBone.name];
            
        }

        /// <summary>
        /// This will add any missing bones from the EquipmentObject.rootBone to the parent
        /// </summary>
        /// <param name="equipmentObject"></param>
        /// <param name="rootBoneTransform"></param>
        /// <param name="targetSkinnedMeshRenderer"></param>
        private static void AddEquipmentBonesToParent(EquipmentObject equipmentObject, Transform rootBoneTransform,
            SkinnedMeshRenderer targetSkinnedMeshRenderer)
        {
            var parentBones = rootBoneTransform.GetComponentsInChildren<Transform>(); // Get the list of bones
            foreach (Transform child in equipmentObject.boneRoot.GetComponentsInChildren<Transform>())
            {
                if (BonesContain(child.name, parentBones)) continue; // If we have the bone already, we skip
                //Debug.Log($"Bone {child.name} was not found...");
                // Cache the name of the parent bone from the child bone list and then get that bone from the parent as well.
                var parentBoneName = child.transform.parent.name;
                var parentBoneTransform = GetBone(parentBoneName, parentBones);

                // Create the new bone on the parent, and set the transform values to match the child bone
                var newBone = new GameObject(child.name);
                newBone.transform.parent = parentBoneTransform;
                newBone.transform.localPosition = child.localPosition;
                newBone.transform.localRotation = child.localRotation;
                newBone.transform.localScale = child.localScale;
                parentBones = rootBoneTransform.GetComponentsInChildren<Transform>(); // Recompute the list of bones

                var bones = new List<Transform>();
                foreach (Transform bone in targetSkinnedMeshRenderer.bones)
                    bones.Add(bone);
                
                bones.Add(newBone.transform);
                targetSkinnedMeshRenderer.bones = bones.ToArray();
                
                
                TryDebugMessage($"Added a bone to the parent called {newBone.name}!");
            }
        }

        private static bool BonesContain(string childName, Transform[] parentBones) => GetBone(childName, parentBones) != null;

        private static Transform GetBone(string boneName, Transform[] bones) =>
            bones.FirstOrDefault(x => x.name == boneName);

        /// <summary>
        /// Searches the children of the target object for objects which have a EquipmentObject component on them, and adds
        /// those and the component to a Dictionary, which is returned.
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        private static Dictionary<GameObject, EquipmentObject> GetEquipmentList(GameObject targetGameObject, bool includeInactive = true)
        {
            var objectList = new Dictionary<GameObject, EquipmentObject>();
            foreach (Transform child in targetGameObject.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (!child.TryGetComponent(out EquipmentObject equipmentObject)) continue;
                objectList.Add(equipmentObject.gameObject, equipmentObject);
            }

            return objectList;
        }
        
        /// <summary>
        /// This will search for the first SkinnedMeshRenderer on any child object of the parent.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static SkinnedMeshRenderer GetFirstSkinnedMeshRenderer(GameObject target)
        {
            foreach (Transform child in target.transform)
            {
                if (!child.TryGetComponent(out SkinnedMeshRenderer smr)) continue;
                //Debug.Log($"Got SMR: {smr.name}");
                return smr;
            }

            return default;
        }

        /// <summary>
        /// This will return a Dictionary with all the names and transforms of the bone hierarchy
        /// </summary>
        /// <param name="rootBoneTransform"></param>
        /// <param name="skinnedMeshRenderer"></param>
        /// <returns></returns>
        private static Dictionary<string, Transform> GetBoneMap(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            var newBoneMap = new Dictionary<string, Transform>();

            // Add each bone from the SkinnedMeshRenderer to the dictionary.
            var boneTransforms = skinnedMeshRenderer.rootBone.GetComponentsInChildren<Transform>();
            foreach (Transform bone in boneTransforms)
            {
                newBoneMap.Add(bone.name, bone);
            }
            
            return newBoneMap;
            
            // March 19, 2022 -- Bug was that the code below was only finding the bones that are actually targetted
            // by the object I guess. Not ALL the bones. So the code above gets them all.
            
            // Add each bone from the SkinnedMeshRenderer to the dictionary.
            foreach (Transform bone in skinnedMeshRenderer.bones)
            {
                newBoneMap.Add(bone.name, bone);
            }

            return newBoneMap;
        }

        /// <summary>
        /// This will search the children of the target object for a transform named string childName, and return it.
        /// </summary>
        /// <param name="target">The GameObject to search</param>
        /// <param name="childName">The name of the object we are looking for</param>
        /// <param name="searchAll">If true, will search all children and grandchildren. Otherwise will search just one level down.</param>
        /// <param name="includeInactive">If searching all, set whether to include inactive objects</param>
        /// <returns></returns>
        public static Transform GetChildTransform(GameObject target, string childName, bool searchAll = true, bool includeInactive = false)
        {
            // Start by looking one level down, returning if we find it.
            foreach (Transform childTransform in target.transform)
            {
                if (childTransform.name != childName) continue;
                //Debug.Log($"Got root bone: {childTransform.name}");
                return childTransform;
            }

            //Debug.Log("Return default");
            if (!searchAll) return default; // If we aren't searching all, end the search here.
            
            // Handle if searchAll = true -- this will include ALL children and grandchildren    
            foreach (Transform childTransform in target.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (childTransform.name != childName) continue;
                return childTransform;
            }

            return default;
        }
        
        private static void TryDebugMessage(string message)
        {
            #if UNITY_EDITOR
            Debug.Log($"{message} <color=#565656>(This message only shows in the Editor)</color>");
            #endif
        }
    }
}
