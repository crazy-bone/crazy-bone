using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using InfinityPBR;

[CustomEditor(typeof(PrefabAndObjectManager))]
[CanEditMultipleObjects]
[Serializable]
public class PrefabAndObjectManagerEditor : Editor
{
    
    private Color inactiveColor2 = new Color(0.75f, .75f, 0.75f, 1f);
    private Color activeColor = new Color(0.6f, 1f, 0.6f, 1f);
    private Color activeColor2 = new Color(0.0f, 1f, 0.0f, 1f);
    private Color mixedColor = Color.yellow;
    private Color redColor = new Color(1f, 0.25f, 0.25f, 1f);

    private WardrobePrefabManager WardrobePrefabManager => Manager.WardrobePrefabManager;
    private BlendShapesManager BlendShapesManager => Manager.BlendShapesManager;

    private PrefabAndObjectManager Manager => GetManager();
    private PrefabAndObjectManager _prefabAndObjectManager;
    private List<string> GroupTypeNames => Manager.GetGroupTypeNames();

    private PrefabAndObjectManager GetManager()
    {
        if (_prefabAndObjectManager != null) return _prefabAndObjectManager;
        _prefabAndObjectManager = (PrefabAndObjectManager) target;
        return _prefabAndObjectManager;
    }


    public Color ColorSet(int g) => ColorSet(Manager.prefabGroups[g]);
    
    public Color ColorSet(PrefabGroup group)
    {
        int v = Manager.GroupIsActive(group);
        if (v == 2)
            return activeColor2;
        if (v == 1)
            return mixedColor;
        return Color.white;
    }

    private void DefaultEditorBool(string optionString, bool value = true)
    {
        if (EditorPrefs.HasKey(optionString)) return;
        EditorPrefs.SetBool(optionString, value);
    }

    public void OnEnable()
    {
        EnsureTypeNames();
        CacheTypes();
        if (BlendShapesManager)
            BlendShapesManager.BuildMatchLists();
        
    }

    private void EnsureTypeNames()
    {
        foreach (var group in Manager.prefabGroups)
        {
            if (!String.IsNullOrWhiteSpace(group.groupType)) continue;
            group.groupType = null;
        }
    }

    public override void OnInspectorGUI()
    {
        // Utility actions
        SetDefaultEditorPrefs();

        Manager.MarkPrefabs();
        Manager.SetUid();

        HelpBoxMessage("PREFAB AND OBJECT MANAGER\n" +
                       "This inspector script is intended to make it easier to assign groups of prefabs and objects, and" +
                       " activate / deactivate them as a group. This could be helpful for managing modular " +
                       "wardrobe or other objects, such as props inside a room.\n\n" +
                       "The script handles instantiating and destroying prefabs as well as activating and deactivating " +
                       "objects already in your scene. Each group can handle any number of both types of objects.");

        // Inspector Drawing
        SectionButtons();
        
        SetupAndOptions();

        GroupTypes();

        ShowPrefabGroups();
        DefaultInspector();

        EditorUtility.SetDirty(this);
    }

    /*
     * This shows the buttons to load the various panels
     */
    private void SectionButtons()
    {
        // Cache values
        var tempGroups = EditorPrefs.GetBool("Prefab Manager Show Prefab Groups");
        var tempTypes = EditorPrefs.GetBool("Prefab Manager Show Group Types");
        var tempSetup = EditorPrefs.GetBool("Prefab Manager Show Setup And Options");
        
        // Show buttons
        EditorGUILayout.BeginHorizontal();
        SectionButton($"Prefab Groups ({Manager.prefabGroups.Count})", "Prefab Manager Show Prefab Groups");
        SectionButton($"Group Types ({GroupTypeNames.Count})", "Prefab Manager Show Group Types");
        SectionButton("Setup & Options", "Prefab Manager Show Setup And Options");
        EditorGUILayout.EndHorizontal();
        
        // Check for changes -- Ensure others are turned off
        if (!tempGroups && EditorPrefs.GetBool("Prefab Manager Show Prefab Groups"))
        {
            EditorPrefs.SetBool("Prefab Manager Show Group Types", false);
            EditorPrefs.SetBool("Prefab Manager Show Setup And Options", false);
        }
        if (!tempTypes && EditorPrefs.GetBool("Prefab Manager Show Group Types"))
        {
            EditorPrefs.SetBool("Prefab Manager Show Prefab Groups", false);
            EditorPrefs.SetBool("Prefab Manager Show Setup And Options", false);
        }
        if (!tempSetup && EditorPrefs.GetBool("Prefab Manager Show Setup And Options"))
        {
            EditorPrefs.SetBool("Prefab Manager Show Group Types", false);
            EditorPrefs.SetBool("Prefab Manager Show Prefab Groups", false);
        }
    }

