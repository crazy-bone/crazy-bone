using System.Collections.Generic;
using UnityEngine;

namespace InfinityPBR
{
    [RequireComponent(typeof(BlendShapesManager))]
    [System.Serializable]
    public class BlendShapesPresetManager : MonoBehaviour
    {
        public List<BlendShapePreset> presets = new List<BlendShapePreset>();
        [HideInInspector] public string[] onTriggerMode = new[] {"Explicit", "Random"};
        
        [HideInInspector] public List<Shape> shapeList = new List<Shape>();
        [HideInInspector] public string[] shapeListNames;
        [HideInInspector] public int shapeListIndex = 0;
        
        [HideInInspector] public bool showHelpBoxes = true;
        [HideInInspector] public bool showFullInspector = false;
        [HideInInspector] public bool showSetup = true;
        
        public BlendShapesManager BlendShapesManager => GetBlendShapesManager();
        private BlendShapesManager _blendShapesManager;
        private BlendShapesManager GetBlendShapesManager()
        {
            if (_blendShapesManager != null) return _blendShapesManager;
            if (TryGetComponent(out BlendShapesManager foundManager))
                _blendShapesManager = foundManager;
            return _blendShapesManager;
        }
        
        /// <summary>
        /// Activates an individual preset
        /// </summary>
        /// <param name="index"></param>
        public void ActivatePreset(int index)
        {
            for (int v = 0; v < presets[index].presetValues.Count; v++)
            {
                BlendShapePresetValue presetValue = presets[index].presetValues[v];
                BlendShapeGameObject obj = BlendShapesManager.GetBlendShapeObject(presetValue.objectName);
                BlendShapeValue value = BlendShapesManager.GetBlendShapeValue(obj, presetValue.valueTriggerName);

                value.value = presetValue.onTriggerMode == "Explicit"
                    ? presetValue.shapeValue
                    : Random.Range(presetValue.limitMin, presetValue.limitMax);
                BlendShapesManager.TriggerShape(obj,value);
            }
        }

        /// <summary>
        /// Activates an individual named preset
        /// </summary>
        /// <param name="name"></param>
        public void ActivatePreset(string name)
        {
            for (int i = 0; i < presets.Count; i++)
            {
                if (presets[i].name != name)
                    continue;
                
                ActivatePreset(i);
                return;
            }
        }
    }

    [System.Serializable]
    public class BlendShapePreset
    {
        public string name;
        public List<BlendShapePresetValue> presetValues = new List<BlendShapePresetValue>();
        [HideInInspector] public bool showValues = false;
    }

    [System.Serializable]
    public class BlendShapePresetValue
    {
        public string objectName;
        public string valueTriggerName;
        public string onTriggerMode;
        [HideInInspector] public int onTriggerModeIndex = 0;
        public float shapeValue;

        public float limitMin;
        public float limitMax;
        public float min;
        public float max;
    }

    [System.Serializable]
    public class Shape
    {
        public BlendShapeGameObject obj;
        public BlendShapeValue value;
    }

}