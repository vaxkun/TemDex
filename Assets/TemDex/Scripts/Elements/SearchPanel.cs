using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SearchPanel : PokePanel
{
    [Header("Obj refs")]
    [SerializeField] private InputField _inputField;
    [SerializeField] private Text _holderText;
    [SerializeField] private Text _inputText;
    [SerializeField] private Text _noMatchesText;

    [SerializeField] private ElementSelection[] _typeSelections;
    [SerializeField] private ElementSelection[] _eggSelections;
    

    private Color _noMatchesColor;
    private Color _noMatchesColorHide;

    public override void Awake()
    {
        base.Awake();

        _noMatchesColor = _noMatchesText.color;
        _noMatchesColorHide = _noMatchesColor;
        _noMatchesColorHide.a = 0;
    }

    void OnEnable()
    {
        StopCoroutine("AnimateNoMatchYield");

        _noMatchesText.gameObject.SetActive(false);
        _noMatchesText.color = _noMatchesColorHide;

        _holderText.text = 
            TextManager.GetText(TextType.Element, 20) + 
            AppManager.Instance.PokeData.PokeInfos[Random.Range(0, AppManager.Instance.PokeData.PokeInfos.Length)].temName;
    }

    void AnimateNoMatch(string reason)
    {
        if (!gameObject.activeSelf)
            return;

        _noMatchesText.text = reason;

        StopCoroutine("AnimateNoMatchYield");
        StartCoroutine("AnimateNoMatchYield");
    }

    public void UpdateAllSelections()
    {
		int length = Enum.GetNames(typeof(TemType)).Length;
		int length2 = Enum.GetNames(typeof(EggType)).Length;

		for (int i = 0; i < length; i++)
		{
			_typeSelections[i].Selected = AppManager.Instance.UserData.SearchTypeSelection[i];
		}
        for (int i = 0; i < length2; i++)
            _eggSelections[i].Selected = AppManager.Instance.UserData.SearchEggSelection[i];
    }

    public void OnEndEdit()
    {
        if (UIManager.Instance.EventSystem.alreadySelecting)
            return;

        #region Check Type selected
        bool anyTypeSelected = false;
        for (int i = 0; i < _typeSelections.Length; i++)
        {
            if (_typeSelections[i].Selected)
            {
                anyTypeSelected = true;
                break;
            }
        }

        if (!anyTypeSelected)
        {
            AnimateNoMatch(TextManager.GetText(TextType.Message, 0));
            return;
        }
        #endregion

        #region Check Egg selected

        bool anyEggSelected = false;
        for (int i = 0; i < _eggSelections.Length; i++)
        {
            if (_eggSelections[i].Selected)
            {
                anyEggSelected = true;
                break;
            }
        }

        if (!anyEggSelected)
        {
            AnimateNoMatch(TextManager.GetText(TextType.Message, 1));
            return;
        }
        #endregion

        TemInfo[] foundedPokeInfos = TrySearchPokeInfos(_inputText.text.ToLower());
        if (foundedPokeInfos != null)
        {
            UIManager.Instance.SetPokeList(foundedPokeInfos);
            UIManager.Instance.HideMenu();
            UIManager.Instance.SetListState(ListState.SearchResults);
            AppManager.RegisterUserEvent(UserEventType.SearchUsed);
            return;
        }
        AnimateNoMatch(TextManager.GetText(TextType.Element, 21));
    }

    public void OnClearClick()
    {
        _inputField.text = "";
    }

    public void OnSelectAllTypesClick()
    {
        for (int i = 0; i < _typeSelections.Length; i++)
        {
            _typeSelections[i].Selected = true;
        }
    }

    public void OnDeselectAllTypesClick()
    {
        for (int i = 0; i < _typeSelections.Length; i++)
        {
            _typeSelections[i].Selected = false;
        }
    }
    
    TemInfo[] TrySearchPokeInfos(string inputText)
    {
        List<TemInfo> foundedPokeInfos = new List<TemInfo>();

        #region Find by Name
        TemInfo[] allPokeInfos = AppManager.Instance.PokeData.PokeInfos;
        for (int i = 0; i < allPokeInfos.Length; i++)
        {
            if (allPokeInfos[i].temName.ToLower().Contains(inputText))
                foundedPokeInfos.Add(allPokeInfos[i]);
        }

        if (foundedPokeInfos.Count == 0)
            return null;

        #endregion

        #region Fin by Type
        List<TemInfo> withTypeList = new List<TemInfo>();

        List<TemType> selectedTypes = new List<TemType>();
        for (int i = 0; i < _typeSelections.Length; i++)
        {
            if (_typeSelections[i].Selected)
                selectedTypes.Add((TemType) _typeSelections[i].ElementId);
        }

        for (int i = 0; i < foundedPokeInfos.Count; i++)
        {
            for (int j = 0; j < selectedTypes.Count; j++)
            {
                if (foundedPokeInfos[i].Type.Contains(selectedTypes[j]))
                {
                    withTypeList.Add(foundedPokeInfos[i]);
                    break;
                }
            }
        }

        if (withTypeList.Count == 0)
            return null;

        #endregion

        #region Find by Egg
        List<TemInfo> withEggList = new List<TemInfo>();
        for (int i = 0; i < withTypeList.Count; i++)
        {
            if (_eggSelections[0].Selected)
            {
                if (withTypeList[i].EggDistanceType == (EggType) _eggSelections[0].ElementId)
                    withEggList.Add(withTypeList[i]);
            }
            if (_eggSelections[1].Selected && !withEggList.Contains(withTypeList[i]))
            {
                if (withTypeList[i].EggDistanceType == (EggType)_eggSelections[1].ElementId)
                    withEggList.Add(withTypeList[i]);
            }
            if (_eggSelections[2].Selected && !withEggList.Contains(withTypeList[i]))
            {
                if (withTypeList[i].EggDistanceType == (EggType)_eggSelections[2].ElementId)
                    withEggList.Add(withTypeList[i]);
            }
            if (_eggSelections[3].Selected && !withEggList.Contains(withTypeList[i]))
            {
                if (withTypeList[i].EggDistanceType == (EggType)_eggSelections[3].ElementId)
                    withEggList.Add(withTypeList[i]);
            }
        }
        if (withEggList.Count == 0)
            return null;
        #endregion

        return withEggList.ToArray();
    }

    IEnumerator AnimateNoMatchYield()
    {
        _noMatchesText.gameObject.SetActive(true);

        float lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime * UIManager.ShowHideTimeMulti;
            yield return new WaitForEndOfFrame();

            _noMatchesText.color = Color.Lerp(_noMatchesColorHide, _noMatchesColor, lt);
        }

        yield return new WaitForSeconds(3);

        lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime * UIManager.ShowHideTimeMulti * 0.25f;
            yield return new WaitForEndOfFrame();

            _noMatchesText.color = Color.Lerp(_noMatchesColor, _noMatchesColorHide, lt);
        }
    }


    public bool[] GetTypeSelectionValues()
    {
        bool[] typeSelVals = new bool[_typeSelections.Length];
        for (int i = 0; i < _typeSelections.Length; i++)
        {
            typeSelVals[i] = _typeSelections[i].Selected;
        }

        return typeSelVals;
    }

    public bool[] GetEggSelectionValues()
    {
        bool[] eggSelVals = new bool[_eggSelections.Length];
        for (int i = 0; i < _eggSelections.Length; i++)
        {
            eggSelVals[i] = _eggSelections[i].Selected;
        }

        return eggSelVals;
    }
}