using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PokeMovePage : PokePanel
{

    public UIState TargetState;

	[Header("Obj refs")]
    [SerializeField] private GameObject _parentPanel;
    [SerializeField] private Text _moveNameText;
    [SerializeField] private PokeTypeLabel _moveTypeLabel;
    [SerializeField] private PokeStats _moveStats;
    [SerializeField] private Text _moveStatsText;
    [SerializeField] private Text _otherStatsText;
    [SerializeField] private Text _otherStatsCountText;
    [SerializeField] private Image _chargeCount;
    [SerializeField] private TypeChartPanel _typeChart;

    public override void Awake()
    {
        base.Awake();

        switch (TargetState)
        {
            case UIState.QuickMovePage:
                _otherStatsText.text = TextManager.GetText(TextType.Element, 29) + ":";
                break;
            case UIState.ChargeMovePage:
                _otherStatsText.text =
                    TextManager.GetText(TextType.Element, 30) + ":" +
                    UIManager.NewLine(1) +
                    TextManager.GetText(TextType.Element, 31) + ":" +
                    UIManager.NewLine(1) +
                    TextManager.GetText(TextType.Element, 32) + ":";
                break;
        }
    }

    public void Set(PrimaryMove move)
    {
        _moveNameText.text = move.Name;
        _moveTypeLabel.SetType(move.Type);
        _moveStatsText.text =
            move.attack +
            UIManager.NewLine(1) +
            move.hold + " " + UIManager.GetFormattedText(TextManager.GetText(TextType.Element, 16), TextColorType.GreyLight, false, 12) +
            UIManager.NewLine(1) +
            move.staminaUsed.ToString("F") + UIManager.GetFormattedText(" / " + TextManager.GetText(TextType.Element, 16), TextColorType.GreyLight, false, 12);

    }

    public void Set(SecondaryMove move)
    {
        Set((PrimaryMove) move);

        _otherStatsCountText.text =
            (move.CritChance* 100) + UIManager.GetFormattedText(" %", TextColorType.GreyLight, false, 12) +
            UIManager.NewLine(1) +
            move.DidgeWindow + UIManager.GetFormattedText(" " + TextManager.GetText(TextType.Element, 16), TextColorType.GreyLight, false, 12) +
            UIManager.NewLine(1) +
            (100 / move.ChargeCount) + UIManager.GetFormattedText(" / 100 ", TextColorType.GreyLight, false, 12);

        SetChargeCount(move.ChargeCount);
    }

    public override void Show(YieldComplete onYieldComplete)
    {
        _parentPanel.SetActive(true);
        base.Show(onYieldComplete);
    }

    public override void Hide(YieldComplete onYieldComplete)
    {
        base.Hide(delegate
        {
            _parentPanel.SetActive(false);
            onYieldComplete();
        });
    }

    void SetChargeCount(int count)
    {
        if (!_chargeCount)
            return;

        _chargeCount.sprite = UIManager.Instance.UIRes.ChargeCountSprites[count - 1];
    }
}
