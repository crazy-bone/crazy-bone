using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using InfinityPBR;

[CustomEditor(typeof(EquipmentObject))]
[Serializable]
public class EquipmentObjectEditor : Editor
{

    void OnEnable()
    {
        AutoPopulate((EquipmentObject) target);
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EquipmentObject equipmentObject = (EquipmentObject) target;

        ShowHelpBox();
        EditorGUILayout.Space();
        
        ShowFields(equipmentObject);
        
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(equipmentObject);
    }

    /*
     * Shows the two fields which need to be populated, with tooltips so fancy.
     */
    private void ShowFields(EquipmentObject equipmentObject)
    {
        ShowPopulateHint(equipmentObject);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"Skinned Mesh Renderer ⓘ", "This is the Skinned Mesh " +
                                                                              "Renderer of this object, and is generally " +
                                                                              "a child of the main parent object."), GUILayout.Width(150));
        equipmentObject.skinnedMeshRenderer = EditorGUILayout.ObjectField(equipmentObject.skinnedMeshRenderer,
            typeof(SkinnedMeshRenderer), true, GUILayout.Width(200)) as SkinnedMeshRenderer;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"Bone Root ⓘ", "This is the root object for the bone structure. " +
                                                                  "Remember the root bone structure must match the parent that " +
                                                                  "this is being attached to!"), GUILayout.Width(150));
        equipmentObject.boneRoot = EditorGUILayout.ObjectField(equipmentObject.boneRoot, 
            typeof(Transform), true, GUILayout.Width(200)) as Transform;
        EditorGUILayout.EndHorizontal();
    }

    /*
     * Shows a box if either of the two are NOT populated, so helpful.
     */
    private void ShowPopulateHint(EquipmentObject equipmentObject)
    {
        // March 19, 2022 -- When this is under a prefab, the set dirty doesn't work!
        //if (equipmentObject.skinnedMeshRenderer != null && equipmentObject.boneRoot != null) return;

        CheckRootBoneName();

        GUI.color = Color.yellow;
        EditorGUILayout.HelpBox($"These fields must be populated. Try opening the prefab to populate them. Note: " +
                                $"Unity bug, I think, when adding this script to a child of a prefab (in the \"Prefab\" management " +
                                $"view), turn off \"Auto Save\", and manually save after adding the script, or the settings won't " +
                                $"actually be saved!", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"Rootbone Name ⓘ", "Set the name of the root bone used for your character(s). The " +
                                                                      "\"Populate\" button will attempt to set the two required values."), GUILayout.Width(120));
        EditorPrefs.SetString("Infinity PBR Equipment Object Root Bone", EditorGUILayout.TextField(EditorPrefs.GetString("Infinity PBR Equipment Object Root Bone"), GUILayout.Width(100)));
        if (GUILayout.Button("Populate", GUILayout.Width(100)))
        {
            AutoPopulate(equipmentObject);
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
    }

    private void CheckRootBoneName()
    {
        if (EditorPrefs.HasKey("Infinity PBR Equipment Object Root Bone") 
            && !String.IsNullOrWhiteSpace(EditorPrefs.GetString("Infinity PBR Equipment Object Root Bone"))) return;

        EditorPrefs.SetString("Infinity PBR Equipment Object Root Bone", "BoneRoot");
    }

    private void AutoPopulate(EquipmentObject equipmentObject)
    {
        PopulateRootBone(equipmentObject);
        PopulateSkinnedMeshRenderer(equipmentObject);
    }

    private void PopulateSkinnedMeshRenderer(EquipmentObject equipmentObject)
    {
        foreach (Transform child in equipmentObject.gameObject.transform)
        {
            if (!child.TryGetComponent(out SkinnedMeshRenderer smr)) continue;
            equipmentObject.skinnedMeshRenderer = smr;
            return;
        }
    }

    private void PopulateRootBone(EquipmentObject equipmentObject)
    {
        foreach (Transform child in equipmentObject.gameObject.transform)
        {
            if (child.gameObject.name != EditorPrefs.GetString("Infinity PBR Equipment Object Root Bone")) continue;
            equipmentObject.boneRoot = child;
            return;
        }
    }

    /*
     * Main helpbox at the top explaining what this does and the two ways to utilize it.
     */
    private void ShowHelpBox()
    {
        EditorGUILayout.HelpBox($"EQUIPMENT OBJECT\n" +
                                        "This script implies that this object can be equipped onto another object, often a " +
                                        "character -- any object with a SkinnedMeshRenderer. During the equip process, the" +
                                        "object will be linked to the bones of the parent object, so that it can move with the " +
                                        "parent object animations.\n\n" +
                                        "1. [Edit time only] Add this object as a child of the target object (which will be " +
                                        "\"equipping\" this), and run the menu option Window/Infinity PBR/Equip Objects. This can " +
                                        "be done with any number of Equipment Objects at once.\n\n" +
                                        "2. [Runtime or Edit time] Use the \"Prefab Child Manager\" script to manage your equipment " +
                                        "objects. This script, when attached to the parent, makes it much easier to manage large " +
                                        "groups of equipment, including objects like this one, objects which do not have a " +
                                        "SkinnedMeshRenderer (i.e. weapons etc), and objects that are not prefabs at all, but are " +
                                        "already attached as a child of the parent object. This is the preferred method to use " +
                                        "at runtime for customization and randomization.", MessageType.None);
    }
}
