using System.Collections;
using System.Collections.Generic;
using InfinityPBR;
using UnityEditor;
using UnityEngine;

/*
 * NOTE: This is "v2" of this script, the original one is "SFB_BlendShapesManager". This script replaces that, but you are
 * able to export values from that script and import them into this script, to transition over. Please add both to the
 * game object and view the inspector for this script. Thanks!
 * www.InfinityPBR.com
 */

namespace InfinityPBR
{
    public class BlendShapesManager : MonoBehaviour
    {
        public List<BlendShapeGameObject> blendShapeGameObjects = new List<BlendShapeGameObject>();
        public bool userMatchRespectsLimits = true; // If true, controlled shapes will respect their set min/max limits, and end up a percentage of the parent shape value

        public List<BlendShapePresetFile> rangeFiles = new List<BlendShapePresetFile>(); // Blend Shape range files
        public List<BlendShapePresetFile> presetFiles = new List<BlendShapePresetFile>(); // Blend shape preset files

        public string blendShapePrimaryPrefix = "SFB_BS_"; // This can be changed by the user in the inspector. Defaults to Infinity PBR values that we use!
        public string blendShapeMatchPrefix = "SFB_BSM_"; // This can be changed by the user in the inspector. Defaults to Infinity PBR values that we use!
        public (string, string) plusMinus = ("Plus", "Minus"); // This can be changed by the user in the inspector. Defaults to Infinity PBR values that we use!
        [HideInInspector] public List<MatchList> matchList = new List<MatchList>();
        [HideInInspector] public string[] matchListNames;
        [HideInInspector] public int matchListIndex = 0;
        
        [HideInInspector] public string exportPath = "";
        [HideInInspector] public bool showHelpBoxes = true;
        [HideInInspector] public bool showSetup = true;
        [HideInInspector] public bool showPresets = true;
        [HideInInspector] public bool showFullInspector = false;
        [HideInInspector] public bool showBlendShapeObjects = true;
        [HideInInspector] public bool showRangeFiles = true;
        [HideInInspector] public bool showPresetFiles = true;
        
        public void SetRandomShapeValue(BlendShapeValue value)
        {
            value.value = Random.Range(value.limitMin, value.limitMax);
            value.lastValue = value.value;
        }

        public void TriggerShape(BlendShapeGameObject obj, BlendShapeValue value, bool triggerAutoMatches = true, bool triggerUserMatches = true)
        {
            if (!obj.gameObject) // If there is no gameObject (i.e. it may have been destroyed), return
                return;
            if (!obj.smr) // If the object doesn't have a skinned mesh renderer, return
                return;

            var targetShapeIndex = obj.smr.sharedMesh.GetBlendShapeIndex(value.fullName);
            if (targetShapeIndex == -1) return;
            obj.smr.SetBlendShapeWeight(targetShapeIndex, value.value);
            /*
             * DID YOU GET AN ERROR ON THE FOLLOWING LINE??
             *
             * Check to make sure you don't have another object of the same name already attached to your prefab. Having
             * two objects of the same name could cause this problem, i.e. if you have some pre-attached objects, and then
             * you try to add another one.
             *
             * You may need to "reload blend shapes" after fixing this.
             *
             * NOTE: March 20, 2022 - I think I fixed this bug by replacing the commented out code below with the one above this
             * comment block. I.e. the index was not always in the same order, so we need to get the
             * index on the shape on the target object by name. If we can't find one, then we return out.
             *
             * This SHOULD work...but leaving this comment here in case it doesn't work as expected, so I can find it later.
             */
            //obj.smr.SetBlendShapeWeight(value.index, value.value);
            if (triggerAutoMatches)
                TriggerAutoMatches(value.triggerName, value.value);
            if (triggerUserMatches)
                TriggerUserMatches(obj, value);
        }

