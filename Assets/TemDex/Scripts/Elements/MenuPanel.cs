using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuPanel : MonoBehaviour
{
    //public bool IsMinimized { get; private set; }
    public UIState CurPanelState { get; private set; }

	[Header("Obj refs")]
    [SerializeField] private RectTransform _basePanel;
    [SerializeField] private MenuButton[] _allButtons;

    public void Show(YieldComplete onYieldComplete)
    {
        gameObject.SetActive(true);
        _basePanel.gameObject.SetActive(true);
        StartCoroutine(SwitchPanelYield(true, onYieldComplete));
    }

    public void Hide(YieldComplete onYieldComplete)
    {
        StartCoroutine(SwitchPanelYield(false, onYieldComplete));
    }

    public void ShowButtons()
    {
        for (int i = 0; i < _allButtons.Length; i++)
            _allButtons[i].Show();
    }

    public void HideButtons()
    {
        for (int i = 0; i < _allButtons.Length; i++)
            _allButtons[i].Hide();
    }

    IEnumerator SwitchPanelYield(bool show, YieldComplete onYieldComplete)
    {
        if (show)
            ShowButtons();
        else HideButtons();

        float lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime*UIManager.ShowHideTimeMulti;
            
            yield return new WaitForEndOfFrame();
        }

        if (!show)
        {
            //IsMinimized = false;
            _basePanel.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        onYieldComplete();
    }

    public void OnSubMenuClick(int targetState)
    {
        UIState targetUiState = (UIState)targetState;
        
        UIManager.Instance.TryCloseMenu(targetUiState, delegate
        {
            switch (targetUiState)
            {
                case UIState.SearchMenu:
                    UIManager.Instance.ShowSearchMenu();
                    CurPanelState = UIState.SearchMenu;
                    break;
                case UIState.SortMenu:
                    UIManager.Instance.ShowSortMenu();
                    CurPanelState = UIState.SortMenu;
                    break;
                case UIState.SettingsMenu:
                    UIManager.Instance.ShowSettingsMenu();
                    CurPanelState = UIState.SettingsMenu;
                    break;
            }
        });
    }

    public void OnAllPokesClick()
    {
        UIManager.Instance.SetPokeList(AppManager.Instance.PokeData.PokeInfos);
        UIManager.Instance.HideMenu();
        UIManager.Instance.SetListState(ListState.AllPokemons);
    }

    public void OnFavPokesClick()
    {
        List<TemInfo> favPokes = new List<TemInfo>();
        for (int i = 0; i < AppManager.Instance.UserData.FavoritePokemons.Count; i++)
        {
            int favId = AppManager.Instance.UserData.FavoritePokemons[i];
            favPokes.Add(AppManager.Instance.PokeData.PokeInfos.ToList().Find(info => info.Id == favId));
        }

        UIManager.Instance.SetPokeList(favPokes.ToArray());
        UIManager.Instance.HideMenu();
        UIManager.Instance.SetListState(ListState.FavPokes);

        AppManager.RegisterUserEvent(UserEventType.FavoritesUsed);
    }
    
}
