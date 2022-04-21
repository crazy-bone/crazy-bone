using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/*
 * This is V2 of this script! It changes things a bit, so if you are actively using this script, you may want to revert
 * or take the time to update your prefabs and code.
 */

namespace InfinityPBR
{
    [RequireComponent(typeof(PrefabAndObjectManager))]
    [RequireComponent(typeof(BlendShapesManager))]
    [System.Serializable]
    public class WardrobePrefabManager : MonoBehaviour
    {
        public bool autoRigWhenActivated = true;
        public bool handleBlendShapes = true;

        [HideInInspector] public bool showHelpBoxes = true;
        [HideInInspector] public bool showFullInspector = false;
        [HideInInspector] public bool showBlendShapeManagement = false;
        [HideInInspector] public bool showSetup = true;
        [HideInInspector] public string[] actionTypes = new string[] {"Explicit", "Less than", "Greater than"};

        public List<BlendShapeGroup> blendShapeGroups = new List<BlendShapeGroup>();
        public List<BlendShapeObject> blendShapeObjects = new List<BlendShapeObject>();
        
        public BlendShapesManager BlendShapesManager => GetBlendShapesManager();
        private BlendShapesManager _blendShapesManager;
    
        public PrefabAndObjectManager PrefabAndObjectManager => GetPrefabAndObjectManager();
        private PrefabAndObjectManager _prefabAndObjectManager;
        
        private BlendShapesManager GetBlendShapesManager()
        {
            if (_blendShapesManager != null) return _blendShapesManager;
            if (TryGetComponent(out BlendShapesManager foundManager))
                _blendShapesManager = foundManager;
            return _blendShapesManager;
        }
    
        private PrefabAndObjectManager GetPrefabAndObjectManager()
        {
            if (_prefabAndObjectManager != null) return _prefabAndObjectManager;
            if (TryGetComponent(out PrefabAndObjectManager foundManager))
                _prefabAndObjectManager = foundManager;
            return _prefabAndObjectManager;
        }

        
        public void OnActivate(string name)
        {
            SetBlendShapes();
            List<BlendShapeItem> blendShapeItems = GetBlendShapeItems("Activate", name);
            if (blendShapeItems != null)
            {
                for (int i = 0; i < blendShapeItems.Count; i++)
                {
                    var revertResults = RevertOnDeactivate(blendShapeItems[i].triggerName, name);
                    bool hasRevert = revertResults.Item1;
                    int revertIndex = revertResults.Item2;
                    if (hasRevert)
                    {
                        BlendShapeItem revertItem = blendShapeGroups[GetGroupIndex(name)].onDeactivate[revertIndex];
                        BlendShapeValue bsvalue = BlendShapesManager.GetBlendShapeValue(revertItem.objectName, revertItem.triggerName);
                        if (bsvalue == null)
                            Debug.LogWarning("Unable to get value");
                        else
                            revertItem.value = bsvalue.value;
                    }

                    float value = blendShapeItems[i].value;
                    float currentValue = BlendShapesManager.GetBlendShapeValue(blendShapeItems[i].objectName, blendShapeItems[i].triggerName).value;
                    if (blendShapeItems[i].actionType == "Less than" && currentValue <= value)
                        value = currentValue;
                    if (blendShapeItems[i].actionType == "Greater than" && currentValue >= value)
                        value = currentValue;
                
                    TriggerBlendShape(blendShapeItems[i].triggerName, value);
                }
            }
            
        }

        public void OnDeactivate(string name)
        {
            SetBlendShapes();
            List<BlendShapeItem> blendShapeItems = GetBlendShapeItems("Deactivate", name);
            if (blendShapeItems != null)
            {
                for (int i = 0; i < blendShapeItems.Count; i++)
                {
                    TriggerBlendShape(blendShapeItems[i].triggerName, blendShapeItems[i].value);
                }
            }
            
        }

        private (bool, int) RevertOnDeactivate(string triggerName, string name)
        {
            bool hasRevert = false;
            int revertIndex = 0;
            List<BlendShapeItem> blendShapeItems = GetBlendShapeItems("Deactivate", name);
            for (int i = 0; i < blendShapeItems.Count; i++)
            {
                if (blendShapeItems[i].triggerName == triggerName)
                {
                    if (blendShapeItems[i].revertBack)
                    {
                        hasRevert = true;
                        revertIndex = i;
                    }
                }
            }

            return (hasRevert, revertIndex);
        }

        private bool GroupIsInList(PrefabGroup group) => blendShapeGroups.FirstOrDefault(x => x.prefabGroup.uid == group.uid) != null;

        private bool GroupIsInManager(PrefabGroup prefabGroup) => PrefabAndObjectManager.prefabGroups.FirstOrDefault(x => x.uid == prefabGroup.uid) != null;