        public void TriggerAutoMatches(string triggerName, float value)
        {
            for (int o = 0; o < blendShapeGameObjects.Count; o++)
            {
                for (int i = 0; i < blendShapeGameObjects[o].blendShapeValues.Count; i++)
                {
                    if (blendShapeGameObjects[o].blendShapeValues[i].triggerName == triggerName)
                    {
                        //Debug.Log("Auto: " + blendShapeGameObjects[o].gameObjectName + " | " + blendShapeGameObjects[o].blendShapeValues[i].triggerName);
                        blendShapeGameObjects[o].blendShapeValues[i].value = blendShapeGameObjects[o].blendShapeValues[i].isMinus ? -value : value;
                        TriggerShape(blendShapeGameObjects[o], blendShapeGameObjects[o].blendShapeValues[i], false, false);
                    }
                }
            }
        }
        
        public void TriggerUserMatches(BlendShapeGameObject obj, BlendShapeValue value)
        {
            foreach (var match in value.otherValuesMatchThis)
            {
                var matchObj = blendShapeGameObjects[match.objectIndex];
                var matchValue = matchObj.blendShapeValues[match.valueIndex];

                float finalValue = userMatchRespectsLimits
                    ? matchValue.limitMin + (AmountBetweenMinAndMax(matchValue) * PercentFromMin(value))
                    : value.value;

                matchValue.value = finalValue;
                matchValue.lastValue = finalValue;

                TriggerShape(matchObj, matchValue,true, false);
            }
        }

        public float PercentFromMin(BlendShapeValue value)
        {
            // example:  -50 to 100, value is -10
            float maxDif = AmountBetweenMinAndMax(value); // Example (100 - -50) = 150
            float posFromMin = value.value - value.limitMin; // Example (-10 - -50) = 40
            return Mathf.Clamp(posFromMin / maxDif, -1f, 1f); // Example ( 40 / 150) = 0.2666 = 26.7%
        }

        public float ValueFromMin(BlendShapeValue value, float percentFromMin)
        {
            return AmountBetweenMinAndMax(value) * percentFromMin;
        }

        public float AmountBetweenMinAndMax(BlendShapeValue value)
        {
            return value.limitMax - value.limitMin;
        }

        public void ResetAll()
        {
            for (int o = 0; o < blendShapeGameObjects.Count; o++)
            {
                BlendShapeGameObject obj = blendShapeGameObjects[o];
                if (obj.displayableValues > 0)
                {
                    for (int i = 0; i < obj.blendShapeValues.Count; i++)
                    {
                        BlendShapeValue value = obj.blendShapeValues[i];
                        if (value.display && !value.matchAnotherValue)
                        {
                            value.value = 0;
                            TriggerShape(obj, value);
                        }
                    }
                }
            }
        }

        /*
         * 
         */
        public void SetAllShapeValues()
        {
            for (int o = 0; o < blendShapeGameObjects.Count; o++)
            {
                BlendShapeGameObject obj = blendShapeGameObjects[o];
                if (obj.displayableValues > 0)
                {
                    for (int i = 0; i < obj.blendShapeValues.Count; i++)
                    {
                        BlendShapeValue value = obj.blendShapeValues[i];
                        if (value.display && !value.matchAnotherValue)
                        {
                            TriggerShape(obj, value);
                        }
                    }
                }
            }
        }

        public void ImportRangeFile(TextAsset textAsset)
        {
            rangeFiles.Add(new BlendShapePresetFile());
            rangeFiles[rangeFiles.Count - 1].textAsset = textAsset;
        }

        public void LoadRangeFile(TextAsset rangeFile)
        {
            Debug.Log("Loading Range File: " + rangeFile.name);
            string contents = rangeFile.text;
            if (ImportRangeFileFromV2(rangeFile))
                return;
            
            if (ImportRangeFileFromV3(rangeFile))
                return;
        }

        public bool ImportRangeFileFromV2(TextAsset textAsset)
        {
            string[] splitText = textAsset.text.Split(new string[] { "," }, System.StringSplitOptions.None);
            if (splitText[0] == "SFB_BlendShapesManager V2")
            {
                for (int i = 1; i < splitText.Length; i++)
                {
                    string fullName = splitText[i + 1];
                    string triggerName = fullName.Replace(plusMinus.Item1, "");
                    float minLimit = float.Parse(splitText[i + 2]);
                    float maxLimit = float.Parse(splitText[i + 3]);
                    
                    var dataAttempt = TryGetShapeData(triggerName);
                    BlendShapeGameObject obj = dataAttempt.Item1;
                    BlendShapeValue value = dataAttempt.Item2;
                    if (obj != null && value != null)
                    {
                        value.limitMin = minLimit;
                        value.limitMax = maxLimit;
                    }

                    i++;
                    i++;
                    i++;
                }

                Debug.Log("Import Finished");
                return true;
            }

            return false;
        }
        
