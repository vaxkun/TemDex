using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(SwipeTrigger))]
public class PokemonList : PokePanel
{
    public const int MaxPokeInfoPerPage = 25;

    public GameObject PokemonTemplate;

    [Header("Obj refs")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private Button _prevPageBtn;
    [SerializeField] private Button _nextPageBtn;

    //Process
    public int CurPageId { get; private set; }
    public SortMethod CurSortMethod { get; private set; }
    public SortOrder CurSortOrder { get; private set; }

    private ScrollRect _scrollRect;
    private TemInfo[] _curPokeInfos;
    private Pokemon[] _pokemons = new Pokemon[MaxPokeInfoPerPage];
    private List<PokeListPage> _pokeInfoPages = new List<PokeListPage>();

    public override void Awake()
    {
        base.Awake();

        _scrollRect = GetComponent<ScrollRect>();
    }

    public void Init()
    {
        for (int i = 0; i < _pokemons.Length; i++)
        {
            _pokemons[i] = Instantiate(PokemonTemplate).GetComponent<Pokemon>();
            _pokemons[i].gameObject.name = PokemonTemplate.name + "_" + i;
            _pokemons[i].DisableObj();
        }

        GetComponent<SwipeTrigger>().OnSwipeLeft += OnNextPageClick;
        GetComponent<SwipeTrigger>().OnSwipeRight += OnPrevPageClick;
    }

    public void SetList(TemInfo[] pokeInfos)
    {
        _pokeInfoPages.Clear();
        _pokeInfoPages.Add(new PokeListPage());

        _curPokeInfos = pokeInfos;

        CurPageId = 0;

        int curItemId = 0;
        for (int i = 0; i < pokeInfos.Length; i++)
        {
            if (curItemId >= MaxPokeInfoPerPage)
            {
                _pokeInfoPages.Add(new PokeListPage());
                curItemId = 0;
            }

            _pokeInfoPages[_pokeInfoPages.Count - 1].PokeInfos.Add(pokeInfos[i]);

            curItemId++;
        }   
    }

    public void SetSortMethod(SortMethod sortMethod, SortOrder sortOrder)
    {
        AppManager.Instance.UserData.UsingSortMethod = CurSortMethod = sortMethod;
        AppManager.Instance.UserData.UsingSortOrder = CurSortOrder = sortOrder;
    }

    public void SortList()
    {
        List<TemInfo> pokeInfos = _curPokeInfos.ToList();

        switch (CurSortMethod)
        {
            case SortMethod.ById:
                if (CurSortOrder == SortOrder.Inc)
                    SetList(pokeInfos.OrderBy(info => info.Id).ToArray());
                else SetList(pokeInfos.OrderByDescending(info => info.Id).ToArray());
                break;
            case SortMethod.ByName:
                if (CurSortOrder == SortOrder.Inc)
                    SetList(pokeInfos.OrderBy(info => info.temName).ToArray());
                else SetList(pokeInfos.OrderByDescending(info => info.temName).ToArray());
                break;
            case SortMethod.ByMaxCp:
                if (CurSortOrder == SortOrder.Inc)
                    SetList(pokeInfos.OrderBy(info => info.MaxCp).ToArray());
                else SetList(pokeInfos.OrderByDescending(info => info.MaxCp).ToArray());
                break;
            case SortMethod.ByBaseAttack:
                if (CurSortOrder == SortOrder.Inc)
                    SetList(pokeInfos.OrderBy(info => info.BaseAttack).ToArray());
                else SetList(pokeInfos.OrderByDescending(info => info.BaseAttack).ToArray());
                break;
            case SortMethod.ByBaseDefense:
                if (CurSortOrder == SortOrder.Inc)
                    SetList(pokeInfos.OrderBy(info => info.BaseDefense).ToArray());
                else SetList(pokeInfos.OrderByDescending(info => info.BaseDefense).ToArray());
                break;
            case SortMethod.ByBaseHp:
                if (CurSortOrder == SortOrder.Inc)
                    SetList(pokeInfos.OrderBy(info => info.BaseStamina).ToArray());
                else SetList(pokeInfos.OrderByDescending(info => info.BaseStamina).ToArray());
                break;
        }
    }

    public void FillPage()
    {
        FillPage(CurPageId);
    }

    public void FillPage(int pageId)
    {
        TemInfo[] pokeInfos = _pokeInfoPages[pageId].PokeInfos.ToArray();
        
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, pokeInfos.Length * Pokemon.PanelHeight + UIManager.PanelBottomOffset);
        _scrollbar.value = 1;
        

        for (int i = 0; i < _pokemons.Length; i++)
            _pokemons[i].DisableObj();

        for (int i = 0; i < pokeInfos.Length; i++)
        {
            Pokemon pokemon = _pokemons[i];
            
            pokemon.RectTransform.SetParent(_content);
            pokemon.RectTransform.anchoredPosition = new Vector2(0, (-Pokemon.PanelHeight * i) - ListStateLine.PanelHeight);
            pokemon.RectTransform.sizeDelta = new Vector2(0, Pokemon.PanelHeight);
            pokemon.RectTransform.localScale = Vector3.one;

            int id = i;
            pokemon.Set(pokeInfos[i], () => OnPokemonClick(pokeInfos[id].Id));

            _pokemons[i].EnableObj();
        }

        UIManager.Instance.SetListPage(pageId + 1, _pokeInfoPages.Count, _curPokeInfos.Length);

        UpdateNaviButtons();
    }
	int GetID(int id)
	{
		int r = 0;
		AppManager temp = AppManager.Instance;
		int length = temp.PokeData.PokeInfos.Length;
		for (int i = 0; i < length; i++)
		{
			if (temp.PokeData.PokeInfos[i].Id == id)
			{
				r = i;
				break;
			}
		}
		return r;
	}

