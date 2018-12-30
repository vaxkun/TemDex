using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

[CreateAssetMenu(fileName = "PokeData", menuName = "Scriptable/PokeData")]
public class PokeData : ScriptableObject
{
	public int[] maxStats;
    public TemInfo[] PokeInfos;
    [Space]
    public PrimaryMove[] PrimaryMoves;
    public SecondaryMove[] SecondaryMoves;
    public Color[] PokeTypeColors;
    public TypeChart TypeChart;
    public Sprite[] PokeTypeSprites;

    public int GetPokeInfoIdByName(string pokemonName)
    {
        for (int i = 0; i < PokeInfos.Length; i++)
        {
            if (PokeInfos[i].temName.Equals(pokemonName))
                return i;
        }
        
        Debug.LogError("No PokeInfo with given pokemon name");
        return 0;
    }

    public int GetPrimaryMoveId(string moveName)
    {
        if (string.IsNullOrEmpty(moveName))
        {
            Debug.LogWarning("PokeType in Move is empty");
            return 0;
        }

        for (int i = 0; i < PrimaryMoves.Length; i++)
        {
            if (PrimaryMoves[i].Name.Equals(moveName))
            {
                return i;
            }
        }

        Debug.LogError("No such PokeType in Move: " + moveName);
        return 0;
    }

    public int GetSecondaryMoveId(string moveName)
    {
        if (string.IsNullOrEmpty(moveName))
        {
            Debug.LogWarning("PokeType in Move is empty");
            return 0;
        }

        for (int i = 0; i < SecondaryMoves.Length; i++)
        {
            if (SecondaryMoves[i].Name.Equals(moveName))
            {
                return i;
            }
        }

        Debug.LogError("No such PokeType in Move: " + moveName);
        return 0;
    }
}

[Serializable]
public class TemInfo
{
    public string temName;
    public int Id;
    public TemType[] Type;

    [Space]
    public float Weight;
    public float Height;
    public float MaxCp;
    public float CpMultiFromEvo;

    [Space]
	public int BaseHp;
	public int BaseAttack;
	public int BaseAttackEsp;
    public int BaseDefense;
	public int BaseDefenseEsp;
	public int BaseSpeed;
    public int BaseStamina;

    [Space]
    public float Rarity;
    public float CaptureRate;
    public float FleeRate;
	public Island OriginIsland;

    [Space]
    public PokeClass Class;
    public int LvlToEvolve;
    public EggType EggDistanceType;

    [Space]
    public int[] PrimaryMovesIds;
    public int[] SecondaryMovesIds;

    [Space]
    public int EvoFromId;
    public int EvoToId;

    [Space]
    public TemType[] Resistance;
    public TemType[] Weaknesses;

    [Space]
    public Sprite Image;
}

[Serializable]
public class PrimaryMove
{
    public string Name;
	public int id;
	public TemType Type;

    [Space]
	public AttackType attackType;
	public int attack;
	public float staminaUsed;
	public float hold;
	public byte priority;
	public TemTypeAttack synergy;
}



[Serializable]
public class SecondaryMove : PrimaryMove
{
    public int ChargeCount;
    public float CritChance;
    public float DidgeWindow;
}

[Serializable]
public class TypeChart
{
    public TypeParameter[] TypeParameters;

    public TypeChart(int elementsCount)
    {
        TypeParameters = new TypeParameter[elementsCount];
    }

    public TemType[] GetStrenghts(TemType pokeType)
    {
        for (int i = 0; i < TypeParameters.Length; i++)
        {
            if (TypeParameters[i].BaseType == pokeType)
                return TypeParameters[i].StrenghtsTypes;
        }

        return null;
    }

    public TemType[] GetWeaknesses(TemType pokeType)
    {
        for (int i = 0; i < TypeParameters.Length; i++)
        {
            if (TypeParameters[i].BaseType == pokeType)
                return TypeParameters[i].WeaknessesTypes;
        }

        return null;
    }

    public TemType[] GetResistance(TemType[] baseTypes)
    {
        List<TemType> resistance = new List<TemType>();

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].WeaknessesTypes != null)
                {
                    if (TypeParameters[j].WeaknessesTypes.Contains(baseTypes[i]) && !resistance.Contains(TypeParameters[j].BaseType))
                        resistance.Add(TypeParameters[j].BaseType);
                }
            }
        }

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].StrenghtsTypes != null)
                {
                    if (TypeParameters[j].StrenghtsTypes.Contains(baseTypes[i]))
                    {
                        resistance.Remove(TypeParameters[j].BaseType);
                    }
                }
            }
        }

        return resistance.ToArray();
    }

    public TemType[] GetWeaknesses([NotNull] TemType[] baseTypes)
    {
        List<TemType> weaknesses = new List<TemType>();

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].StrenghtsTypes != null)
                {
                    if (TypeParameters[j].StrenghtsTypes.Contains(baseTypes[i]) && !weaknesses.Contains(TypeParameters[j].BaseType))
                        weaknesses.Add(TypeParameters[j].BaseType);
                }
            }
        }

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].WeaknessesTypes != null)
                {
                    if (TypeParameters[j].WeaknessesTypes.Contains(baseTypes[i]))
                    {
                        weaknesses.Remove(TypeParameters[j].BaseType);
                    }
                }
            }
        }

        return weaknesses.ToArray();
    }
}

[Serializable]
public class TypeParameter
{
    public TemType BaseType;
    public TemType[] StrenghtsTypes;
    public TemType[] WeaknessesTypes;
}

[Serializable]
public enum EggType
{
    None = 0, km2 = 2, km5 = 5, km10 = 10
}

[Serializable]
public enum PokeClass
{
    Normal = 0, Mythical = 1
}

[Serializable]
public enum TemType
{
	Crystal, Digital, Earth, Electric, Fire, Melee, Mental, Nature, Neutral, Toxic, Water, Wind
}
[Serializable]
public enum Island
{
	Unknown, Deniz, Arbury, Omninesia, Cipanku, Tucma, Kisiwa
}
[Serializable]
public enum AttackType
{
	Unknown, Physical, Special, Effect
}
public enum TemTypeAttack
{
	None, Crystal, Digital, Earth, Electric, Fire, Melee, Mental, Nature, Neutral, Toxic, Water, Wind
}