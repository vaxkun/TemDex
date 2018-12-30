using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TypeChartPanel : MonoBehaviour
{
    public const float LabelStartPosY = 30;
    public const float LabelSpaceY = 5;

    public GameObject TypeLabelTemplate;

    [Header("Obj refs")]
    [SerializeField] private RectTransform _strenghtsRect;
    [SerializeField] private RectTransform _weaksRect;

    [SerializeField] private Text _strenghtText;
    [SerializeField] private Text _weaksText;
	[SerializeField] private Text _DealDoubleDamageText;
	[SerializeField] private Text _DealHalfDamageText;
	[SerializeField] private Text _RecieveDoubleDamageText;
	[SerializeField] private Text _RecieveHalfDamageText;

	private RectTransform _rectTransform;

    private float _labelHeightY;
    private List<GameObject> _curLabelsObj = new List<GameObject>(); 

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _labelHeightY = TypeLabelTemplate.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void Set(TypeChartPanelType panelType, List<TemType> strengths, List<TemType> weaks,List<TemType> dealsDouble,List<TemType> recieveHalf,List<TemType> dealsHalf,List<TemType> recieveDouble)
    {
        for (int i = 0; i < _curLabelsObj.Count; i++)
            Destroy(_curLabelsObj[i]);
        _curLabelsObj = new List<GameObject>();

        //Set rect
        int maxLabels = Mathf.Max(strengths.Count, weaks.Count)+1+Mathf.Max(dealsDouble.Count,dealsHalf.Count)+1+Mathf.Max(recieveHalf.Count,recieveDouble.Count);
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, LabelStartPosY + ((_labelHeightY + LabelSpaceY) * maxLabels));

        switch (panelType)
        {
            case TypeChartPanelType.Pokemon:
                _strenghtText.text = TextManager.GetText(TextType.Element, 42);
                _weaksText.text = TextManager.GetText(TextType.Element, 43);
				_DealDoubleDamageText.text = TextManager.GetText(TextType.Element, 52);
				_RecieveHalfDamageText.text = TextManager.GetText(TextType.Element, 53);
				_DealHalfDamageText.text = TextManager.GetText(TextType.Element, 54);
				_RecieveDoubleDamageText.text = TextManager.GetText(TextType.Element, 55);
				break;
            case TypeChartPanelType.Move:
                _strenghtText.Rebuild(CanvasUpdate.PreRender);
                _strenghtText.text = TextManager.GetText(TextType.Element, 44);
                _weaksText.text = TextManager.GetText(TextType.Element, 45);
                break;
        }

        //Create labels
		//Resistances
        for (int i = 0; i < strengths.Count; i++)
        {
            RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
            labelRect.SetParent(_strenghtsRect);
            labelRect.localScale = Vector3.one;
            labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * i);

            labelRect.GetComponent<PokeTypeLabel>().SetType(strengths[i]);

            _curLabelsObj.Add(labelRect.gameObject);
        }
		//DealsDouble
		_DealDoubleDamageText.rectTransform.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * strengths.Count+1);
		for (int i = 0; i < dealsDouble.Count; i++)
		{
			RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
			labelRect.SetParent(_strenghtsRect);
			labelRect.localScale = Vector3.one;
			labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * (i+ strengths.Count+1));

			labelRect.GetComponent<PokeTypeLabel>().SetType(dealsDouble[i]);

			_curLabelsObj.Add(labelRect.gameObject);
		}
		//RecieveHalf
		_RecieveHalfDamageText.rectTransform.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * (strengths.Count +dealsDouble.Count+1));
		for (int i = 0; i < recieveHalf.Count; i++)
		{
			RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
			labelRect.SetParent(_strenghtsRect);
			labelRect.localScale = Vector3.one;
			labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * (i + strengths.Count + 1+ dealsDouble.Count+1));

			labelRect.GetComponent<PokeTypeLabel>().SetType(recieveHalf[i]);

			_curLabelsObj.Add(labelRect.gameObject);
		}


		for (int i = 0; i < weaks.Count; i++)
        {
            RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
            labelRect.SetParent(_weaksRect);
            labelRect.localScale = Vector3.one;
            labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * i);

            labelRect.GetComponent<PokeTypeLabel>().SetType(weaks[i]);

            _curLabelsObj.Add(labelRect.gameObject);
        }
		//DealsHalf
		_DealHalfDamageText.rectTransform.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * weaks.Count + 1);
		for (int i = 0; i < dealsHalf.Count; i++)
		{
			RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
			labelRect.SetParent(_weaksRect);
			labelRect.localScale = Vector3.one;
			labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * (i + weaks.Count + 1));

			labelRect.GetComponent<PokeTypeLabel>().SetType(dealsHalf[i]);

			_curLabelsObj.Add(labelRect.gameObject);
		}
		//RecieveDouble
		_RecieveDoubleDamageText.rectTransform.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * (weaks.Count + dealsHalf.Count + 1));
		for (int i = 0; i < recieveDouble.Count; i++)
		{
			RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
			labelRect.SetParent(_weaksRect);
			labelRect.localScale = Vector3.one;
			labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * (i + weaks.Count + 1 + dealsHalf.Count + 1));

			labelRect.GetComponent<PokeTypeLabel>().SetType(recieveDouble[i]);

			_curLabelsObj.Add(labelRect.gameObject);
		}

	}
	public void Set(TypeChartPanelType panelType, TemType[] strenghts, TemType[] weaks)
	{
		for (int i = 0; i < _curLabelsObj.Count; i++)
			Destroy(_curLabelsObj[i]);
		_curLabelsObj = new List<GameObject>();

		//Set rect
		int maxLabels = Mathf.Max(strenghts.Length, weaks.Length);
		_rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, LabelStartPosY + ((_labelHeightY + LabelSpaceY) * maxLabels));

		switch (panelType)
		{
			case TypeChartPanelType.Pokemon:
				_strenghtText.text = TextManager.GetText(TextType.Element, 42);
				_weaksText.text = TextManager.GetText(TextType.Element, 43);
				
				break;
			case TypeChartPanelType.Move:
				_strenghtText.Rebuild(CanvasUpdate.PreRender);
				_strenghtText.text = TextManager.GetText(TextType.Element, 44);
				_weaksText.text = TextManager.GetText(TextType.Element, 45);
				break;
		}

		//Create labels
		for (int i = 0; i < strenghts.Length; i++)
		{
			RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
			labelRect.SetParent(_strenghtsRect);
			labelRect.localScale = Vector3.one;
			labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * i);

			labelRect.GetComponent<PokeTypeLabel>().SetType(strenghts[i]);

			_curLabelsObj.Add(labelRect.gameObject);
		}

		for (int i = 0; i < weaks.Length; i++)
		{
			RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
			labelRect.SetParent(_weaksRect);
			labelRect.localScale = Vector3.one;
			labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * i);

			labelRect.GetComponent<PokeTypeLabel>().SetType(weaks[i]);

			_curLabelsObj.Add(labelRect.gameObject);
		}

	}
}

[Serializable]
public enum TypeChartPanelType
{
    Pokemon = 0, Move
}