        private BlendShapeGroup BlendShapeGroupByName(string name) =>
            blendShapeGroups.FirstOrDefault(x => x.prefabGroup.name == name);

        private List<BlendShapeItem> GetBlendShapeItems(string eventType, string blendShapeGroupName)
        {
            var shapeGroup = BlendShapeGroupByName(blendShapeGroupName);

            if (shapeGroup == null)
                return null;
            
            if (eventType == "Activate")
                return shapeGroup.onActivate;
            if (eventType == "Deactivate")
                return shapeGroup.onDeactivate;

            return null; // This should not happen
        }

        /*
         * This will update the group list, pulling in the data from the Prefab And Object Manager component.
         * It will not remove groups that already exist.
         */
        public void UpdateGroupList()
        {
            // Add groups we don't have yet
            foreach (var group in PrefabAndObjectManager.prefabGroups)
            {
                if (GroupIsInList(group)) continue;
                
                var newGroup = new BlendShapeGroup
                {
                    prefabGroup = group
                };
                blendShapeGroups.Add(newGroup); ;
            }
            
            // Remove orphan groups
            for (int i = blendShapeGroups.Count - 1; i >= 0; i--)
            {
                if (GroupIsInManager(blendShapeGroups[i].prefabGroup)) continue;
                blendShapeGroups.RemoveAt(i);
            }

            PopulateGroupBlendShapeNames();
        }

        public void PopulateGroupBlendShapeNames()
        {
            for (int g = blendShapeGroups.Count - 1; g >= 0; g--)
            {
                blendShapeGroups[g].actualBlendShapeNames.Clear();
                blendShapeGroups[g].blendShapeNames.Clear();
                blendShapeGroups[g].blendShapeObjectName.Clear();

                if (string.IsNullOrEmpty(blendShapeGroups[g].prefabGroup.name))
                {
                    Debug.LogWarning("Warning: The name of this group is empty. If you recently updated the " +
                                     "script, you may want to try resetting the \"Wardrobe Prefab Manager\", by clicking the " +
                                     "three vertical dots to the right of it in the Inspector. Will now return as an error would " +
                                     "be presented.");
                    return;
                }
                
                foreach (var groupObject in PrefabAndObjectManager.prefabGroups[GetGroupIndex(blendShapeGroups[g].prefabGroup.name)].groupObjects)
                {
                    SkinnedMeshRenderer[] renderers = groupObject.objectToHandle.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    foreach (SkinnedMeshRenderer smr in renderers)
                    {
                        for (int b = 0; b < smr.sharedMesh.blendShapeCount; b++)
                        {
                            string newName = smr.sharedMesh.GetBlendShapeName(b);

                            if (!newName.Contains(BlendShapesManager.blendShapePrimaryPrefix))
                                continue;

                            if (newName.Contains(BlendShapesManager.plusMinus.Item2))
                                continue;
                            
                            blendShapeGroups[g].actualBlendShapeNames.Add(newName);
                            newName = newName.Replace(BlendShapesManager.plusMinus.Item1, "");
                            newName = newName.Replace(BlendShapesManager.blendShapePrimaryPrefix, "");
                            blendShapeGroups[g].blendShapeNames.Add(newName);
                            blendShapeGroups[g].blendShapeObjectName.Add(smr.gameObject.name);
                        }
                    }
                }
            }
        }

        public int GetGroupIndex(string groupName)
        {
            for (int g = 0; g < PrefabAndObjectManager.prefabGroups.Count; g++)
            {
                if (PrefabAndObjectManager.prefabGroups[g].name != groupName) continue;
                
                return g;
            }

            Debug.LogWarning($"No group index was found for {groupName}");
            return -1;
        }

        public void SetBlendShapes()
        {
            blendShapeObjects.Clear();
            
            Transform[] gameObjects = gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform transform in gameObjects)
            {
                GameObject gameObject = transform.gameObject;
                if (!gameObject.GetComponent<SkinnedMeshRenderer>())
                    continue;

                SkinnedMeshRenderer smr = gameObject.GetComponent<SkinnedMeshRenderer>();

                if (!smr)
                    continue;
                if (!smr.sharedMesh)
                    continue;
                if (smr.sharedMesh.blendShapeCount == 0)
                    continue;

                blendShapeObjects.Add(new BlendShapeObject());
                BlendShapeObject newObject = blendShapeObjects[blendShapeObjects.Count - 1];

                newObject.gameObjectWithShapes = gameObject;
                for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
                {
                    newObject.blendShapeEntries.Add(new BlendShapeEntry());
                    BlendShapeEntry newEntry = newObject.blendShapeEntries[newObject.blendShapeEntries.Count - 1];
                    newEntry.index = i;

                    string triggerName = BlendShapesManager.GetHumanName(smr.sharedMesh.GetBlendShapeName(i));

                    if (triggerName.Contains(BlendShapesManager.plusMinus.Item2))
                        newEntry.isMinus = true;

                    newEntry.triggerName = triggerName;
                }
            }
        }