    private void GroupTypes()
    {
        if (!EditorPrefs.GetBool("Prefab Manager Show Group Types")) return;

        HelpBoxMessage("Organize your groups into types, often used to ensure that only one group " +
                       "of each type is active at a time. An example would be \"Hair\" for characters, or " +
                       "perhaps \"Table Items\" for props on the top of a table. You can update the name of a type" +
                       "here.\n\n" +
                       "To add a new type, simply write it into the \"Type\" field when viewing your prefab groups. " +
                       "Each group starts without a type.\n\n" +
                       "To delete a type, remove all groups of that type, or change all groups to a different type.", MessageType.Info);

        foreach (var typeName in GroupTypeNames)
            DisplayGroupType(typeName);
    }

    private void DisplayGroupType(string typeName)
    {
        if (String.IsNullOrWhiteSpace(typeName)) return;
        
        var oldName = typeName;
        var newName = EditorGUILayout.DelayedTextField(typeName, GUILayout.Width(250));
        if (oldName == newName) return;

        if (!UpdateGroupTypeName(oldName, newName))
            return;

        EditorPrefs.SetBool($"Prefab Manager Show Type {newName}", true);
    }

    /*
     * This will check to see if we can update the type name. If so, will change all the groups of the type to
     * the new name.
     */
    private bool UpdateGroupTypeName(string oldName, string newName)
    {
        newName = newName.Trim(); // Remove any whitespace before / after the content
        if (String.IsNullOrWhiteSpace(newName)) return false; // If it's empty, return
        if (oldName == newName) return false; // If we didn't change anything, return
        if (GroupTypeNames.Count(x => x == newName) > 0) // If we already have a type of that name, return
        {
            Debug.LogWarning($"Error: {newName} already exists!");
            return false;
        }

        // Make the update on all existing prefab groups
        foreach (var group in Manager.prefabGroups)
        {
            if (group.groupType != oldName) continue;
            group.groupType = newName;
        }

        CacheTypes(); // force the types cache to reload
        
        return true;
    }

    private void CacheTypes(bool value = true) => Manager.cacheTypes = value;

    
    
    private void DefaultInspector()
    {
        if (!EditorPrefs.GetBool("Prefab Manager Show Full Inspector")) return;
        
        EditorGUILayout.Space();
        DrawDefaultInspector();
    }

    private void ShowPrefabGroups()
    {
        EditorGUILayout.BeginHorizontal();
        if (!EditorPrefs.GetBool("Prefab Manager Show Prefab Groups"))
        {
            EditorGUILayout.EndHorizontal();
            return;
        }
        
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Create New Group", GUILayout.Width(150)))
        {
            Manager.CreateNewPrefabGroup();
            CacheTypes(); // force the types cache to reload
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUILayout.EndHorizontal();

        foreach(var typeName in GroupTypeNames)
            ShowGroupsOfType(typeName);
    }

    private List<PrefabGroup> GroupsOfType(string groupType) => String.IsNullOrWhiteSpace(groupType) ? Manager.prefabGroups.Where(x => String.IsNullOrWhiteSpace(x.groupType)).ToList() : Manager.prefabGroups.Where(x => x.groupType == groupType).ToList();

    private void ShowGroupsOfType(string typeName = "")
    {
        var groupsOfType = GroupsOfType(typeName);
        
        var typeDetails = $"{groupsOfType.Count}";

        EditorPrefs.SetBool($"Prefab Manager Show Type {typeName}", EditorGUILayout.Foldout(EditorPrefs.GetBool($"Prefab Manager Show Type {typeName}"), $"{(!String.IsNullOrWhiteSpace(typeName) ? $"{typeName}" : "[No type]")} ({typeDetails})"));
        if (!EditorPrefs.GetBool($"Prefab Manager Show Type {typeName}")) return;

        

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(10)); // Indent
        ShowDefaultToggle(null, true);
        ShowObjectsButton(null, true);
        ShowShapesButton(null, true);
        ShowGroupName(null, true);
        ShowGroupType(null, true);
        ShowGroupActivateDeactivate(null, true);
        ShowRemovePrefabGroup(null, true);
        EditorGUILayout.EndHorizontal();
        
        foreach (var group in groupsOfType)
            ShowPrefabGroupRow(group);
        
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button($"Create New{(string.IsNullOrWhiteSpace(typeName) ? "" : $" {typeName}")} Group", GUILayout.Width(250)))
        {
            Manager.CreateNewPrefabGroup(typeName);
            CacheTypes(); // force the types cache to reload
        }
        GUI.backgroundColor = Color.white;

