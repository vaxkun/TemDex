using System;
using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject tmObject = (GameObject)Resources.Load("TextManager");
                _instance = tmObject.GetComponent<TextManager>();
				Debug.Log(Application.systemLanguage);
				if(Application.systemLanguage == SystemLanguage.Spanish)
				{
					_instance.CurTextSet = _instance.TextSets[1];
				}
				else
				{
					_instance.CurTextSet = _instance.TextSets[0];
				}
                
            }
            return _instance;
        }
    }
    private static TextManager _instance;

    public TextSet[] TextSets;

    public int CurTextSetId;
    public TextSet CurTextSet { get; private set; }

    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;
    }

	void SetNewTextSet(int id)
	{
		_instance.CurTextSetId = id;
		_instance.CurTextSet = _instance.TextSets[_instance.CurTextSetId];
	}
    public static TextManager Load()
    {
        return Instance;
    }

    public static TextSet GetCurTextSet()
    {
        return Instance.CurTextSet;
    }

    public static string GetText(TextType textType, int textId)
    {
        switch (textType)
        {
            case TextType.Element:
                return Instance.CurTextSet.Elements[Mathf.Clamp(textId, 0, Instance.CurTextSet.Elements.Length - 1)];

            case TextType.Description:
                return Instance.CurTextSet.Descriptions[Mathf.Clamp(textId, 0, Instance.CurTextSet.Descriptions.Length - 1)];

            case TextType.Message:
                return Instance.CurTextSet.Messages[Mathf.Clamp(textId, 0, Instance.CurTextSet.Messages.Length - 1)];
        }

        return "Missing text";
    }

    public static string GetNumericFormat(int value)
    {
        return string.Format("{0:n0}", value).Replace(',', ' ');
    }

    private static void SetCurTextSet(AppLanguage textSetLang)
    {
        for (int i = 0; i < Instance.TextSets.Length; i++)
        {
            if (Instance.TextSets[i].Lang == textSetLang)
            {
                Instance.CurTextSet = Instance.TextSets[i];
                Instance.CurTextSetId = i;
                return;
            }
        }

        Instance.CurTextSet = Instance.TextSets[0];
    }
}

