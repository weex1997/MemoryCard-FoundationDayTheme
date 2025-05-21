using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardManager_2))]
public class CardManagerEditor_2: Editor
{
    SerializedObject manager;
    SerializedProperty _pairAmount;
    SerializedProperty _width;
    SerializedProperty _height;
    SerializedProperty _spriteList;
    int spriteAmount;
    float w, h;

    private void OnEnable()
    {
        manager = new SerializedObject(target);
        _pairAmount = manager.FindProperty("pairAmount");
        _width = manager.FindProperty("width");
        _height = manager.FindProperty("height");
        _spriteList = manager.FindProperty("spriteList");
        spriteAmount = _spriteList.arraySize;
    }

    public override void OnInspectorGUI()
    {
        manager.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUI.enabled = false;
        EditorGUILayout.PropertyField(_pairAmount);
        GUI.enabled = true;
        EditorGUILayout.PropertyField(_width);
        EditorGUILayout.PropertyField(_height);

        // Calculating the pairs amount
        float tmp = _width.intValue * (float)_height.intValue / 2;
        _pairAmount.intValue = (int)System.Math.Ceiling(tmp);

        // Updating sprite amount in case of changes
        spriteAmount = _spriteList.arraySize;

        // Ensure the pairs amount does not exceed half the number of sprites (as each pair requires two distinct sprites)
        if (_pairAmount.intValue * 2 > spriteAmount)
        {
            EditorGUILayout.HelpBox("Too many Card Pairs for available sprites", MessageType.Error);
        }

        if (_width.intValue < 0)
        {
            _width.intValue = 0;
        }

        if (_height.intValue < 0)
        {
            _height.intValue = 0;
        }

        EditorGUILayout.EndVertical();

        manager.ApplyModifiedProperties();
        DrawDefaultInspector();
    }
}
