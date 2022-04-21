using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using InfinityPBR;

[CustomEditor(typeof(WardrobePrefabManager))]
[CanEditMultipleObjects]
[Serializable]
public class WardrobePrefabManagerEditor : Editor
{

    
    private Color inactiveColor2 = new Color(0.75f, .75f, 0.75f, 1f);
    private Color activeColor = new Color(0.6f, 1f, 0.6f, 1f);
    private Color activeColor2 = new Color(0.0f, 1f, 0.0f, 1f);
    private Color mixedColor = Color.yellow;
    private Color redColor = new Color(1f, 0.25f, 0.25f, 1f);
    
    
    private string[] actionTypes = new string[] {"Explicit", "Less than", "Greater than"};

    private BlendShapesManager BlendShapesManager => Manager.BlendShapesManager;
    private PrefabAndObjectManager PrefabAndObjectManager => Manager.PrefabAndObjectManager;
    private WardrobePrefabManager Manager => GetManager();
    private WardrobePrefabManager _wardrobePrefabManager;

    private bool _initialCheck = false;
    
    private WardrobePrefabManager GetManager()
    {
        if (_wardrobePrefabManager != null) return _wardrobePrefabManager;
        _wardrobePrefabManager = (WardrobePrefabManager) target;
        return _wardrobePrefabManager;
    }

    private void OnValidate()
    {
        Manager.UpdateGroupList();
    }

    public override void OnInspectorGUI()
    {
        InitialCheck();
        
        if (EditorPrefs.GetBool("Wardrobe Prefab Manager Show Help Boxes"))
        {
            HelpBoxMessage("WARDROBE PREFAB MANAGER\n\n" +
                           "While this script holds data which you can see if you toggle on \"Show full inspector\", the " +
                           "data is managed by the Prefab And Object Manager. New options will be visible when this component " +
                           "is also attached.\n\n" +
                           "-- Required Component: Prefab And Object Manager\n" +
                           "-- Required Component: Blend Shapes Manager\n\n" +
                           "This component is intended to be used in conjunction with the Prefab And Object " +
                           "Manager, and (optionally) the Blend Shapes Manager. When prefabs are instantiated " +
                           "using the Prefab And Object Manager, this script will handle rigging, and will also " +
                           "handle the Blend Shapes as well. Be sure to toggle on \"Auto rig when activated\" to " +
                           "have new wardrobe rigged to the character.\n\nBe sure to toggle on \"Handle Blend Shapes\" " +
                           "if you plan on using mesh morphing via the Blend Shapes Manager.", MessageType.Warning);
        }
        else
        {
            HelpBoxMessage("Data is managed by the Prefab And Object Manager!", MessageType.Warning, true);
        }

        ReloadButton();
        
        ShowSetupAndOptions();
        
        ShowFullInspector();
        EditorUtility.SetDirty(this);
    }

    private void ReloadButton()
    {
        if (GUILayout.Button("Manually Update Group List & Blend Shapes"))
            Manager.UpdateGroupList();
    }

    private void InitialCheck()
    {
        if (PrefabAndObjectManager == null) return;
        if (Manager.blendShapeGroups.Count == PrefabAndObjectManager.prefabGroups.Count) return;
        Manager.UpdateGroupList();
    }

    private void ShowButtons()
    {
        SectionButton($"Setup & Options", "Wardrobe Prefab Manager Setup & Options");
    }

    private void ShowBlendShapes()
    {
        if (BlendShapesManager == null) return;
        if (!Manager.handleBlendShapes) return;

        DrawLine();
        HelpBoxMessage("Blend Shape Groups\n\n" +
                       "When a group is activated or deactivated using the Prefab And Object Manager component, you can " +
                       "automatically set specific values for specific blend shapes. This is useful when a shape may need " +
                       "to be set to a specific value when some wardrobe is active.\n\nAn example could be \"Nose Size\", which " +
                       "may need to be smaller when a specific \"Helmet\" wardrobe is active, or \"Breasts Size\" when specific " +
                       "torso wardrobe is active.\n\n" +
                       "You can set a specific \"On Activate\" value for blend shapes, and also \"On Deactivate\", including " +
                       "having the shape return to it's last value. This would allow the nose in the example above to " +
                       "automatically revert to the proper blend shape value when the helmet is removed.", MessageType.None);

        EditorGUILayout.BeginHorizontal();
        SectionButton($"Blend Shape Groups", "Wardrobe Prefab Manager Blend Shapes");
        if (!EditorPrefs.GetBool("Wardrobe Prefab Manager Blend Shapes"))
        {
            EditorGUILayout.EndHorizontal();
            return;
        }

        var countMismatch = PrefabAndObjectManager.prefabGroups.Count != Manager.blendShapeGroups.Count;
        if (countMismatch)
            Manager.UpdateGroupList();
        /*
        GUI.backgroundColor = countMismatch ? Color.red : Color.white;
        if (GUILayout.Button("Update Group List", GUILayout.Width(150)))
            Manager.UpdateGroupList();
        EditorGUILayout.LabelField(new GUIContent($"{symbolInfo}", "Click this after you make changes to the Prefab Groups in the " +
                                                                   "Prefab And Object Manager."), GUILayout.Width(30));
        */
        EditorGUILayout.EndHorizontal();

        /*
        if (countMismatch)
        {
            HelpBoxMessage($"WARNING: Prefab And Object Manager has {PrefabAndObjectManager.prefabGroups.Count} groups, but this component " +
                           $"is showing {Manager.blendShapeGroups.Count}. Click the \"Update Group List\" button to update the groups.", MessageType.Warning);
        }
        */
        
        GUI.backgroundColor = Color.white;
        
        foreach(var typeName in Manager.PrefabAndObjectManager.GroupTypeNames)
            ShowGroupsOfType(typeName);
    }

