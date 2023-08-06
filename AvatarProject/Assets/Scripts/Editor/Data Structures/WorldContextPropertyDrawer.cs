using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using AvatarBA.AI.GOAP;

[CustomPropertyDrawer(typeof(WorldContext))]
public class WorldContextPropertyDrawer : GenericDictionaryPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => base.GetPropertyHeight(property, label);
}