        public bool ImportRangeFileFromV3(TextAsset textAsset)
        {
            string[] splitText = textAsset.text.Split(new string[] { "," }, System.StringSplitOptions.None);
            if (splitText[0] == "InfinityPBR_BlendShapesManager_RangeFile")
            {
                Debug.Log("Range File Version: " + splitText[0]);
                for (int i = 1; i < splitText.Length; i++)
                {
                    string gameObjectName = splitText[i];
                    string fullName = splitText[i + 1];
                    string triggerName = fullName.Replace(plusMinus.Item1, "");
                    triggerName = triggerName.Replace(blendShapePrimaryPrefix, "");
                    float minLimit = float.Parse(splitText[i + 2]);
                    float maxLimit = float.Parse(splitText[i + 3]);
                    
                    var dataAttempt = TryGetShapeData(gameObjectName, triggerName);
                    BlendShapeGameObject obj = dataAttempt.Item1;
                    BlendShapeValue value = dataAttempt.Item2;
                    if (obj != null && value != null)
                    {
                        value.limitMin = minLimit;
                        value.limitMax = maxLimit;
                    }

                    i++;
                    i++;
                    i++;
                }

                return true;
            }

            return false;
        }

        public (BlendShapeGameObject, BlendShapeValue) TryGetShapeData(string objGameObjectName, string triggerName)
        {
            BlendShapeGameObject obj = null;
            BlendShapeValue value = null;

            obj = GetBlendShapeObject(objGameObjectName);

            for (int i = 0; i < obj.blendShapeValues.Count; i++)
            {
                BlendShapeValue thisValue = obj.blendShapeValues[i];
                if (thisValue.display && thisValue.triggerName == triggerName)
                {
#if UNITY_EDITOR
                    Debug.Log("Matched: " + triggerName);
#endif
                    return (obj, thisValue);
                }
            }
            return (obj, value);
        }
        
        public (BlendShapeGameObject, BlendShapeValue) TryGetShapeData(string triggerName)
        {
            BlendShapeGameObject obj = null;
            BlendShapeValue value = null;
            

            for (int o = 0; o < blendShapeGameObjects.Count; o++)
            {
                BlendShapeGameObject thisObj = blendShapeGameObjects[o];
                if (thisObj.displayableValues > 0)
                {
                    for (int i = 0; i < thisObj.blendShapeValues.Count; i++)
                    {
                        BlendShapeValue thisValue = thisObj.blendShapeValues[i];
                        if (thisValue.display && thisValue.triggerName == triggerName)
                        {
#if UNITY_EDITOR
                            Debug.Log("Matched: " + triggerName);
#endif
                            return (thisObj, thisValue);
                        }
                    }
                }
            }

            return (obj, value);
        }
        
        /*
         * This is the same as clicking the button in the Blend Shapes Manager Inspector. It will first attempt to
         * relink any missing game objects, which may have been turned off or deleted, but may be available now.
         *
         * Then it will load new shapes, remove unused objects, and finally set the values.
         */
        public void LoadBlendShapeData()
        {
            Transform[] gameObjects = gameObject.GetComponentsInChildren<Transform>(true);
            AttemptRelinks(gameObjects);
            LoadNewShapes(gameObjects);
            RemoveUnusedObjects(gameObjects);
            SetAllShapeValues();
        }

        public void LoadNewShapes(Transform[] gameObjects)
        {
            foreach (Transform transform in gameObjects)
            {
                GameObject gameObject = transform.gameObject;
                SkinnedMeshRenderer smr = GetSkinnedMeshRenderer(gameObject);
                if (smr == null)
                    continue;

                //Debug.Log("Adding " + gameObject.name + " | " + smr);
                
                blendShapeGameObjects.Add(new BlendShapeGameObject());
                int newObjectIndex = blendShapeGameObjects.Count - 1;
                BlendShapeGameObject newObject = blendShapeGameObjects[newObjectIndex];
                newObject.smr = smr;
                newObject.gameObjectName = gameObject.name;
                newObject.gameObject = gameObject;

                AddShapesToList(smr, newObject, newObjectIndex);
            }
        }