        public void TriggerBlendShape(string triggerName, float shapeValue)
        {
            BlendShapesManager.TriggerAutoMatches(triggerName, shapeValue);
        }

        public BlendShapeGroup GetGroup(PrefabGroup group) => blendShapeGroups.FirstOrDefault(x => x.prefabGroup.uid == group.uid);

        public void AddToList(string eventType, MatchList matchList, BlendShapeGroup group)
        {
            BlendShapeItem newItem = new BlendShapeItem();
            BlendShapeGameObject obj = BlendShapesManager.blendShapeGameObjects[matchList.objectIndex];
            BlendShapeValue value = obj.blendShapeValues[matchList.valueIndex];
            string triggerName = value.triggerName;
        
            if (eventType == "Activate")
            {
                // This commented-out loop was intended to ensure only one entry per item, but we now wnat to allow 
                // multiple due to the addition of the "actionType", so you can do less than & greater than. (Between)
                for (int i = 0; i < group.onActivate.Count; i++)
                {
                    //if (group.onActivate[i].triggerName == triggerName)
                    //    return;
                }
                group.onActivate.Add(new BlendShapeItem());
                newItem = group.onActivate[group.onActivate.Count - 1];
            }
            if (eventType == "Deactivate" || eventType == "Revert Back")
            {
                for (int i = 0; i < group.onDeactivate.Count; i++)
                {
                    if (group.onDeactivate[i].triggerName == triggerName)
                        return;
                }
                group.onDeactivate.Add(new BlendShapeItem());
                newItem = group.onDeactivate[group.onDeactivate.Count - 1];
                if (eventType == "Revert Back")
                    newItem.revertBack = true;
            }

            newItem.min = value.min;
            newItem.triggerName = triggerName;
            newItem.objectName = obj.gameObjectName;
            

        }
        
        public void AddToList(string eventType, BlendShapeGroup group)
        {
            BlendShapeItem newItem = new BlendShapeItem();
            if (group.actualBlendShapeNames[group.shapeChoiceIndex].Contains(BlendShapesManager.plusMinus.Item1))
                newItem.min = -100f;
            string triggerName = BlendShapesManager.GetHumanName(group.actualBlendShapeNames[group.shapeChoiceIndex]);
            string objectName = "";
        
            if (eventType == "Activate")
            {
                for (int i = 0; i < group.onActivate.Count; i++)
                {
                    if (group.onActivate[i].triggerName == triggerName)
                        return;
                }
                group.onActivate.Add(new BlendShapeItem());
                newItem = group.onActivate[group.onActivate.Count - 1];
                objectName = group.blendShapeObjectName[group.shapeChoiceIndex];
            }
            else if (eventType == "Deactivate" || eventType == "Revert Back")
            {
                for (int i = 0; i < group.onDeactivate.Count; i++)
                {
                    if (group.onDeactivate[i].triggerName == group.actualBlendShapeNames[group.shapeChoiceIndex])
                        return;
                }
                group.onDeactivate.Add(new BlendShapeItem());
                newItem = group.onDeactivate[group.onDeactivate.Count - 1];
                objectName = group.blendShapeObjectName[group.shapeChoiceIndex];
                if (eventType == "Revert Back")
                    newItem.revertBack = true;
            }

            newItem.triggerName = triggerName;
            newItem.objectName = objectName;

            
        }
    }

    [System.Serializable]
    public class BlendShapeGroup
    {
        //public string name;
        //public string type;
        public PrefabGroup prefabGroup;
        public List<BlendShapeItem> onActivate = new List<BlendShapeItem>();
        public List<BlendShapeItem> onDeactivate = new List<BlendShapeItem>();
        [HideInInspector] public bool showList = false;
        [HideInInspector] public List<string> blendShapeNames = new List<string>();
        [HideInInspector] public List<string> actualBlendShapeNames = new List<string>();
        [HideInInspector] public List<string> blendShapeObjectName = new List<string>();
        [HideInInspector] public int shapeChoiceIndex = 0;
        //[HideInInspector] public int deactivateChoiceIndex = 0;
    }

    [System.Serializable]
    public class BlendShapeItem
    {
        public string triggerName;
        public string objectName;
        public float value;
        public float min = 0f;
        public float max = 100f;
        public bool revertBack = false;
        public string actionType = "Explicit";
        public int actionTypeIndex = 0;
    }

    [System.Serializable]
    public class BlendShapeObject
    {
        public GameObject gameObjectWithShapes;
        public List<BlendShapeEntry> blendShapeEntries = new List<BlendShapeEntry>();
    }

    [System.Serializable]
    public class BlendShapeEntry
    {
        public int index;
        public string triggerName;
        public bool isMinus = false;
    }
}
