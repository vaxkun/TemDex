using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WelcomePage : PokePanel
{
    public const float TextStartPosY = 115;

	[Header("Obj refs")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _panelBase;
    [SerializeField] private Text _descriptionText;

    void Start()
    {
        StartCoroutine(StartYield());
    }

    IEnumerator StartYield()
    {
        string listFormattedDot = UIManager.GetFormattedText("* ", TextColorType.GreenSmooth, true, -1);

        _descriptionText.text =
            listFormattedDot + TextManager.GetText(TextType.Message, 4) +
            UIManager.NewLine(2) +
            listFormattedDot + TextManager.GetText(TextType.Message, 5) +
            UIManager.NewLine(2) +
            listFormattedDot + TextManager.GetText(TextType.Message, 6) +
            UIManager.NewLine(2) +
            listFormattedDot + TextManager.GetText(TextType.Message, 7) +
            UIManager.NewLine(2) +
            listFormattedDot + TextManager.GetText(TextType.Message, 8);

        yield return new WaitForEndOfFrame();

        float textContentSizeY = _descriptionText.rectTransform.sizeDelta.y;

        _panelBase.sizeDelta = new Vector2(_panelBase.sizeDelta.x, TextStartPosY + textContentSizeY + UIManager.PanelMinSpace);

        _content.sizeDelta = new Vector2(_content.sizeDelta.x, _panelBase.sizeDelta.y + UIManager.PanelBottomOffset * 2);
    }

    public void OnGoClick()
    {
        UIManager.Instance.HideWelcomePanel();
    }
}
