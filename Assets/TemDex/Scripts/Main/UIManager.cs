using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    #region static
    public static readonly Dictionary<TextColorType, string> TextColors = new Dictionary<TextColorType, string>
    {
        { TextColorType.BlackLight, "#464646FF" },
        { TextColorType.GreyLight, "#969696FF" },
        { TextColorType.GreenSmooth, "#50955AFF" },
        { TextColorType.RedSmooth, "#e51f2d" },
		{ TextColorType.White, "#FFFFFF" },
		{ TextColorType.LightBlue, "#3899C8" },
		{ TextColorType.LightOrange, "#EA801A" },
    };
    public static readonly Dictionary<TextColorType, Color> TextColorsList = new Dictionary<TextColorType, Color>
    {
        {TextColorType.BlackLight, new Color(0.275f, 0.275f, 0.275f, 1f)},
        {TextColorType.GreyLight, new Color(0.5859f, 0.5859f, 0.5859f, 1f)},
        {TextColorType.GreenSmooth, new Color(0.3125f, 0.586f, 0.3515f, 1f)},
        {TextColorType.RedSmooth, new Color(0.898f, 0.121f, 0.175f, 1f)},
		{ TextColorType.White, new Color(1f, 1f, 1f, 1f) },
		{ TextColorType.LightBlue, new Color(0.2196078f, 0.6f, 0.7843137f, 1f) },
		{ TextColorType.LightOrange, new Color(0.9176471f, 0.5019608f, 0.1019608f, 1f) },
	};

    public const float PanelMinSpace = 5;
    public const float PanelBottomOffset = 75;
    public const float ShowHideTimeMulti = 5f; //4.5f
    public static readonly Vector3 HideSize = new Vector3(0.8f, 0.8f, 1f);
    #endregion

    public static UIManager Instance;

    public UIState State = UIState.BlockInput;
    public UIResources UIRes;

    public EventSystem EventSystem { get { return _eventSystem; } }

    [Header("Obj refs")]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Canvas _baseCanvas;
    
    [SerializeField] private ListStateLine _listStateLine;
    [SerializeField] private MenuPanel _menuPanel;
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private SearchPanel _searchPanel;
    [SerializeField] private SortPanel _sortPanel;
    [SerializeField] private PokemonList _pokemonList;
    [SerializeField] private PokemonPage _pokemonPage;

    [SerializeField] private PokeMovePage _quickMovePage;
    [SerializeField] private PokeMovePage _chargeMovePage;

    private WelcomePage _welcomePage;

    private float _timeToExit;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        _pokemonList.Init();
        _pokemonList.SetSortMethod(AppManager.Instance.UserData.UsingSortMethod, AppManager.Instance.UserData.UsingSortOrder);
        SetPokeList(AppManager.Instance.PokeData.PokeInfos);
        _sortPanel.UpdateSortCheckboxes(_pokemonList.CurSortMethod, _pokemonList.CurSortOrder);
        _searchPanel.UpdateAllSelections();

        SetListState(ListState.AllPokemons);

        if (!AppManager.Instance.UserData.FirstStart)
        {
            _listStateLine.Show(() => { });
            _pokemonList.Show(delegate
            {
                SetState(UIState.PokemonList);
            });
        }
        else
        {
            ShowWelcomePage();
        }

        Debug.Log("UI initialized");
    }

    void Update()
    {
        HandleInput();

        if (_timeToExit > 0)
            _timeToExit -= Time.deltaTime;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (State)
            {
                case UIState.PokemonPage:
                    ClosePokemonPage();
                    break;
                case UIState.PokemonList:
                    break;
                case UIState.Menu:
                    HideMenu();
                    break;
                case UIState.SettingsMenu:
                   _menuPanel.ShowButtons();
                    HideSettingsMenu();
                    break;
                case UIState.SearchMenu:
                    _menuPanel.ShowButtons();
                    HideSearchMenu();
                    break;
                case UIState.SortMenu:
                    _menuPanel.ShowButtons();
                    HideSortMenu();
                    break;
                case UIState.QuickMovePage:
                case UIState.ChargeMovePage:
                    CloseMovePage();
                    break;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Menu))
        {
            switch (State)
            {
                case UIState.PokemonList:
                    ShowMenu();
                    break;
                case UIState.SearchMenu:
                    _menuPanel.ShowButtons();
                    HideSearchMenu();
                    break;
                case UIState.SortMenu:
                    _menuPanel.ShowButtons();
                    HideSortMenu();
                    break;
                case UIState.SettingsMenu:
                    _menuPanel.ShowButtons();
                    HideSettingsMenu();
                    break;
            }
        }
    }


    public void SetPokeList(TemInfo[] pokeList)
    {
        _pokemonList.SetList(pokeList);
        _pokemonList.SortList();
        _pokemonList.FillPage();
    }

    public void TryCloseMenu(UIState targetState, YieldComplete onYieldComplete)
    {
        if (State == UIState.BlockInput || targetState == State)
            return;

        switch (State)
        {
            case UIState.SortMenu:
                SetState(UIState.BlockInput);
                _sortPanel.Hide(() =>
                {
                    SetState(UIState.Menu);
                    onYieldComplete();
                });
                return;
            case UIState.SearchMenu:
                SetState(UIState.BlockInput);
                _searchPanel.Hide(() =>
                {
                    SetState(UIState.Menu);
                    onYieldComplete();
                });
                return;
        }

        onYieldComplete();
    }

    public void ShowWelcomePage()
    {
        if (State == UIState.WelcomePage)
            return;
        SetState(UIState.BlockInput);

        RectTransform welcomePage =
            Instantiate(UIRes.GetResource(UIRes.WelcomePageSource)).GetComponent<RectTransform>();
        welcomePage.SetParent(_baseCanvas.transform);
        welcomePage.sizeDelta = Vector2.zero;
        welcomePage.anchoredPosition = Vector2.zero;

        _welcomePage = welcomePage.GetComponent<WelcomePage>();
        _welcomePage.Show(() => SetState(UIState.WelcomePage));

    }

    public void HideWelcomePanel()
    {
        if (State == UIState.BlockInput || State != UIState.WelcomePage)
            return;

        _welcomePage.Hide(() =>
        {
            Destroy(_welcomePage);

            _listStateLine.Show(() => { });
            _pokemonList.Show(delegate
            {
                SetState(UIState.PokemonList);
            });

        });
    }

    public void ShowListState()
    {
        _listStateLine.Show(() => {});
    }

    public void HideListState()
    {
        _listStateLine.Hide(() => {});
    }

    public void SetListState(ListState state)
    {
        _listStateLine.SetState(state);
    }

    public void SetListPage(int currentPage, int totalPages, int totalItems)
    {
        _listStateLine.SetPage(currentPage, totalPages, totalItems);
    }

    public void ShowMenu()
    {
        if (State != UIState.PokemonList)
            return;
        SetState(UIState.BlockInput);

        HideListState();
        _pokemonList.Hide(() => {});
        _menuPanel.Show(() => SetState(UIState.Menu));
    }

    public void HideMenu()
    {
        if (State == UIState.BlockInput)
            return;

        if (State != UIState.Menu)
        {
            UIState menuState = _menuPanel.CurPanelState;
            switch (menuState)
            {
                case UIState.SearchMenu:
                    _searchPanel.Hide(() => { });
                    break;
                case UIState.SortMenu:
                    _sortPanel.Hide(() => { });
                    break;
                case UIState.SettingsMenu:
                    _settingsPanel.Hide(() => { });
                    break;
            }
        }

        SetState(UIState.BlockInput);
        _pokemonList.Show(() => {});
        ShowListState();
        _menuPanel.Hide(() =>
        {
            SetState(UIState.PokemonList);

        });
    }

    public void ShowSettingsMenu()
    {
        if (State != UIState.Menu)
            return;
        SetState(UIState.BlockInput);

        _menuPanel.HideButtons();
        _settingsPanel.Show(() => SetState(UIState.SettingsMenu));
    }

    public void HideSettingsMenu()
    {
        if (State != UIState.SettingsMenu)
            return;
        SetState(UIState.BlockInput);

        _settingsPanel.Hide(() => SetState(UIState.Menu));
    }

    public void ShowSearchMenu()
    {
        if (State != UIState.Menu)
            return;
        SetState(UIState.BlockInput);

        _menuPanel.HideButtons();
        _searchPanel.Show(() => SetState(UIState.SearchMenu));
    }

    public void HideSearchMenu()
    {
        if (State != UIState.SearchMenu)
            return;
        SetState(UIState.BlockInput);

        _searchPanel.Hide(() => SetState(UIState.Menu));
    }

    public void ShowSortMenu()
    {
        if (State != UIState.Menu)
            return;
        SetState(UIState.BlockInput);

        _menuPanel.HideButtons();
        _sortPanel.Show(() => SetState(UIState.SortMenu));        
    }

    public void HideSortMenu()
    {
        if(State != UIState.SortMenu)
            return;
        SetState(UIState.BlockInput);

        _sortPanel.Hide(() => SetState(UIState.Menu));
    }

    public void ShowPokemonPage(int id)
    {
        if (State != UIState.PokemonList && State != UIState.PokemonPage)
            return;
        SetState(UIState.BlockInput);

        if (_pokemonList.gameObject.activeSelf)
            _pokemonList.Hide(() => { });

        HideListState();
        _pokemonPage.Set(AppManager.Instance.PokeData.PokeInfos[GetID(id)]);
        _pokemonPage.Show(false, () => SetState(UIState.PokemonPage));

        AppManager.RegisterUserEvent(UserEventType.PokemonPageOpened);
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
	public void ClosePokemonPage()
    {
        if (State != UIState.PokemonPage)
            return;
        SetState(UIState.BlockInput);

        _pokemonList.Show(() => {});
        ShowListState();
        _pokemonPage.Hide(() =>
        {
            SetState(UIState.PokemonList);
        });

        
    }

    public void ShowQuickMovePage(PrimaryMove move)
    {
        if (State == UIState.BlockInput)
            return;
        SetState(UIState.BlockInput);
        _pokemonPage.Hide(() => { });

        _quickMovePage.Show(() => SetState(UIState.QuickMovePage));
        _quickMovePage.Set(move);

        AppManager.RegisterUserEvent(UserEventType.PokemonMoveOpened);
    }

    public void ShowSecondaryMovePage(SecondaryMove move)
    {
        if (State == UIState.BlockInput)
            return;
        SetState(UIState.BlockInput);
        _pokemonPage.Hide(() => {});

        _chargeMovePage.Show(() => SetState(UIState.ChargeMovePage));
        _chargeMovePage.Set(move);

        AppManager.RegisterUserEvent(UserEventType.PokemonMoveOpened);
    }

    public void CloseMovePage()
    {
        if (State == UIState.BlockInput)
            return;
        switch (State)
        {
            case UIState.QuickMovePage:
                SetState(UIState.BlockInput);
                _quickMovePage.Hide(() => { });
                _pokemonPage.Show(true, () => SetState(UIState.PokemonPage));
                break;
            case UIState.ChargeMovePage:
                SetState(UIState.BlockInput);
                _chargeMovePage.Hide(() => { });
                _pokemonPage.Show(true, () => SetState(UIState.PokemonPage));
                break;
        }
    }
    
    public void SortPokeList(SortMethod sortMethod, SortOrder sortOrder)
    {
        if (State != UIState.SortMenu)
            return;
        SetState(UIState.BlockInput);

        _sortPanel.UpdateSortCheckboxes(sortMethod, sortOrder);
        if (_sortPanel.IsActive)
            _sortPanel.Hide(() => {});

        _pokemonList.SetSortMethod(sortMethod, sortOrder);
        _pokemonList.SortList();
        _pokemonList.FillPage();

        _pokemonList.Show(() => {});
        _menuPanel.Hide(() => SetState(UIState.PokemonList));
        
        AppManager.RegisterUserEvent(UserEventType.SortUsed);
    }

    public void UpdateUserData()
    {
        AppManager.Instance.UserData.SearchTypeSelection = _searchPanel.GetTypeSelectionValues();
        AppManager.Instance.UserData.SearchEggSelection = _searchPanel.GetEggSelectionValues();
    }

    
    public static void SetState(UIState state)
    {
        Instance.State = state;
    }
    
    public static Vector2 GetCanvasSize()
    {
        return Instance._baseCanvas.GetComponent<RectTransform>().sizeDelta;
    }

    public static string GetColoredText(TextColorType colorType, string text)
    {
        return "<color=" + TextColors[colorType] + ">" + text + "</color>";
    }

    public static string GetBoldText(string text)
    {
        return "<b>" + text + "</b>";
    }

    public static string GetSizedText(int size, string text)
    {
        return "<size=" + size + ">" + text + "</size>";
    }

    public static string NewLine()
    {
        return "\n";
    }

    public static string NewLine(int linesCount)
    {
        string line = "";
        for (int i = 0; i < linesCount; i++)
        {
            line += "\n";
        }
        return line;
    }

    public static string GetFormattedText(string text, TextColorType colorType, bool bold, int size)
    {
        string txt = text;

        if (colorType != TextColorType.None)
            txt = "<color=" + TextColors[colorType] + ">" + txt + "</color>";

        if (bold)
            txt = "<b>" + txt + "</b>";

        if (size > 0)
            txt = "<size=" + size + ">" + txt + "</size>";

        return txt;
    }
}

[Serializable]
public class UIResources
{
    public string WelcomePageSource;
    public Sprite[] ChargeCountSprites;
    public Sprite[] CheckBoxSprites;
    public Sprite[] HighlightPanelImage;
    public Sprite[] FavSprites;
	public Sprite[] AttackTypes;
	public Sprite[] Priorities;
    public Sprite[] EggSprites;

    public GameObject GetResource(string path)
    {
        return Resources.Load<GameObject>(path);
    }
}

public enum TextColorType
{
    BlackLight = 0, GreyLight, GreenSmooth, RedSmooth, None , White, LightBlue, LightOrange
}

public enum UIState
{
    WelcomePage = -1,
    BlockInput = 0,
    PokemonList = 1,
    PokemonPage = 2,
    Menu = 3,
    SearchMenu = 4,
    SortMenu = 5,
    QuickMovePage = 6,
    ChargeMovePage = 7,
    SettingsMenu = 8
}

public enum SortMethod
{
    ById = 0, ByName, ByMaxCp, ByBaseAttack, ByBaseDefense, ByBaseHp
}

public enum SortOrder
{
    Inc = 0, Dec
}