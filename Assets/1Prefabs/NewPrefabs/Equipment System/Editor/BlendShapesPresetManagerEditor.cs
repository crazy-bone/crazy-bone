using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using InfinityPBR;

[CustomEditor(typeof(BlendShapesPresetManager))]
[CanEditMultipleObjects]
[Serializable]
public class BlendShapesPresetManagerEditor : Editor
{
    private Color inactiveColor2 = new Color(0.75f, .75f, 0.75f, 1f);
    private Color activeColor = new Color(0.6f, 1f, 0.6f, 1f);
    private Color activeColor2 = new Color(0.0f, 1f, 0.0f, 1f);
    private Color mixedColor = Color.yellow;
    private Color redColor = new Color(1f, 0.25f, 0.25f, 1f);

    private BlendShapesPresetManager Manager => GetManager();
    private BlendShapesPresetManager _blendShapesPresetManager;
    private BlendShapesManager BlendShapesManager => Manager.BlendShapesManager;

    private BlendShapesPresetManager GetManager()
    {
        if (_blendShapesPresetManager != null) return _blendShapesPresetManager;
        _blendShapesPresetManager = (BlendShapesPresetManager) target;
        return _blendShapesPresetManager;
    }

    public override void OnInspectorGUI()
    {
        if (Manager.showHelpBoxes)
        {
            EditorGUILayout.HelpBox("BLEND SHAPE PRESET MANAGER\n" +
                                    "Use this script with BlendShapesManager.cs to create groups of preset shapes, which can " +
                                    "be called with a single line of code. For example, you may wish to create a \"Strong\" or " +
                                    "a \"Weak\" version of a character, or have multiple face settings.\n\nYou can also set some " +
                                    "shapes to randomize on load, which will allow you to create random character looks whenever " +
                                    "one is instantiated.", MessageType.None);
        }

        ShowSetupAndOptions();

        DrawLine();

        ShowButtons();

        ShowPresetList();

        ShowDefaultInspector();
    }

    private void ShowDefaultInspector()
    {
        if (!Manager.showFullInspector) return;
        
        DrawLine();
        EditorGUILayout.Space();
        DrawDefaultInspector();
    }

    private void ShowPresetList()
    {
        // DISPLAY LIST
        for (int i = 0; i < Manager.presets.Count; i++)
        {
            BlendShapePreset preset = Manager.presets[i];
            ShowPreset(preset, i);
        }
    }

    private void ShowPreset(BlendShapePreset preset, int presetIndex)
    {
        GUI.backgroundColor = preset.showValues ? activeColor : Color.white;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(preset.showValues ? "o" : "=", GUILayout.Width(20)))
        {
            preset.showValues = !preset.showValues;
        }
        EditorGUILayout.LabelField("Preset Name",GUILayout.Width(150));
        preset.name = EditorGUILayout.TextField(preset.name);
        if (GUILayout.Button("Activate", GUILayout.Width(150)))
        {
            Manager.ActivatePreset(presetIndex);
        }
        if (GUILayout.Button("Copy", GUILayout.Width(150)))
        {
            CopyPreset(Manager, presetIndex);
        }
        