    private List<PrefabGroup> GroupsOfType(string groupType) => String.IsNullOrWhiteSpace(groupType) ? Manager.PrefabAndObjectManager.prefabGroups.Where(x => String.IsNullOrWhiteSpace(x.groupType)).ToList() : Manager.PrefabAndObjectManager.prefabGroups.Where(x => x.groupType == groupType).ToList();
    
    private void ShowGroupsOfType(string typeName)
    {
        var groupsOfType = GroupsOfType(typeName);
        
        var typeDetails = $"{groupsOfType.Count}";

        EditorPrefs.SetBool($"Prefab Manager Show Type {typeName}", EditorGUILayout.Foldout(EditorPrefs.GetBool($"Prefab Manager Show Type {typeName}"), $"{(!String.IsNullOrWhiteSpace(typeName) ? $"{typeName}" : "[No type]")} ({typeDetails})"));
        if (!EditorPrefs.GetBool($"Prefab Manager Show Type {typeName}")) return;
        
        for (int g = 0; g < Manager.blendShapeGroups.Count; g++)
        {
            //EditorGUILayout.Space();
            BlendShapeGroup group = Manager.blendShapeGroups[g];
            group.showList = EditorGUILayout.Foldout(group.showList, group.prefabGroup.name);
            if (group.showList)
            {
                EditorGUI.indentLevel++;

                string[] blendShapeNames = group.blendShapeNames.ToArray();

                // ON ACTIVATE
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("On Activate", GUILayout.Width(120));

                if (group.blendShapeNames.Count == 0)
                {
                    EditorGUILayout.LabelField("No Blend Shapes Available");
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    group.shapeChoiceIndex = EditorGUILayout.Popup(group.shapeChoiceIndex, blendShapeNames);
                    if (GUILayout.Button("Add To List"))
                        AddToList("Activate", group);
                    EditorGUILayout.EndHorizontal();
                }

                // ON ACTIVATE GLOBAL
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Global Shapes", GUILayout.Width(120));

                if (BlendShapesManager.matchList.Count == 0)
                {
                    EditorGUILayout.LabelField("No Global Blend Shapes Available");
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    BlendShapesManager.matchListIndex = EditorGUILayout.Popup(BlendShapesManager.matchListIndex,
                        BlendShapesManager.matchListNames);
                    if (GUILayout.Button("Add To List"))
                        AddToList("Activate", BlendShapesManager.matchList[BlendShapesManager.matchListIndex],
                            group);
                    EditorGUILayout.EndHorizontal();
                }

                // ON ACTIVATE LIST
                for (int i = 0; i < group.onActivate.Count; i++)
                {
                    BlendShapeItem item = group.onActivate[i];
                    DisplayItem(group, item, i, "Activate", Manager);
                }

                EditorGUILayout.EndVertical();



                // ON DEACTIVATE
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("On Deactivate", GUILayout.Width(120));

                if (group.blendShapeNames.Count == 0)
                {
                    EditorGUILayout.LabelField("No Blend Shapes Available");
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    group.shapeChoiceIndex = EditorGUILayout.Popup(group.shapeChoiceIndex, blendShapeNames);
                    if (GUILayout.Button("Add To List"))
                        AddToList("Deactivate", group);
                    if (GUILayout.Button("Revert Back"))
                        AddToList("Revert Back", group);
                    EditorGUILayout.EndHorizontal();
                }

                // ON DEACTIVATE GLOBAL
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Global Shapes", GUILayout.Width(120));

                if (BlendShapesManager.matchList.Count == 0)
                {
                    EditorGUILayout.LabelField("No Global Blend Shapes Available");
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    BlendShapesManager.matchListIndex = EditorGUILayout.Popup(BlendShapesManager.matchListIndex,
                        BlendShapesManager.matchListNames);
                    if (GUILayout.Button("Add To List"))
                        AddToList("Deactivate", BlendShapesManager.matchList[BlendShapesManager.matchListIndex],
                            group);
                    if (GUILayout.Button("Revert Back"))
                        AddToList("Revert Back",
                            BlendShapesManager.matchList[BlendShapesManager.matchListIndex], group);
                    EditorGUILayout.EndHorizontal();
                }

                // ON ACTIVATE LIST
                for (int i = 0; i < group.onDeactivate.Count; i++)
                {
                    BlendShapeItem item = group.onDeactivate[i];
                    DisplayItem(group, item, i, "Deactivate", Manager);
                }

                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;
            }
        }
    }

