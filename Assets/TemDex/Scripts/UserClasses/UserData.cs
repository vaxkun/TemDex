using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public bool FirstStart;

    public SortMethod UsingSortMethod;
    public SortOrder UsingSortOrder;

    public List<int> FavoritePokemons;

    public bool[] SearchTypeSelection;
    public bool[] SearchEggSelection;

    public void LoadAll()
    {
        SearchTypeSelection = new bool[Enum.GetNames(typeof(TemType)).Length];
        SearchEggSelection = new bool[Enum.GetNames(typeof(EggType)).Length];
        FavoritePokemons = new List<int>();

        if (!PlayerPrefs.HasKey("FirstStartReached"))
        {
            for (int i = 0; i < SearchTypeSelection.Length; i++)
                SearchTypeSelection[i] = true;

            for (int i = 0; i < SearchEggSelection.Length; i++)
                SearchEggSelection[i] = true;

            PlayerPrefs.SetInt("FirstStartReached", 1);
            FirstStart = true;

            SaveAll();
        }

        //Load sorting methods
        UsingSortMethod = (SortMethod)PlayerPrefs.GetInt("UsingSortMethod", 0);
        UsingSortOrder = (SortOrder)PlayerPrefs.GetInt("UsingSortOrder", 0);

        //Load favorite pokemons
        int favoritePokemonId = 0;
        while (PlayerPrefs.HasKey("FavoritePokemons_" + favoritePokemonId))
        {
            FavoritePokemons.Add(PlayerPrefs.GetInt("FavoritePokemons_" + favoritePokemonId));
            favoritePokemonId++;
        }

        //Load seearch selections
        for (int i = 0; i < SearchTypeSelection.Length; i++)
            SearchTypeSelection[i] = Convert.ToBoolean(PlayerPrefs.GetInt("SearchTypeSelection_" + i));

        for (int i = 0; i < SearchEggSelection.Length; i++)
            SearchEggSelection[i] = Convert.ToBoolean(PlayerPrefs.GetInt("SearchEggSelection_" + i));
    }

    public void SaveAll()
    {
        //Save sorting methods
        PlayerPrefs.SetInt("UsingSortMethod", (int)UsingSortMethod);
        PlayerPrefs.SetInt("UsingSortOrder", (int)UsingSortOrder);

        //Save favorites pokemons
        int favoritePokemonId = 0;
        while (PlayerPrefs.HasKey("FavoritePokemons_" + favoritePokemonId))
        {
            PlayerPrefs.DeleteKey("FavoritePokemons_" + favoritePokemonId);
            favoritePokemonId++;
        }
        for (int i = 0; i < FavoritePokemons.Count; i++)
            PlayerPrefs.SetInt("FavoritePokemons_" + i, FavoritePokemons[i]);

        //Save search selections
        for (int i = 0; i < SearchTypeSelection.Length; i++)
            PlayerPrefs.SetInt("SearchTypeSelection_" + i, Convert.ToInt32(SearchTypeSelection[i]));
        for (int i = 0; i < SearchEggSelection.Length; i++)
            PlayerPrefs.SetInt("SearchEggSelection_" + i, Convert.ToInt32(SearchEggSelection[i]));

        PlayerPrefs.Save();
    }


    public void AddFavoritePokemon(int pokemonId)
    {
        if (FavoritePokemons.Contains(pokemonId))
            return;

        FavoritePokemons.Add(pokemonId);
        SaveAll();
    }

    public void RemoveFavoritePokemon(int pokemonId)
    {
        if (!FavoritePokemons.Contains(pokemonId))
            return;

        FavoritePokemons.Remove(pokemonId);
        SaveAll();
    }
}