        EditorGUILayout.EndHorizontal();
        
        
        if (preset.showValues)
        {
            EditorGUI.indentLevel++;
            for (int v = 0; v < preset.presetValues.Count; v++)
            {
                
                BlendShapePresetValue value = preset.presetValues[v];
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = redColor;
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    Undo.RecordObject(Manager, "Undo Record");
                    preset.presetValues.RemoveAt(v);
                    GUI.backgroundColor = preset.showValues ? activeColor : Color.white;
                }
                else
                {
                    
                    GUI.backgroundColor = preset.showValues ? activeColor : Color.white;
                    EditorGUILayout.LabelField(value.objectName + " " + value.valueTriggerName);
                    
                    Undo.RecordObject(Manager, "Undo Record");
                    value.onTriggerModeIndex = EditorGUILayout.Popup(value.onTriggerModeIndex, Manager.onTriggerMode);
                    value.onTriggerMode = Manager.onTriggerMode[value.onTriggerModeIndex];
                    if (value.onTriggerMode == "Explicit")
                    {
                        Undo.RecordObject(Manager, "Undo Record");
                        value.shapeValue = EditorGUILayout.Slider(value.shapeValue, value.min, value.max);
                    }

                    if (value.onTriggerMode == "Random")
                    {
                        //EditorGUILayout.LabelField("This value will be randomized.");
                    }
                    
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Add new shape", GUILayout.Width(150));
            Manager.shapeListIndex = EditorGUILayout.Popup(Manager.shapeListIndex, Manager.shapeListNames);
            if (GUILayout.Button("Add Blendshape"))
            {
                AddNewPresetValue(Manager, preset);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            
            EditorGUILayout.Space();
            if (GUILayout.Button("Set all Explicit"))
            {
                for (int v = 0; v < preset.presetValues.Count; v++)
                {
                    preset.presetValues[v].onTriggerModeIndex = 0;
                    preset.presetValues[v].onTriggerMode = "Explicit";
                }
            }
            if (GUILayout.Button("Set all Random"))
            {
                for (int v = 0; v < preset.presetValues.Count; v++)
                {
                    preset.presetValues[v].onTriggerModeIndex = 1;
                    preset.presetValues[v].onTriggerMode = "Random";
                }
            }
            if (GUILayout.Button("Set values = 0"))
            {
                for (int v = 0; v < preset.presetValues.Count; v++)
                {
                    preset.presetValues[v].shapeValue = 0f;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            DrawLine();
            GUI.backgroundColor = redColor;
            if (GUILayout.Button("Remove This Preset"))
            {
                Manager.presets.RemoveAt(presetIndex);
            }
            GUI.backgroundColor = preset.showValues ? activeColor : Color.white;
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
    }

    private void ShowButtons()  
    {
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Add new preset group"))
            AddNewPresetGroup(Manager);

        if (GUILayout.Button("Reload Blendshape List"))
            BuildShapeList(BlendShapesManager, Manager);
        
        EditorGUILayout.EndHorizontal();
    }

    private void ShowSetupAndOptions()
    {
        Manager.showSetup = EditorGUILayout.Foldout(Manager.showSetup, "Setup & Options");
                if (Manager.showSetup)
                {
                    EditorGUI.indentLevel++;
                    Manager.showHelpBoxes = EditorGUILayout.Toggle("Show help boxes", Manager.showHelpBoxes);
                    Manager.showFullInspector = EditorGUILayout.Toggle("Show full inspector", Manager.showFullInspector);
                    
                    EditorGUI.indentLevel--;
                }
    }

    private void BuildShapeList(BlendShapesManager blendShapesManager, BlendShapesPresetManager presetManager)
    {
        Debug.Log("BuildShapeList");
        presetManager.shapeList.Clear();
        presetManager.shapeListIndex = 0;

        for (int o = 0; o < blendShapesManager.blendShapeGameObjects.Count; o++)
        {
            BlendShapeGameObject obj = blendShapesManager.blendShapeGameObjects[o];
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

                presetManager.shapeList.Add(new Shape());
                Shape newShape = presetManager.shapeList[presetManager.shapeList.Count - 1];
                newShape.obj = obj;
                newShape.value = value;
            }
        }

        presetManager.shapeListNames = new string[presetManager.shapeList.Count];
        for (int i = 0; i < presetManager.shapeList.Count; i++)
        {
            presetManager.shapeListNames[i] = presetManager.shapeList[i].obj.gameObjectName + " " +
                                              presetManager.shapeList[i].value.triggerName;
        }

        UpdatePresetLimits(presetManager);
    }

    private void UpdatePresetLimits(BlendShapesPresetManager presetManager)
    {
        foreach (BlendShapePreset preset in presetManager.presets)
        {
            foreach (BlendShapePresetValue value in preset.presetValues)
            {
                BlendShapeValue shapeValue = BlendShapesManager.GetBlendShapeValue(value.objectName, value.valueTriggerName);
                value.limitMin = shapeValue.limitMin;
                value.limitMax = shapeValue.limitMax;
                
            }
        }
    }

    private void AddNewPresetValue(BlendShapesPresetManager presetManager, BlendShapePreset preset)
    {
        preset.presetValues.Add(new BlendShapePresetValue());
        BlendShapePresetValue newValue = preset.presetValues[preset.presetValues.Count - 1];

        Shape shape = presetManager.shapeList[presetManager.shapeListIndex];

        newValue.objectName = shape.obj.gameObjectName;
        newValue.valueTriggerName = shape.value.triggerName;
        newValue.limitMin = shape.value.limitMin;
        newValue.limitMax = shape.value.limitMax;
        newValue.min = shape.value.min;
        newValue.max = shape.value.max;
    }

    private void AddNewPresetGroup(BlendShapesPresetManager presetManager)
    {
        presetManager.presets.Add(new BlendShapePreset());
    }

    private void CopyPreset(BlendShapesPresetManager presetManager, int presetIndex)
    {
        AddNewPresetGroup(presetManager);
        BlendShapePreset copyFrom = presetManager.presets[presetIndex];
        BlendShapePreset copyTo = presetManager.presets[presetManager.presets.Count - 1];
        copyTo.name = copyFrom.name + " Copy";
        copyTo.presetValues = new List<BlendShapePresetValue>();
        for (int v = 0; v < copyFrom.presetValues.Count; v++)
        {
            copyTo.presetValues.Add(new BlendShapePresetValue());
            copyTo.presetValues[v].max = copyFrom.presetValues[v].max;
            copyTo.presetValues[v].min = copyFrom.presetValues[v].min;
            copyTo.presetValues[v].limitMax = copyFrom.presetValues[v].limitMax;
            copyTo.presetValues[v].limitMin = copyFrom.presetValues[v].limitMin;
            copyTo.presetValues[v].shapeValue = copyFrom.presetValues[v].shapeValue;
            copyTo.presetValues[v].objectName = copyFrom.presetValues[v].objectName;
            copyTo.presetValues[v].onTriggerMode = copyFrom.presetValues[v].onTriggerMode;
            copyTo.presetValues[v].valueTriggerName = copyFrom.presetValues[v].valueTriggerName;
            copyTo.presetValues[v].onTriggerModeIndex = copyFrom.presetValues[v].onTriggerModeIndex;
        }

        copyTo.showValues = copyFrom.showValues;
    }

    void DrawLine(bool spacers = true, int height = 1)
    {
        if (spacers)
            EditorGUILayout.Space();
        Rect rect = EditorGUILayout.GetControlRect(false, height );
        rect.height = height;
        EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
        if (spacers)
            EditorGUILayout.Space();
    }
}