        GUI.backgroundColor =  Color.white;
        
    }

    private void ShowPrefabGroupRow(PrefabGroup group)
    {
        GUI.backgroundColor = ColorSet(group);
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(10)); // Indent
        
        ShowDefaultToggle(group);
        ShowObjectsButton(group);
        ShowShapesButton(group);
        ShowGroupName(group);
        ShowGroupType(group);
        ShowGroupActivateDeactivate(group);
        ShowRemovePrefabGroup(group);
        
        EditorGUILayout.EndHorizontal();

        ShowObjects(group);
        ShowShapes(group);
        
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndVertical();
    }

    private void ShowObjects(PrefabGroup group)
    {
        if (!group.showPrefabs) return; // return if we haven't toggled this on
        
        // Show each of the objects attached to this Prefab Group
        for (int i = 0; i < group.groupObjects.Count; i++)
        {
            GroupObject groupObject = group.groupObjects[i];
            ShowGroupObject(group, groupObject);
            GUI.backgroundColor = Color.white;
        }

        ShowAddNewObjectField(group);
    }
    
    private void ShowShapes(PrefabGroup group)
    {
        if (!group.showShapes) return; // return if we haven't toggled this on
        if (!WardrobePrefabManager)
        {
            Debug.Log("No Wardrobe Prefab Manager");
            return;
        }

        var blendShapeGroup = WardrobePrefabManager.GetGroup(group);
        if (blendShapeGroup == null)
        {
            Debug.Log($"Group {group.name} is null??");
            return;
        }
        
        string[] blendShapeNames = blendShapeGroup.blendShapeNames.ToArray();

        // ON ACTIVATE
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"On Activate {symbolInfo}", $"These shapes will be modified when this Prefab Group is " +
                                                                               $"activated. This is how you can set shapes to make sure they fit with the " +
                                                                               $"wardrobe in the Prefab Group. Select a shape, and then click \"Add To List\"."), GUILayout.Width(120));

        if (blendShapeGroup.blendShapeNames.Count == 0)
        {
            EditorGUILayout.LabelField("No Blend Shapes Available");
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            blendShapeGroup.shapeChoiceIndex = EditorGUILayout.Popup(blendShapeGroup.shapeChoiceIndex, blendShapeNames);
            if (GUILayout.Button("Add To List"))
            {
                WardrobePrefabManager.AddToList("Activate", blendShapeGroup);
                SetAllDirty();
            }
            EditorGUILayout.EndHorizontal();
        }

        // ON ACTIVATE GLOBAL
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"Global Shapes {symbolInfo}", $"The top list contains only the shapes assigned to " +
                                                                                 $"the objects in the Prefab Group. \"Global Shapes\" will list all " +
                                                                                 $"shapes available."), GUILayout.Width(120));

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
            {
                WardrobePrefabManager.AddToList("Activate",
                    BlendShapesManager.matchList[BlendShapesManager.matchListIndex], blendShapeGroup);
                SetAllDirty();
            }
            EditorGUILayout.EndHorizontal();
        }

        // ON ACTIVATE LIST
        for (int i = 0; i < blendShapeGroup.onActivate.Count; i++)
        {
            BlendShapeItem item = blendShapeGroup.onActivate[i];
            WardrobePrefabManagerDisplayItem(blendShapeGroup, item, i, "Activate", WardrobePrefabManager);
        }

        EditorGUILayout.EndVertical();
        

        // ON DEACTIVATE
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"On Deactivate {symbolInfo}", $"These shapes will be triggered when the Prefab Group is " +
                                                                                 $"deactivated. Often the shapes will match those in \"On Activate\"."), GUILayout.Width(120));

        if (blendShapeGroup.blendShapeNames.Count == 0)
        {
            EditorGUILayout.LabelField("No Blend Shapes Available");
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            blendShapeGroup.shapeChoiceIndex = EditorGUILayout.Popup(blendShapeGroup.shapeChoiceIndex, blendShapeNames);
            if (GUILayout.Button("Add To List"))
            {
                WardrobePrefabManager.AddToList("Deactivate", blendShapeGroup);
                SetAllDirty();
            }
            if (GUILayout.Button("Revert Back"))
            {
                WardrobePrefabManager.AddToList("Revert Back", blendShapeGroup);
                SetAllDirty();
            }
            EditorGUILayout.EndHorizontal();
        }

        // ON DEACTIVATE GLOBAL
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"Global Shapes {symbolInfo}", $"The top list contains only the shapes assigned to " +
                                                                                 $"the objects in the Prefab Group. \"Global Shapes\" will list all " +
                                                                                 $"shapes available."), GUILayout.Width(120));

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
            {
                WardrobePrefabManager.AddToList("Deactivate",
                    BlendShapesManager.matchList[BlendShapesManager.matchListIndex],
                    blendShapeGroup);
                SetAllDirty();
            }
            if (GUILayout.Button("Revert Back"))
            {
                WardrobePrefabManager.AddToList("Revert Back",
                    BlendShapesManager.matchList[BlendShapesManager.matchListIndex], blendShapeGroup);
                SetAllDirty();
            }
            EditorGUILayout.EndHorizontal();
        }

        // ON ACTIVATE LIST
        for (int i = 0; i < blendShapeGroup.onDeactivate.Count; i++)
        {
            BlendShapeItem item = blendShapeGroup.onDeactivate[i];
            WardrobePrefabManagerDisplayItem(blendShapeGroup, item, i, "Deactivate", WardrobePrefabManager);
        }

        EditorGUILayout.EndVertical();
    }

    private void SetAllDirty(){
        EditorUtility.SetDirty(this);
    }
    
    private void WardrobePrefabManagerDisplayItem(BlendShapeGroup group, BlendShapeItem item, int itemIndex, string type, WardrobePrefabManager wardrobePrefabManager)
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
                EditorGUILayout.LabelField(new GUIContent($"{symbolInfo}", $"\"Explicit\" will set the shape to the value specified, while \"Less than\" will set the shape to ensure it is " +
                                                                           $"less than or equal to the value specified. \"Greater than\" will set it to make sure it is greater than or equal to the " +
                                                                           $"specified value.\n\nUse the \"Set\" button to set the value selected and visualize the outcome.\n\n" +
                                                                           $"[On Deactivate] Use \"Revert Back\" to make this shape revert back to it's previous value that was active when the " +
                                                                           $"Prefab Group was activated."), GUILayout.Width(25));
                item.actionTypeIndex = EditorGUILayout.Popup(item.actionTypeIndex, WardrobePrefabManager.actionTypes);
                item.actionType = WardrobePrefabManager.actionTypes[item.actionTypeIndex];
                item.value = EditorGUILayout.Slider(item.value, item.min, item.max);
                if (GUILayout.Button("Set"))
                    wardrobePrefabManager.TriggerBlendShape(item.triggerName, item.value);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    /*
     * This is where we add new objects to the Prefab Group, users can drag/drop or select an object to add it to the
     * list.
     */
    private void ShowAddNewObjectField(PrefabGroup group)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUI.contentColor = Color.yellow;
        GUI.backgroundColor = Color.yellow;
        group.newPrefab = EditorGUILayout.ObjectField(new GUIContent($"Add Prefab or Child Object to Group {symbolInfo}", 
            $"Drag or select a Prefab from your project or a Game Object from the scene to add it to {group.name}. " +
            $"You can mix both types in each group. Prefabs will be instantiated and destroyed, and Game Objects will " +
            $"be turned on and off when this group is activated or deactivated."), group.newPrefab, typeof(GameObject), true) as GameObject;
        if (group.newPrefab)
        {
            if (group.newPrefab != null && group.newPrefab.transform.IsChildOf(Manager.transform))
                Manager.AddGameObjectToGroup(group.newGameObject, group);
            else if (PrefabUtility.IsPartOfAnyPrefab(group.newPrefab))
                Manager.AddPrefabToGroup(group.newPrefab, group);
            else if (group.newPrefab != null)
                Debug.LogError("Error: " + group.newPrefab.name +
                               " isn't a prefab that can be added, or isn't a child of the parent object.");

            group.newPrefab = null;

            if (Manager.instantiatePrefabsAsAdded)
                Manager.ActivateGroup(group);
        }
        GUI.contentColor = Color.white;
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void ShowGroupObject(PrefabGroup group, GroupObject groupObject)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(30)); // Indent

        ShowObjectFields(group, groupObject);
        ShowObjectDelete(group, groupObject);

        CheckOptionsAreSet(group, groupObject);
        
        EditorGUILayout.EndHorizontal();
    }

    private void ShowObjectFields(PrefabGroup group, GroupObject groupObject)
    {
        Undo.RecordObject(Manager, "Update Prefab Object In Group");
        GameObject oldObject = groupObject.objectToHandle;
        groupObject.objectToHandle =
            EditorGUILayout.ObjectField(groupObject.objectToHandle, typeof(GameObject),
                !groupObject.isPrefab) as GameObject;
        if (oldObject != groupObject.objectToHandle)
        {
            if (groupObject.isPrefab)
            {
                if (!PrefabUtility.IsPartOfAnyPrefab(groupObject.objectToHandle))
                {
                    groupObject.objectToHandle = oldObject;
                    Debug.LogError("Error: This isn't a prefab that can be added.");
                }
                else
                {
                    Event e = Event.current;
                    if (e.shift)
                        UpdateAllObjects(oldObject, groupObject.objectToHandle);
                }
            }
            else
            {
                if (!groupObject.objectToHandle.transform.IsChildOf(Manager.transform))
                {
                    groupObject.objectToHandle = oldObject;
                    Debug.LogError("Error: This isn't a GameObject that can be added.");
                }
                else
                {
                    Event e = Event.current;
                    if (e.shift)
                        UpdateAllObjects(oldObject, groupObject.objectToHandle);
                }
            }

            if (Manager.instantiatePrefabsAsAdded)
            {
                Manager.DeactivateGroup(group);
                Manager.ActivateGroup(group);
            }

        }

        if (groupObject.isPrefab)
        {
            Undo.RecordObject(Manager, "Update Parent Transform In Group");
            Transform oldTransformObject = groupObject.parentTransform;
            groupObject.parentTransform =
                EditorGUILayout.ObjectField(groupObject.parentTransform, typeof(Transform), true) as
                    Transform;
            if (oldTransformObject != groupObject.parentTransform)
            {

                if (!groupObject.parentTransform.IsChildOf(Manager.thisTransform))
                {
                    groupObject.parentTransform = oldTransformObject;
                    Debug.LogError("Error: Transform must be the parent transform or a child of " +
                                   Manager.thisTransform.name);
                }
                else
                {
                    Event e = Event.current;
                    if (e.shift)
                        UpdateAllTransforms(groupObject.parentTransform);
                }
            }
        }
    }

    private void ShowObjectDelete(PrefabGroup group, GroupObject groupObject)
    {
        Undo.RecordObject (Manager, "Remove Prefab Group GameObject");
        GUI.backgroundColor = redColor;
        if (GUILayout.Button(symbolX, GUILayout.Width(25)))
        {
            RemoveObject(group, groupObject);
            GUIUtility.ExitGUI();
        }
        GUI.backgroundColor = Color.white;
    }

    private void ShowObjectsButton(PrefabGroup group, bool header = false)
    {
        var fieldWidth = 80;
        if (header)
        { 
            EditorGUILayout.LabelField($"", GUILayout.Width(fieldWidth + 3));
            return;
            
        }
        Undo.RecordObject (Manager, "Toggle Wardrobe Group Show Objects");
        GUI.backgroundColor = group.showPrefabs ? activeColor : Color.white;
        if (GUILayout.Button("Objects (" + group.groupObjects.Count + ")", GUILayout.Width(fieldWidth)))
        {
            group.showPrefabs = !group.showPrefabs;
            group.showShapes = !group.showPrefabs && group.showShapes;
        }
        GUI.backgroundColor = Color.white;
    }
    
    private void ShowShapesButton(PrefabGroup group, bool header = false)
    {
        if (!BlendShapesManager || !WardrobePrefabManager) return;
        
        var fieldWidth = 80;
        if (header)
        { 
            EditorGUILayout.LabelField($"", GUILayout.Width(fieldWidth + 3));
            return;
            
        }
        Undo.RecordObject (Manager, "Toggle Wardrobe Group Show Shapes");
        GUI.backgroundColor = group.showShapes ? activeColor : Color.white;
        if (GUILayout.Button("Shapes",GUILayout.Width(fieldWidth)))
        {
            group.showShapes = !group.showShapes;
            group.showPrefabs = !group.showShapes && group.showPrefabs;
            
            
        }
        GUI.backgroundColor = Color.white;
    }

    private void ShowGroupActivateDeactivate(PrefabGroup group, bool header = false)
    {
        var fieldWidth = 160;
        if (header)
        { 
            EditorGUILayout.LabelField($"", GUILayout.Width(fieldWidth));
            return;
        }

        Undo.RecordObject (Manager, "Toggle Prefab Group Objects On");
        GUI.backgroundColor = Manager.GroupIsActive(group) == 2 ? inactiveColor2 : Color.white;
        if (GUILayout.Button("Activate", GUILayout.Width(80)))
            Manager.ActivateGroup(group);
        GUI.backgroundColor = Color.white;

        Undo.RecordObject (Manager, "Toggle Prefab Group Objects Off");
        GUI.backgroundColor = Manager.GroupIsActive(group) == 0 ? inactiveColor2 : Color.white;
        if (GUILayout.Button("Deactivate", GUILayout.Width(80)))
            Manager.DeactivateGroup(group);
        GUI.backgroundColor = Color.white;
    }

    private void ShowDefaultToggle(PrefabGroup group, bool header = false)
    {
        var fieldWidth = 50;
        if (header)
        { 
            EditorGUILayout.LabelField(new GUIContent($"Def. {symbolInfo}", $"Optional. Toggle one group to " +
                                                                               $"be the default group. When a group of this " +
                                                                               $"type is deactivated, the default group will " +
                                                                               $"automatically be activated.\n\nThis option is " +
                                                                               $"only available for groups with a \"Type\"."), GUILayout.Width(fieldWidth));
            return;
        }

        if (String.IsNullOrWhiteSpace(group.groupType))
        {
            group.isDefault = false;
            EditorGUILayout.LabelField($"", GUILayout.Width(fieldWidth));
            return;
        }

        var cacheToggle = group.isDefault;
        Undo.RecordObject (Manager, "Change Prefab Group Default");
        group.isDefault = EditorGUILayout.Toggle(group.isDefault, GUILayout.Width(fieldWidth));

        // Set all the other ones to false if this is now true
        if (cacheToggle != group.isDefault && group.isDefault)
        {
            foreach (var groupOfType in GroupsOfType(group.groupType))
            {
                if (groupOfType == group) continue;
                groupOfType.isDefault = false;
            }
        }
    }

    private void ShowGroupType(PrefabGroup group, bool header = false)
    {
        var fieldWidth = 100;
        if (header)
        { 
            EditorGUILayout.LabelField(new GUIContent($"Type {symbolInfo}", $"You can group Prefab Groups by type, making " +
                                                               $"it easier to have only one active at a time, or simply for " +
                                                               $"organization purposes."), GUILayout.Width(fieldWidth));
            return;
        }
        
        var cachedType = group.groupType;
        Undo.RecordObject (Manager, "Change Prefab Group Type");
        group.groupType = EditorGUILayout.DelayedTextField(group.groupType, GUILayout.Width(100));
        if (cachedType != group.groupType)
        {
            EnsureTypeNames();
            CacheTypes();
            EditorPrefs.SetBool($"Prefab Manager Show Type {group.groupType}", true);
        }
    }

    private void ShowGroupName(PrefabGroup group, bool header = false)
    {
        var fieldWidth = 180;
        if (header)
        { 
            EditorGUILayout.LabelField(new GUIContent($"Group Name {symbolInfo}", $"The name of the group must " +
                                                                                  $"be unique, and can be used to activate and " +
                                                                                  $"deactivate the group at runtime."), GUILayout.Width(fieldWidth));
            return;
        }
        
        var cachedName = group.name;
        Undo.RecordObject (Manager, "Change Prefab Group Name");
        group.name = EditorGUILayout.DelayedTextField(group.name, GUILayout.Width(fieldWidth));
        if (cachedName != group.name)
        {
            if (String.IsNullOrEmpty(group.name))
            {
                Debug.LogWarning("Error: Group names can not be empty.");
                group.name = cachedName;
            }
            if (Manager.prefabGroups.Count(x => x.name == group.name) > 1)
            {
                Debug.LogWarning("Error: Group names must be unique.");
                group.name = cachedName;
            }
        }
    }

    private void ShowRemovePrefabGroup(PrefabGroup group, bool header = false)
    {
        var fieldWidth = 25;
        if (header)
        { 
            EditorGUILayout.LabelField($"", GUILayout.Width(fieldWidth));
            return;
        }
        
        Undo.RecordObject (Manager, "Remove Prefab Group");
        GUI.backgroundColor = redColor;
        if (GUILayout.Button(symbolX, GUILayout.Width(fieldWidth)))
        {
            for (int i = 0; i < group.groupObjects.Count; i++)
                RemoveObject(group, group.groupObjects[i]);

            Manager.RemovePrefabGroup(group);
            CacheTypes();
            GUIUtility.ExitGUI();
        }
        GUI.backgroundColor = Color.white;
    }

    private void SectionButton(string button, string prefs, int width = -1)
    {
        GUI.backgroundColor = EditorPrefs.GetBool(prefs) ? Color.green : Color.black;
        if (GUILayout.Button(button))
            EditorPrefs.SetBool(prefs, !EditorPrefs.GetBool(prefs));
        GUI.backgroundColor = Color.white;
    }
    
    private void SetupAndOptions()
    {
        if (!EditorPrefs.GetBool("Prefab Manager Show Setup And Options")) return;
        
        EditorGUI.indentLevel++;
        EditorPrefs.SetBool("Prefab Manager Show Help Boxes", 
            EditorGUILayout.Toggle(new GUIContent($"Show Help Boxes {symbolInfo}", "Toggles help boxes in the Inspector"), 
                EditorPrefs.GetBool("Prefab Manager Show Help Boxes")));
        EditorPrefs.SetBool("Prefab Manager Show Full Inspector", 
            EditorGUILayout.Toggle(new GUIContent($"Show Full Inspector {symbolInfo}", "If true, will show the full default inspector at the bottom" +
                                                                           "of the window. Use for debugging, not for editing data!"), 
                EditorPrefs.GetBool("Prefab Manager Show Full Inspector")));
        Manager.instantiatePrefabsAsAdded =  
            EditorGUILayout.Toggle(new GUIContent($"Instantiate Prefabs when Added to Group {symbolInfo}", "If true, prefabs that are added to a group " +
                "will be instantiated into the scene."), Manager.instantiatePrefabsAsAdded);
        Manager.onlyOneGroupActivePerType =  
            EditorGUILayout.Toggle(new GUIContent($"Only one group active per type {symbolInfo}", "If true, only one group per named \"type\" can be active " +
                      "at a time, and any active group will be deactivated when a new one is " +
                      "activated. This means you only have to call the \"Activate\" method, and " +
                      "the rest is handled for you."), Manager.onlyOneGroupActivePerType);
        Manager.unpackPrefabs = 
            EditorGUILayout.Toggle(new GUIContent($"Unpack Prefabs when Instantiated {symbolInfo}", "If true, prefabs that are instantiated will be unpacked."), 
                Manager.unpackPrefabs);
        
        EditorGUILayout.Space();
        HelpBoxMessage("Use the option below to set all InGameObject values to null. This is useful " +
                                "if you've copied the component values from another character, to clean it up.");
        if (GUILayout.Button("Make all \"In Game Objects\" null"))
        {
            RemoveInGameObjectLinks();
        }
        
        HelpBoxMessage("If you've copied another objects data or added the component from another object " +
                       "as new to this object, use this option to relink all the available objects to the new object.\n\n " +
                       "HINT: If you hold shift while you replace the transform in the list, all transforms will be updated to " +
                       "the new selection.");
        if (GUILayout.Button("Relink objects to this parent object"))
        {
            RelinkObjects();
        }
        
        EditorGUI.indentLevel--;
        EditorUtility.SetDirty(this);
    }

    private void HelpBoxMessage(string message, MessageType messageType = MessageType.None)
    {
        if (!EditorPrefs.GetBool("Prefab Manager Show Help Boxes")) return;
        EditorGUILayout.HelpBox(message,messageType);
    }

    private void SetDefaultEditorPrefs()
    {
        DefaultEditorBool("Prefab Manager Show Help Boxes", true);
        DefaultEditorBool("Prefab Manager Show Full Inspector", false);
        DefaultEditorBool("Prefab Manager Instantiate When Added", true);
        DefaultEditorBool("Prefab Manager One Active Group Per Type", true);
        DefaultEditorBool("Prefab Manager Unpack Prefabs", true);
    }

    private void RemoveInGameObjectLinks()
    {
        foreach (PrefabGroup group in Manager.prefabGroups)
        {
            foreach (GroupObject obj in group.groupObjects)
            {
                obj.inGameObject = null;
            }
        }
    }

    private void UpdateAllObjects(GameObject oldObject, GameObject newObject)
    {
        foreach (PrefabGroup group in Manager.prefabGroups)
        {
            foreach (GroupObject obj in group.groupObjects)
            {
                if (obj.objectToHandle == oldObject)
                    obj.objectToHandle = newObject;
            }
        }
    }

    private void UpdateAllTransforms(Transform transform)
    {
        foreach (PrefabGroup group in Manager.prefabGroups)
        {
            foreach (GroupObject obj in group.groupObjects)
            {
                obj.parentTransform = transform;
            }
        }
    }
    
    private void RemoveObject(PrefabGroup prefabGroup, GroupObject groupObject)
    {
        GameObject inGameObject = groupObject.inGameObject;
        if (groupObject.isPrefab && inGameObject)
            Manager.DestroyObject(inGameObject);
        else if (!groupObject.isPrefab && inGameObject)
            inGameObject.SetActive(false);

        prefabGroup.RemoveGroupObject(groupObject);
    }

    private void CheckOptionsAreSet(int g, int i) => CheckOptionsAreSet(Manager.prefabGroups[g], Manager.prefabGroups[g].groupObjects[i]);

    private void CheckOptionsAreSet(PrefabGroup group, GroupObject groupObject)
    {
        if (groupObject.parentTransform == null)
            groupObject.parentTransform = Manager.thisTransform;
    }
    
    private void RelinkObjects()
    {
        Debug.Log("Begin Relink");
        for (int g = 0; g < Manager.prefabGroups.Count; g++)
        {
            PrefabGroup prefabGroup = Manager.prefabGroups[g];
            
            for (int i = 0; i < prefabGroup.groupObjects.Count; i++)
            {
                GroupObject groupObject = prefabGroup.groupObjects[i];

                // If this is a prefab, do a different operation
                if (groupObject.isPrefab)
                {
                    Debug.Log($"Prefab relink from {groupObject.parentTransform}");
                    GameObject foundObject = FindGameObject(groupObject.parentTransform.name);
                    if (foundObject == null) continue;
                    
                    Debug.Log("Found the object " + foundObject.name);
                    groupObject.parentTransform = foundObject.transform;
                    continue;
                }
                
                if (groupObject.objectToHandle.transform.IsChildOf(groupObject.parentTransform))
                {
                    var prefabName = groupObject.objectToHandle.name;
                    Debug.Log($"In-Game object relink for {prefabName}");
                    
                    GameObject foundObject = FindGameObject(prefabName);
                    if (foundObject == null) continue;
                    
                    Debug.Log("Found the object " + foundObject.name);
                    
                    groupObject.parentTransform = Manager.gameObject.transform;
                }
            }
        }
    }
    
    private GameObject FindGameObject(string lookupName)
    {
        if (Manager.gameObject.name == lookupName)
            return Manager.gameObject;
        
        Transform[] gameObjects = Manager.gameObject.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in gameObjects)
        {
            if (child.name == lookupName)
                return child.gameObject;
        }

        Debug.Log($"Warning: Did not find a child named {lookupName}! This re-link will be skipped.");
        return null;
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
