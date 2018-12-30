using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(GetText))]
public class GetTextEditor : Editor
{
    private GetText _getText;

    void OnEnable()
    {
        _getText = (GetText)target;
    }

    public override void OnInspectorGUI()
    {
        _getText.TextType = (TextType)EditorGUILayout.EnumPopup("Text Type", _getText.TextType);
        _getText.TextId = Mathf.Clamp(EditorGUILayout.IntField("Text ID", _getText.TextId), 0, 100500);

        EditorGUILayout.LabelField(_getText.GetComponent<Text>().text);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            _getText.UpdateText();
        }
    }
}