    private void ShowFullInspector()
    {
        if (!Manager.showFullInspector) return;
        
        EditorGUILayout.Space();
        DrawDefaultInspector();
    }

    private void ShowSetupAndOptions()
    {
        //if (!EditorPrefs.GetBool("Wardrobe Prefab Manager Setup & Options")) return;
        
        EditorGUI.indentLevel++;
        EditorPrefs.SetBool("Wardrobe Prefab Manager Show Help Boxes", EditorGUILayout.Toggle("Show help boxes", EditorPrefs.GetBool("Wardrobe Prefab Manager Show Help Boxes")));
        Manager.showFullInspector = EditorGUILayout.Toggle("Show full Inspector", Manager.showFullInspector);
        Manager.autoRigWhenActivated = EditorGUILayout.Toggle(new GUIContent($"Auto rig when activated {symbolInfo}", 
            $"Toggle on if you want objects to rig themselves to the Skinned Mesh Renderer bone structure. This is used " +
            $"for equipping wardrobe, as an example."), Manager.autoRigWhenActivated);
        if (BlendShapesManager)
            Manager.handleBlendShapes = EditorGUILayout.Toggle(new GUIContent($"Handle blend shapes {symbolInfo}", 
                $"If true, the Blend Shapes Manager will handle blend shapes, which means that armor or wardrobe that " +
                $"is instantiated onto a character with customized blend shape values will have it's own blend shape values set, so " +
                $"the wardrobe looks correct."), Manager.handleBlendShapes);
        EditorGUI.indentLevel--;
    }

    private void SectionButton(string button, string prefs, int width = 150)
    {
        GUI.backgroundColor = EditorPrefs.GetBool(prefs) ? Color.green : Color.black;
        if (GUILayout.Button(button))
            EditorPrefs.SetBool(prefs, !EditorPrefs.GetBool(prefs));
        GUI.backgroundColor = Color.white;
    }
    
    private void HelpBoxMessage(string message, MessageType messageType = MessageType.None, bool forceShow = false)
    {
        if (!forceShow && !EditorPrefs.GetBool("Wardrobe Prefab Manager Show Help Boxes")) return;
        EditorGUILayout.HelpBox(message,messageType);
    }

    private void DisplayItem(BlendShapeGroup group, BlendShapeItem item, int itemIndex, string type, WardrobePrefabManager wardrobePrefabManager)
    {
        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = redColor;
        if (GUILayout.Button("X", GUILayout.Width(25)))
        {
            EditorGUILayout.EndHorizontal();

            if (type == "Activate")
                group.onActivate.RemoveAt(itemIndex);
            if (type == "Deactivate")
                group.onDeactivate.RemoveAt(itemIndex);

        }
        else
        {
            GUI.backgroundColor = Color.white;
            EditorGUILayout.LabelField(item.objectName + " " + item.triggerName);

            if (item.revertBack)
            {
                EditorGUILayout.LabelField("This value will revert to pre-activation value");
            }
            else
            {
                item.actionTypeIndex = EditorGUILayout.Popup(item.actionTypeIndex, actionTypes);
                item.actionType = actionTypes[item.actionTypeIndex];
                item.value = EditorGUILayout.Slider(item.value, item.min, item.max);
                if (GUILayout.Button("Set"))
                    wardrobePrefabManager.TriggerBlendShape(item.triggerName, item.value);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddToList(string eventType, MatchList matchList, BlendShapeGroup group)
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
    
    private void AddToList(string eventType, BlendShapeGroup group)
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
    
    private string symbolInfo = "ⓘ";
    private string symbolX = "✘";
    private string symbolCheck = "✔";
    private string symbolCheckSquare = "☑";
    private string symbolDollar = "$";
    private string symbolCent = "¢";
    private string symbolCarrotRight = "‣";
    private string symbolCarrotLeft = "◄";
    private string symbolCarrotUp = "▲";
    private string symbolCarrotDown = "▼";
    private string symbolDash = "⁃";
    private string symbolBulletClosed = "⦿";
    private string symbolBulletOpen = "⦾";
}