        public void AttemptRelinks(Transform[] gameObjects)
        {
            for (int o = 0; o < blendShapeGameObjects.Count; o++)
            {
                BlendShapeGameObject obj = blendShapeGameObjects[o];
                if (obj.gameObject)
                    continue;

                foreach (Transform transform in gameObjects)
                {
                    if (transform.gameObject.name == obj.gameObjectName)
                    {
                        if (transform.gameObject.GetComponent<SkinnedMeshRenderer>())
                        obj.gameObject = transform.gameObject;
                        obj.smr = transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    }
                }
            }
        }

        private SkinnedMeshRenderer GetSkinnedMeshRenderer(GameObject gameObject)
        {
            if (IsGameObjectInList(gameObject))
                return null;
            if (!gameObject.GetComponent<SkinnedMeshRenderer>())
                return null;
            if (!gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh)
                return null;
            if (gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount == 0)
                return null;

            return gameObject.GetComponent<SkinnedMeshRenderer>();
        }

        private void RemoveUnusedObjects(Transform[] transforms)
        {
            for (int i = blendShapeGameObjects.Count - 1; i >= 0; i--)
            {
                if (blendShapeGameObjects[i].gameObject)    // This will keep an object from being removed if it's gone missing, so it can be relinked later
                {
                    #if UNITY_EDITOR
                    if (!ArrayUtility.Contains(transforms, blendShapeGameObjects[i].gameObject.transform))
                        blendShapeGameObjects.RemoveAt(i);
                    #endif
                }
            }
        }

        private void AddShapesToList(SkinnedMeshRenderer smr, BlendShapeGameObject newObject, int newObjectIndex)
        {
            for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
            {
                newObject.blendShapeValues.Add(new BlendShapeValue());
                BlendShapeValue newEntry = newObject.blendShapeValues[newObject.blendShapeValues.Count - 1];

                string triggerName = GetHumanName(smr.sharedMesh.GetBlendShapeName(i));

                if (triggerName == "")
                    continue;
                    
                if (triggerName.Contains(plusMinus.Item2))
                    newEntry.isMinus = true;

                if (triggerName.Contains(plusMinus.Item2) || triggerName.Contains(plusMinus.Item1))
                {
                    newEntry.min = -100f;
                    newEntry.limitMin = -100f;
                }

                if (smr.sharedMesh.GetBlendShapeName(i).Contains(blendShapePrimaryPrefix) && !newEntry.isMinus)
                {
                    newEntry.display = true;
                    newObject.displayableValues++;
                }

                newEntry.index = i;
                newEntry.fullName = smr.sharedMesh.GetBlendShapeName(i);
                    
                triggerName = triggerName.Replace(blendShapeMatchPrefix, "");
                triggerName = triggerName.Replace(blendShapePrimaryPrefix, "");
                triggerName = triggerName.Replace(plusMinus.Item1, "");
                triggerName = triggerName.Replace(plusMinus.Item2, "");
                    
                newEntry.triggerName = triggerName;
                newEntry.objectIndex = newObjectIndex;
            }
        }

        public bool IsGameObjectInList(GameObject gameObject)
        {
            for (int i = 0; i < blendShapeGameObjects.Count; i++)
            {
                if (blendShapeGameObjects[i].gameObject == gameObject)
                    return true;
            }

            return false;
        }
        
        public string GetHumanName(string blendShapeName)
        {
            if (!blendShapeName.Contains(blendShapePrimaryPrefix) && !blendShapeName.Contains(blendShapeMatchPrefix))
                return "";
            
            string[] periodParse = blendShapeName.Split(new string[] { "." }, System.StringSplitOptions.None);
            string[] stringParse = periodParse [periodParse.Length == 1 ? 0 : 1].Split(new string[] { "_" }, System.StringSplitOptions.None);

            if (stringParse.Length >= 3 && blendShapeName.Contains(blendShapePrimaryPrefix))
                return stringParse [2];
            if (stringParse.Length >= 4 && blendShapeName.Contains(blendShapeMatchPrefix))
                return stringParse [3];

            return "";
        }

