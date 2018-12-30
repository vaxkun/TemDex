using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(SwipeTrigger))]
public class PokemonPage : MonoBehaviour
{ 
    public const float MoveStartHeightPos = 30;
    public const float PanelSpace = 5;
    public const float FullScreenPanelCloseHeight = 100;

	int[] maxStatsTemp = new int[7];


	public GameObject QuickMoveTemplate;
    public GameObject ChargeMoveTemplate;

	[Header("Obj refs")]
    #region Obj refs
    [SerializeField] private PokeImageLabel _baseImageLabel;
    [SerializeField] private PokeImageLabel _prevImageLabel;
    [SerializeField] private PokeImageLabel _nextImageLabel;

    [SerializeField] private Text _idText;
    [SerializeField] private PokeTypeLabel[] _pokeTypeLabels;
    [SerializeField] private Text _classText;
    [SerializeField] private Text _maxCpWHText;
    [SerializeField] private PokeStats _pokeStats;
    [SerializeField] private TypeChartPanel _typeChart;
    [SerializeField] private Text _statsText;
    [SerializeField] private Image _eggImage;
    [SerializeField] private Text _eggDistanceText;
    [SerializeField] private Text _addInfoText;
    [SerializeField] private Text _addInfoCountText;
    [SerializeField] private ElementSelection _favoriteButton;

    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform[] _allPanels;
	[SerializeField] private UIPolygon[] baseStats = new UIPolygon[2];
	[SerializeField] private Text[] baseStatsTXT = new Text[7];
	[SerializeField] private UIPolygon[] hepta = new UIPolygon[3];
#endregion

    //Process
    private TemInfo _curPokeInfo;
    private GameObject[] _curQuickMovesObj = new GameObject[0];
    private GameObject[] _curChargeMovesObj = new GameObject[0];

    private CanvasGroup _canvasGroup;
    private ScrollRect _scrollRect;
    private float _lastScrollPos;
	bool firstOpening = true;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _scrollRect = GetComponent<ScrollRect>();
        _lastScrollPos = _scrollRect.verticalNormalizedPosition;

        GetComponent<SwipeTrigger>().OnSwipeRight += OnPrevPokeClick;
        GetComponent<SwipeTrigger>().OnSwipeLeft += OnNextPokeClick;

