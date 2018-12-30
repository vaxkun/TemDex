using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListStateLine : PokePanel
{
    public const float PanelHeight = 20;

    [Header("Obj refs")]
    [SerializeField] private Text _pageText;
    [SerializeField] private Text _stateText;

    public void SetPage(int currentPage, int totalPages, int totalItems)
    {
        _pageText.text =
            TextManager.GetText(TextType.Element, 38) + " " +
            currentPage + "/" + totalPages + " (" + totalItems + " " +
            TextManager.GetText(TextType.Element, 39) + ")";
    }

    public void SetState(ListState state)
    {
        switch (state)
        {
            case ListState.AllPokemons:
                _stateText.text = TextManager.GetText(TextType.Element, 41);
                break;
            case ListState.SearchResults:
                _stateText.text = TextManager.GetText(TextType.Element, 40);
                break;
            case ListState.FavPokes:
                _stateText.text = TextManager.GetText(TextType.Element, 47);
                break;

        }
    }
}

[Serializable]
public enum ListState
{
    AllPokemons = 0, SearchResults, FavPokes
}
