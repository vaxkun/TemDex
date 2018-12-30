using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    public PokeData PokeData;
    public UserData UserData;
    public UserEvents UserEvents;

    [Space]
    public string AppName = "";
    public string AppVer = "v1.0";

    public string IssueReportUrl = "";
    public string DevTwitterLink = "";

    void Awake()
    {
        Instance = this;

        Input.multiTouchEnabled = false;

        UserData = new UserData();
        UserData.LoadAll();
        UserEvents = new UserEvents();

        Debug.Log("Application initialized");
    }
	private void Start()
	{
		if(PlayerPrefs.GetInt("MaxStats" + AppVer) != 1)
		{
			CheckMaxStats();
		}
	}
	void CheckMaxStats()
	{
		PlayerPrefs.SetInt("MaxStats" + AppVer,1);
		int length = PokeData.PokeInfos.Length;
		for (int i = 0; i < length; i++)
		{
			if(PokeData.PokeInfos[i].BaseHp > PokeData.maxStats[0])
			{
				PokeData.maxStats[0] = PokeData.PokeInfos[i].BaseHp;
			}
			if(PokeData.PokeInfos[i].BaseAttack > PokeData.maxStats[1])
			{
				PokeData.maxStats[1] = PokeData.PokeInfos[i].BaseAttack;
			}
			if (PokeData.PokeInfos[i].BaseAttackEsp > PokeData.maxStats[2])
			{
				PokeData.maxStats[2] = PokeData.PokeInfos[i].BaseAttackEsp;
			}
			if (PokeData.PokeInfos[i].BaseDefense > PokeData.maxStats[3])
			{
				PokeData.maxStats[3] = PokeData.PokeInfos[i].BaseDefense;
			}
			if (PokeData.PokeInfos[i].BaseDefenseEsp > PokeData.maxStats[4])
			{
				PokeData.maxStats[4] = PokeData.PokeInfos[i].BaseDefenseEsp;
			}
			if (PokeData.PokeInfos[i].BaseSpeed > PokeData.maxStats[5])
			{
				PokeData.maxStats[5] = PokeData.PokeInfos[i].BaseSpeed;
			}
			if (PokeData.PokeInfos[i].BaseStamina > PokeData.maxStats[6])
			{
				PokeData.maxStats[6] = PokeData.PokeInfos[i].BaseStamina;
			}
		}
	}
	public void OnApplicationQuit()
    {
        SendUserEvents();

        UIManager.Instance.UpdateUserData();
        UserData.SaveAll();

        Debug.Log("Quit");
    }

    void SendUserEvents()
    {
        Analytics.CustomEvent("End of session", new Dictionary<string, object>
        {
            {"RealtimeSinceStartup", Time.realtimeSinceStartup},

            {"SearchUsed", UserEvents.SearchCount},
            {"SortUsed", UserEvents.SortCount},
            {"FavoritesUsed", UserEvents.FavoritesCount},
            {"PokemonPageOpened", UserEvents.PokemonPageCount},
            {"PokemonMoveOpened", UserEvents.PokemonMoveCount},
        });
    }

    public static void RegisterUserEvent(UserEventType eventType)
    {
        switch (eventType)
        {
            case UserEventType.SearchUsed:
                Instance.UserEvents.SearchCount++;
                break;
            case UserEventType.SortUsed:
                Instance.UserEvents.SortCount++;
                break;
            case UserEventType.FavoritesUsed:
                Instance.UserEvents.FavoritesCount++;
                break;
            case UserEventType.PokemonPageOpened:
                Instance.UserEvents.PokemonPageCount++;
                break;
            case UserEventType.PokemonMoveOpened:
                Instance.UserEvents.PokemonMoveCount++;
                break;
        }
    }
}

public delegate void YieldComplete();
public delegate void SwipeRight();
public delegate void SwipeLeft();
public delegate void ElementSelected(bool selected);

public struct UserEvents
{
    public int SearchCount;
    public int SortCount;
    public int FavoritesCount;
    public int PokemonPageCount;
    public int PokemonMoveCount;
}

public enum UserEventType
{
    SearchUsed, SortUsed, FavoritesUsed, PokemonPageOpened, PokemonMoveOpened
}