        public BlendShapeGameObject GetBlendShapeObject(string name)
        {
            for (int i = 0; i < blendShapeGameObjects.Count; i++)
            {
                if (blendShapeGameObjects[i].gameObjectName == name)
                    return blendShapeGameObjects[i];
            }

            Debug.LogWarning("Warning: Could not find a blend Shape Object named " + name);

            return null;
        }
        
        public BlendShapeValue GetBlendShapeValue(BlendShapeGameObject obj, string triggerName, bool minusValuesOK = false)
        {
            //Debug.Log("Obj: " + obj.displayableValues);
            for (int i = 0; i < obj.blendShapeValues.Count; i++)
            {
                if (obj.blendShapeValues[i].triggerName == triggerName)
                {
                    if (!obj.blendShapeValues[i].isMinus || minusValuesOK)
                        return obj.blendShapeValues[i];
                }
            }

            Debug.LogWarning("Warning: Was not able to get blend shape value for " + triggerName);
            return null;
        }

        public BlendShapeValue GetBlendShapeValue(string objectName, string triggerName, bool minusValuesOK = false)
        {
            BlendShapeGameObject obj = GetBlendShapeObject(objectName);
            if (obj == null)
            {
                Debug.LogWarning("Warning: Unable to get a BlendShapeObject for " + objectName + " (Trigger: " +
                                 triggerName + ")");
                return null;
            }
            return GetBlendShapeValue(obj, triggerName, minusValuesOK);
        }

        public void BuildMatchLists()
        {
            matchList.Clear();
            matchListIndex = 0;

            for (int o = 0; o < blendShapeGameObjects.Count; o++)
            {
                BlendShapeGameObject obj = blendShapeGameObjects[o];
                if (obj.displayableValues == 0)
                    continue;
                if (!obj.gameObject)
                    continue;
            
                for (int i = 0; i < obj.blendShapeValues.Count; i++)
                {
                    BlendShapeValue value = obj.blendShapeValues[i];
                    if (!value.display)
                        continue;
                    if (value.matchAnotherValue)
                        continue;
                    if (value.otherValuesMatchThis.Count > 0)
                        continue;

                    matchList.Add(new MatchList());
                    MatchList newMatchlist = matchList[matchList.Count - 1];
                
                    newMatchlist.name = obj.gameObject.name + " " + value.triggerName;
                    newMatchlist.objectIndex = o;
                    newMatchlist.valueIndex = i;
                }
            }

            matchListNames = new string[matchList.Count];
            for (int i = 0; i < matchList.Count; i++)
            {
                matchListNames[i] = matchList[i].name;
            }
        
        }
    }
    
    [System.Serializable]
    public class BlendShapeGameObject
    {
        public string gameObjectName;
        public GameObject gameObject;
        public SkinnedMeshRenderer smr;
        public int displayableValues = 0;
        public List<BlendShapeValue> blendShapeValues = new List<BlendShapeValue>();
        [HideInInspector] public bool showValues = false;
    }

    [System.Serializable]
    public class BlendShapeValue
    {
        public int objectIndex;
        public int index;
        public bool display = false;
        public string fullName;
        public string triggerName;
        public bool isMinus = false;

        public float value = 0f;
        public float min = 0f;
        public float max = 100f;

        [HideInInspector] public float limitMin = 0f;
        [HideInInspector] public float limitMax = 100f;

        [HideInInspector] public float lastValue = 0f;
        [HideInInspector] public bool showValueOptions = false;
        [HideInInspector] public bool isOpen = false;

        // If this is being controlled by another shape, set these
        public int matchThisObjectIndex = 0;
        public int matchThisValueIndex = 0;
        public bool matchAnotherValue = false;
        
        // Shapes that this value controls will go here.
        public List<MatchValue> otherValuesMatchThis = new List<MatchValue>();
    }

    [System.Serializable]
    public class MatchValue
    {
        public int objectIndex;
        public int valueIndex;
    }

    [System.Serializable]
    public class BlendShapePresetFile
    {
        public string name;
        public TextAsset textAsset;
    }
    
    [System.Serializable]
    public class MatchList
    {
        public string name;
        public int objectIndex;
        public int valueIndex;
    }
}
