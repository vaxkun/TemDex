using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Pokemon : MonoBehaviour
{
    public const float PanelHeight = 100;

    public RectTransform RectTransform { get; private set; }

    [Header("Obj refs")]
    [SerializeField] private Text _pokeMaxCpText;
    [SerializeField] private Image _pokeImage;
    [SerializeField] private Text _pokeNameText;
    [SerializeField] private PokeStats _pokeStats;
    [SerializeField] private PokeTypeLabel[] _pokeTypeLabels;
    [SerializeField] private Button _button;
	[SerializeField] private Text _pokeAverage;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Set(TemInfo pokeInfo, UnityAction clickAction)
    {
		if(pokeInfo.Id < 200)
		{
			_pokeNameText.text =
			UIManager.GetFormattedText("  #" + pokeInfo.Id, TextColorType.White, false, 12) + " " +
			pokeInfo.temName
			;
		}
		else if(pokeInfo.Id >= 0)
		{
			_pokeNameText.text =
				UIManager.GetFormattedText("  #" + "TBA", TextColorType.White, false, 12) + " " +
				pokeInfo.temName;
		}
        
        
        _pokeMaxCpText.text =
            TextManager.GetText(TextType.Element, 0) +
            UIManager.NewLine(1) +
            UIManager.GetFormattedText(TextManager.GetNumericFormat((int) pokeInfo.MaxCp), TextColorType.LightOrange,
                true, 30);

		_pokeAverage.text =
			TextManager.GetText(TextType.Element, 63) +
			UIManager.NewLine(1) +
			UIManager.GetFormattedText(TextManager.GetNumericFormat((int)GetAverage(pokeInfo)), TextColorType.LightOrange, true, 45);

        //_pokeStats.SetStats(pokeInfo.AttackRate, pokeInfo.DefenseRate, pokeInfo.StaminaRate);

        _pokeImage.sprite = pokeInfo.Image;        

        foreach (var pokeTypeLabel in _pokeTypeLabels)
            pokeTypeLabel.gameObject.SetActive(false);
        for (int i = 0; i < pokeInfo.Type.Length; i++)
        {
            if (i < _pokeTypeLabels.Length)
            {
                _pokeTypeLabels[i].SetType(pokeInfo.Type[i]);
                _pokeTypeLabels[i].gameObject.SetActive(true);
            }                
        }

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(clickAction);
    }

	int GetAverage(TemInfo _pokeInfo)
	{
		float r = (_pokeInfo.BaseHp + _pokeInfo.BaseAttack + _pokeInfo.BaseAttackEsp + _pokeInfo.BaseDefense + _pokeInfo.BaseDefenseEsp + _pokeInfo.BaseSpeed) / 6;
		return (int)r;
	}

    public void EnableObj()
    {
        gameObject.SetActive(true);
    }

    public void DisableObj()
    {
        gameObject.SetActive(false);
    }
}