        _favoriteButton.OnElementSelected += delegate(bool selected)
        {
            if (_curPokeInfo == null)
                return;

            if (selected)
                AppManager.Instance.UserData.AddFavoritePokemon(_curPokeInfo.Id);
            else AppManager.Instance.UserData.RemoveFavoritePokemon(_curPokeInfo.Id);

        };
		maxStatsTemp = AppManager.Instance.PokeData.maxStats;
	}

    void Update()
    {
        HandleDrag();
    }

    void HandleDrag()
    {
        if ((_content.anchoredPosition.y < -FullScreenPanelCloseHeight) || (_content.sizeDelta.y - UIManager.GetCanvasSize().y - _content.anchoredPosition.y) < -FullScreenPanelCloseHeight)
            UIManager.Instance.ClosePokemonPage();
    }

    public void Set(TemInfo pokeInfo)
    {
        gameObject.SetActive(true);
        StartCoroutine(SetYield(pokeInfo));
    }

    IEnumerator SetYield(TemInfo pokeInfo)
    {
        _curPokeInfo = pokeInfo;

        foreach (var o in _curQuickMovesObj)
            Destroy(o);
        foreach (var o in _curChargeMovesObj)
            Destroy(o);

        float nextPanelPosY = 0;

        #region Set labels (0 PANEL)
		if(pokeInfo.Id < 200)
		{
			_idText.text = "#" + pokeInfo.Id;
		}
		else if(pokeInfo.Id >= 200)
		{
			_idText.text = "#" + "TBA";
		}

        _baseImageLabel.Set(pokeInfo.Image, pokeInfo.temName);

        if (pokeInfo.EvoFromId > -1)
        {
            TemInfo evoFrom = AppManager.Instance.PokeData.PokeInfos[GetID(pokeInfo.EvoFromId)];
            _prevImageLabel.Set(evoFrom.Image, evoFrom.temName, evoFrom.LvlToEvolve);
            _prevImageLabel.EnableObj();
        }
        else _prevImageLabel.DisableObj();

        if (pokeInfo.EvoToId > -1)
        {
            TemInfo evoTo = AppManager.Instance.PokeData.PokeInfos[GetID(pokeInfo.EvoToId)];
            _nextImageLabel.Set(evoTo.Image, evoTo.temName, pokeInfo.LvlToEvolve);
            _nextImageLabel.EnableObj();
        }
        else _nextImageLabel.DisableObj();

        if (AppManager.Instance.UserData.FavoritePokemons.Contains(pokeInfo.Id))
            _favoriteButton.Selected = true;
        else _favoriteButton.Selected = false;

        nextPanelPosY -= _allPanels[0].sizeDelta.y + PanelSpace;
        #endregion

        #region Set info (1 PANEL)
        foreach (var pokeTypeLabel in _pokeTypeLabels)
            pokeTypeLabel.DisableObj();
        for (int i = 0; i < pokeInfo.Type.Length; i++)
        {
            _pokeTypeLabels[i].SetType(pokeInfo.Type[i]);
            _pokeTypeLabels[i].EnableObj();
        }

        if (pokeInfo.Class == PokeClass.Mythical)
            _classText.text = pokeInfo.Class.ToString().ToUpper();
        else _classText.text = "";

        _maxCpWHText.text =
            TextManager.GetText(TextType.Element, 0) +
            UIManager.NewLine(1) +
            UIManager.GetFormattedText(TextManager.GetNumericFormat((int)pokeInfo.MaxCp), TextColorType.LightBlue, true, 30) +
            UIManager.NewLine(1) +
            TextManager.GetText(TextType.Element, 2) + ": " + UIManager.GetFormattedText(pokeInfo.Weight.ToString(), TextColorType.LightBlue, true, 16) +
            " " + UIManager.GetFormattedText(TextManager.GetText(TextType.Element, 17), TextColorType.White, true, 12) +
            UIManager.NewLine(1) +
            TextManager.GetText(TextType.Element, 3) + ": " + UIManager.GetFormattedText(pokeInfo.Height.ToString(), TextColorType.LightBlue, true, 16) +
            " " + UIManager.GetFormattedText(TextManager.GetText(TextType.Element, 18), TextColorType.White, true, 12);

        //_pokeStats.SetStats(pokeInfo.AttackRate, pokeInfo.DefenseRate, pokeInfo.StaminaRate);
        _statsText.text =
            pokeInfo.BaseAttack +
            UIManager.NewLine(1) +
            pokeInfo.BaseDefense +
            UIManager.NewLine(1) +
            pokeInfo.BaseStamina;

		//HEPTAGON
		//HP, AT, AT.ESP, DEF, DEF.ESP, SPEED, STAM
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				switch (j)
				{
					case 0:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseHp, j);
						baseStatsTXT[j].text = TextManager.GetText(TextType.Element, 56) + UIManager.NewLine(1) + pokeInfo.BaseHp;
						break;
					case 1:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseAttack, j);
						baseStatsTXT[j].text = TextManager.GetText(TextType.Element, 57) + UIManager.NewLine(1) + pokeInfo.BaseAttack;
						break;
					case 2:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseAttackEsp, j);
						baseStatsTXT[j].text = TextManager.GetText(TextType.Element, 58) + UIManager.NewLine(1) + pokeInfo.BaseAttackEsp;
						break;
					case 3:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseDefense, j);
						baseStatsTXT[j].text = pokeInfo.BaseDefense + UIManager.NewLine(1) + TextManager.GetText(TextType.Element, 59);
						break;
					case 4:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseDefenseEsp, j);
						baseStatsTXT[j].text = pokeInfo.BaseDefenseEsp + UIManager.NewLine(1) + TextManager.GetText(TextType.Element, 60);
						break;
					case 5:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseSpeed, j);
						baseStatsTXT[j].text = TextManager.GetText(TextType.Element, 61) + UIManager.NewLine(1) + pokeInfo.BaseSpeed;
						break;
					case 6:
						baseStats[i].VerticesDistances[j] = CalculateHeptagonDistance(pokeInfo.BaseStamina, j);
						baseStatsTXT[j].text = TextManager.GetText(TextType.Element, 62) + UIManager.NewLine(1) + pokeInfo.BaseStamina;
						break;
				}
			}
		}


        if (pokeInfo.EggDistanceType != EggType.None)
        {
            _eggDistanceText.text = ((int)pokeInfo.EggDistanceType) + TextManager.GetText(TextType.Element, 23);
            _eggDistanceText.color = UIManager.TextColorsList[TextColorType.LightBlue];
            _eggImage.sprite = UIManager.Instance.UIRes.EggSprites[0];
        }
        else
        {
            _eggDistanceText.text = TextManager.GetText(TextType.Element, 24);
            _eggDistanceText.color = UIManager.TextColorsList[TextColorType.LightBlue];
            _eggImage.sprite = UIManager.Instance.UIRes.EggSprites[1];
        }

        nextPanelPosY -= _allPanels[1].sizeDelta.y + (PanelSpace * 2);
		#endregion

		#region Set Resistance & Weaknesses (4 PANEL)
		TypeController TC = TypeController.instance;

		_typeChart.Set(TypeChartPanelType.Pokemon, TC.GiveStrongTemType(_curPokeInfo), TC.GiveWeakType(_curPokeInfo),TC.GiveAttack2x(_curPokeInfo),TC.GiveDefense05x(_curPokeInfo),
			TC.GiveAttack05x(_curPokeInfo),TC.GiveDefense2x(_curPokeInfo));

        yield return new WaitForEndOfFrame();

        _allPanels[4].anchoredPosition = new Vector2(_allPanels[4].anchoredPosition.x, nextPanelPosY);
        nextPanelPosY -= _allPanels[4].sizeDelta.y + PanelSpace;
        #endregion

        #region Set moves (2 PANEL)
        int movesCount = Mathf.Max(pokeInfo.PrimaryMovesIds.Length, pokeInfo.SecondaryMovesIds.Length);
        float movePanelHeight = movesCount * (PokeMove.PanelHeight + (PanelSpace));

        _allPanels[2].sizeDelta = new Vector2(_allPanels[2].sizeDelta.x, movePanelHeight + MoveStartHeightPos);
        _allPanels[2].anchoredPosition = new Vector2(_allPanels[2].anchoredPosition.x, nextPanelPosY);

        _curQuickMovesObj = new GameObject[pokeInfo.PrimaryMovesIds.Length];
        for (int i = 0; i < pokeInfo.PrimaryMovesIds.Length; i++)
        {
            _curQuickMovesObj[i] = Instantiate(QuickMoveTemplate);
            PokeMove move = _curQuickMovesObj[i].GetComponent<PokeMove>();

            move.RectTransform.SetParent(_allPanels[2]);
            move.RectTransform.anchoredPosition = new Vector2(0, -(MoveStartHeightPos + PokeMove.PanelHeight * i) - (PanelSpace * i));
            move.RectTransform.sizeDelta = new Vector2(0, move.RectTransform.sizeDelta.y);
            move.RectTransform.localScale = Vector3.one;

            move.Set(AppManager.Instance.PokeData.PrimaryMoves[pokeInfo.PrimaryMovesIds[i]]);
        }

        _curChargeMovesObj = new GameObject[pokeInfo.SecondaryMovesIds.Length];
        for (int i = 0; i < pokeInfo.SecondaryMovesIds.Length; i++)
        {
            _curChargeMovesObj[i] = Instantiate(ChargeMoveTemplate);
            PokeMove move = _curChargeMovesObj[i].GetComponent<PokeMove>();

            move.RectTransform.SetParent(_allPanels[2]);
            move.RectTransform.anchoredPosition = new Vector2(0, -(MoveStartHeightPos + PokeMove.PanelHeight * i) - (PanelSpace * i));
            move.RectTransform.sizeDelta = new Vector2(0, move.RectTransform.sizeDelta.y);
            move.RectTransform.localScale = Vector3.one;

            move.Set(AppManager.Instance.PokeData.SecondaryMoves[pokeInfo.SecondaryMovesIds[i]]);
        }

        nextPanelPosY -= _allPanels[2].sizeDelta.y + PanelSpace;
        #endregion

        #region Set Additional Info (3 PANEL)
        //Fill INFO
        string percentFormatted = UIManager.GetFormattedText(" %", TextColorType.GreyLight, false, 12);

        _addInfoText.text =
            TextManager.GetText(TextType.Element, 33) + ":" +
            UIManager.NewLine(1);

        if (_curPokeInfo.CpMultiFromEvo > 0)
            _addInfoText.text +=
                TextManager.GetText(TextType.Element, 34) + ":" +
                UIManager.NewLine(1);

        _addInfoText.text +=
            TextManager.GetText(TextType.Element, 35) + ":" +
            UIManager.NewLine(1) +
            TextManager.GetText(TextType.Element, 36) + ":" +
            UIManager.NewLine(1) +
            TextManager.GetText(TextType.Element, 37) + ":" +
			UIManager.NewLine(1) +
			TextManager.GetText(TextType.Element, 51) + ":";

        //Fill INFO COUNT
        _addInfoCountText.text =
            _curPokeInfo.BaseHp +
            UIManager.NewLine(1);

        if (_curPokeInfo.CpMultiFromEvo > 0)
            _addInfoCountText.text +=
                _curPokeInfo.CpMultiFromEvo + UIManager.GetFormattedText("x", TextColorType.GreyLight, false, 12) +
                UIManager.NewLine(1);

        _addInfoCountText.text +=
            _curPokeInfo.CaptureRate + percentFormatted +
            UIManager.NewLine(1) +
            _curPokeInfo.FleeRate + percentFormatted +
            UIManager.NewLine(1) +
            _curPokeInfo.Rarity.ToString("F") + percentFormatted +
			UIManager.NewLine(1) +
			_curPokeInfo.OriginIsland.ToString();

        yield return new WaitForEndOfFrame();

        _allPanels[3].anchoredPosition = new Vector2(_allPanels[3].anchoredPosition.x, nextPanelPosY);
        _allPanels[3].sizeDelta = new Vector2(_allPanels[3].sizeDelta.x, _addInfoText.GetComponent<RectTransform>().sizeDelta.y + (PanelSpace * 2));
        nextPanelPosY -= _allPanels[3].sizeDelta.y + PanelSpace;
        #endregion
        
        _content.sizeDelta = new Vector2(0, Mathf.Abs(nextPanelPosY) + UIManager.PanelBottomOffset);

		if (firstOpening)
		{
			firstOpening = false;
			for (int i = 0; i < 3; i++)
			{
				hepta[i].enabled = true;
			}
		}
    }
	int GetID(int id)
	{
		int r = 0;
		AppManager temp = AppManager.Instance;
		int length = temp.PokeData.PokeInfos.Length;
		for (int i = 0; i < length; i++)
		{
			if(temp.PokeData.PokeInfos[i].Id == id)
			{
				r = i;
				break;
			}
		}
		return r;
	}

	float CalculateHeptagonDistance(int stat,int pos)
	{
		float r = (float)stat/(float)maxStatsTemp[pos];
		return r;
	}
    public void Show(bool useLastScrollPos, YieldComplete onYieldComplete)
    {
        if (useLastScrollPos)
            _scrollRect.verticalNormalizedPosition = _lastScrollPos;
        else _scrollRect.verticalNormalizedPosition = 1;

        gameObject.SetActive(true);
        StartCoroutine(SwitchPanelYield(true, useLastScrollPos, onYieldComplete));
    }

    public void Hide(YieldComplete onYieldComplete)
    {
        _lastScrollPos = _scrollRect.verticalNormalizedPosition;
        StartCoroutine(SwitchPanelYield(false, false, onYieldComplete));
    }

    IEnumerator SwitchPanelYield(bool show, bool useLastScrollPos, YieldComplete onYieldComplete)
    {
        float lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime * UIManager.ShowHideTimeMulti;

            if (show)
            {
                _content.localScale = new Vector3(Mathf.SmoothStep(UIManager.HideSize.x, 1, lt),
                    Mathf.SmoothStep(UIManager.HideSize.y, 1, lt));
                _canvasGroup.alpha = Mathf.SmoothStep(0, 1, lt);
            }
            else
            {
                _content.localScale = new Vector3(Mathf.SmoothStep(1, UIManager.HideSize.x, lt), Mathf.SmoothStep(1, UIManager.HideSize.y, lt));
                _canvasGroup.alpha = Mathf.SmoothStep(1, 0, lt);
            }

            if (useLastScrollPos)
                _scrollRect.verticalNormalizedPosition = _lastScrollPos;

            yield return new WaitForEndOfFrame();
        }

        if (!show)
        {
            gameObject.SetActive(false);
        }
        else if (useLastScrollPos)
            _scrollRect.verticalNormalizedPosition = _lastScrollPos;

        onYieldComplete();
    }


    public void OnPrevPokeClick()
    {
        if (UIManager.Instance.State == UIState.BlockInput)
            return;

        if (_curPokeInfo.EvoFromId >= 0)
            Hide(() => UIManager.Instance.ShowPokemonPage(_curPokeInfo.EvoFromId));
    }

    public void OnNextPokeClick()
    {
        if (UIManager.Instance.State == UIState.BlockInput)
            return;

        if (_curPokeInfo.EvoToId >= 0)
            Hide(() => UIManager.Instance.ShowPokemonPage(_curPokeInfo.EvoToId));
    }
}