	void UpdateNaviButtons()
    {
        if (CurPageId == 0)
            _prevPageBtn.gameObject.SetActive(false);
        else _prevPageBtn.gameObject.SetActive(true);

        if (CurPageId == _pokeInfoPages.Count - 1)
            _nextPageBtn.gameObject.SetActive(false);
        else _nextPageBtn.gameObject.SetActive(true);
    }

    public void AnimateTransition(int toPageId, bool toRight)
    {
        if (UIManager.Instance.State != UIState.PokemonList)
            return;
        UIManager.SetState(UIState.BlockInput);

        StartCoroutine(AnimateTransitionYield(toPageId, toRight, () => UIManager.SetState(UIState.PokemonList)));
    }

    IEnumerator AnimateTransitionYield(int toPageId, bool toRight, YieldComplete onYieldComplete)
    {
        float canvasSizeXFrom = 0;
        float canvasSizeXTo = UIManager.GetCanvasSize().x;
        if (toRight)
            canvasSizeXTo = -canvasSizeXTo;

        float lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime*UIManager.ShowHideTimeMulti * 2;

            _content.anchoredPosition = new Vector2(Mathf.SmoothStep(canvasSizeXFrom, canvasSizeXTo, lt), _content.anchoredPosition.y);
            yield return new WaitForEndOfFrame();
        }

        FillPage(toPageId);

        lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime * UIManager.ShowHideTimeMulti * 2;

            _content.anchoredPosition = new Vector2(Mathf.SmoothStep(-canvasSizeXTo, canvasSizeXFrom, lt), _content.anchoredPosition.y);
            yield return new WaitForEndOfFrame();
        }

        onYieldComplete();
    }

    public void OnMenuClick()
    {
        UIManager.Instance.ShowMenu();
    }

    public void OnPrevPageClick()
    {
        if (UIManager.Instance.State == UIState.BlockInput)
            return;

        if (CurPageId >= 1)
        {
            _scrollRect.StopMovement();
            CurPageId--;
            AnimateTransition(CurPageId, false);
            UpdateNaviButtons();
        }
    }

    public void OnNextPageClick()
    {
        if (UIManager.Instance.State == UIState.BlockInput)
            return;

        if (CurPageId < _pokeInfoPages.Count - 1)
        {
            _scrollRect.StopMovement();
            CurPageId++;
            AnimateTransition(CurPageId, true);
            UpdateNaviButtons();
        }
    }

    public void OnPokemonClick(int id)
    {
        if (UIManager.Instance.State == UIState.BlockInput)
            return;

        UIManager.Instance.ShowPokemonPage(id);
    }
   
}

[Serializable]
public class PokeListPage
{
    public List<TemInfo> PokeInfos;

    public PokeListPage()
    {
        PokeInfos = new List<TemInfo>();
    }
}
