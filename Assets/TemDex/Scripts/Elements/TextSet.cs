using UnityEngine;
using System.Collections;
using System.IO;

public enum TextType
{
    Element, Description, Message, LevelImageAnswer
}

[CreateAssetMenu(fileName = "TextSet", menuName = "Scriptable/Text set")]
public class TextSet : ScriptableObject
{
    [Header("Translation")]
    public AppLanguage Lang;

    [Space]
    public string[] Elements;
    public string[] Descriptions;
    public string[] Messages;
}

public enum AppLanguage
{
    en = 0, ru, de, zh , es
}


