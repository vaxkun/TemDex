using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class GetText : MonoBehaviour
{
    [SerializeField]
    public TextType TextType;
    [SerializeField]
    public int TextId;

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        GetComponent<Text>().text = TextManager.GetText(TextType, TextId);
    }
}

