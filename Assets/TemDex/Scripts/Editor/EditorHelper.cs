using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.UI;

public class EditorHelper : Editor
{
    public const string UiMaterialPath = "Assets/POKEDEXX/Graphics/Materials/UI.mat";
    public const string UiFontMaterialPath = "Assets/POKEDEXX/Graphics/Materials/Font.mat";

    public const string PokeImagesPath = "Assets/POKEDEXX/Graphics/PokeImages/";
    public static readonly string PokeImagesFullPath = Application.dataPath + "/POKEDEXX/Graphics/PokeImages/";
    public const int PokeImageWidth = 260;
    public const int PokeImageHeight = 240;

    public const string PokeDataObjPath = "Assets/POKEDEXX/Objects/PokeData.asset";
    public const string JsonPath = "Assets/POKEDEXX/Objects/JSON_data/";

    public const string PokemonsJsonName = "Pokemons.json";
    public const string PrimaryMovesJsonName = "PrimaryMoves.json";
    public const string SecondaryMovesJsonName = "SecondaryMoves.json";
    public const string TypeChartJsonName = "TypeChart.json";

    
    [MenuItem("Editor Tools/Load Poke Info from JSON")]
    static void LoadPokeInfoFromJson()
    {
        try
        {
            PokeData pokeData = (PokeData)AssetDatabase.LoadAssetAtPath(PokeDataObjPath, typeof(PokeData));

            #region Fill Type Chart
            TextAsset typeChartJsonString = (TextAsset)AssetDatabase.LoadAssetAtPath(JsonPath + TypeChartJsonName, typeof(TextAsset));

            PokeTypeChartJson typeChartJson = JsonUtility.FromJson<PokeTypeChartJson>(typeChartJsonString.text);
            TypeChart typeChart = pokeData.TypeChart = new TypeChart(typeChartJson.TypeChart.Length);

            for (int i = 0; i < typeChart.TypeParameters.Length; i++)
            {
                TypeParameter typeParam = typeChart.TypeParameters[i] = new TypeParameter();

                typeParam.BaseType = (TemType)Enum.Parse(typeof(TemType), typeChartJson.TypeChart[i].Type);

                if (!string.IsNullOrEmpty(typeChartJson.TypeChart[i].Strenghts))
                {
                    string[] strenghtsString = typeChartJson.TypeChart[i].Strenghts.Split(new[] { ", " }, StringSplitOptions.None);
                    typeParam.StrenghtsTypes = new TemType[strenghtsString.Length];
                    for (int j = 0; j < typeParam.StrenghtsTypes.Length; j++)
                    {
                        typeParam.StrenghtsTypes[j] = (TemType)Enum.Parse(typeof(TemType), strenghtsString[j]);
                    }
                }

                if (!string.IsNullOrEmpty(typeChartJson.TypeChart[i].Weaknesses))
                {
                    string[] weaksString = typeChartJson.TypeChart[i].Weaknesses.Split(new[] { ", " }, StringSplitOptions.None);
                    typeParam.WeaknessesTypes = new TemType[weaksString.Length];
                    for (int j = 0; j < typeParam.WeaknessesTypes.Length; j++)
                    {
                        typeParam.WeaknessesTypes[j] = (TemType)Enum.Parse(typeof(TemType), weaksString[j]);
                    }
                }
            }
            #endregion

            #region Load Primary Moves
            TextAsset primaryMovesJsonString = (TextAsset)AssetDatabase.LoadAssetAtPath(JsonPath + PrimaryMovesJsonName, typeof(TextAsset));
            PrimaryMovesDataJson primaryMovesDataJson = JsonUtility.FromJson<PrimaryMovesDataJson>(primaryMovesJsonString.text);

            pokeData.PrimaryMoves = new PrimaryMove[primaryMovesDataJson.PrimaryMoves.Length];
            for (int i = 0; i < primaryMovesDataJson.PrimaryMoves.Length; i++)
            {
                PrimaryMove primaryMove = pokeData.PrimaryMoves[i] = new PrimaryMove();
                PrimaryMovesJson primaryMovesJson = primaryMovesDataJson.PrimaryMoves[i];

                primaryMove.Name = primaryMovesJson.Name;
                primaryMove.Type = (TemType)Enum.Parse(typeof(TemType), primaryMovesJson.Type);
                primaryMove.attack = primaryMovesJson.Atk;
                primaryMove.hold = primaryMovesJson.Cooldown;
                primaryMove.staminaUsed = primaryMovesJson.DPS;
            }
            #endregion

            #region Load Secondary Moves
            TextAsset secondaryMovesJsonString = (TextAsset)AssetDatabase.LoadAssetAtPath(JsonPath + SecondaryMovesJsonName, typeof(TextAsset));
            SecondaryMovesDataJson secondaryMovesDataJson = JsonUtility.FromJson<SecondaryMovesDataJson>(secondaryMovesJsonString.text);

            pokeData.SecondaryMoves = new SecondaryMove[secondaryMovesDataJson.SecondaryMoves.Length];
            for (int i = 0; i < secondaryMovesDataJson.SecondaryMoves.Length; i++)
            {
                SecondaryMove secMove = pokeData.SecondaryMoves[i] = new SecondaryMove();
                SecondaryMovesJson secMoveJson = secondaryMovesDataJson.SecondaryMoves[i];

                secMove.Name = secMoveJson.Name;
                secMove.Type = (TemType)Enum.Parse(typeof(TemType), secMoveJson.Type);
                secMove.ChargeCount = Mathf.Clamp(secMoveJson.ChargeCount, 1, 5);
                secMove.attack = secMoveJson.Atk;
                secMove.hold = secMoveJson.Cooldown;
                secMove.staminaUsed = secMoveJson.DPS;
                secMove.CritChance = secMoveJson.CritChance;
                secMove.DidgeWindow = secMoveJson.DodgeWindow;
            }
            #endregion

            #region Load Pokemons
            TextAsset pokemonsJsonString = (TextAsset)AssetDatabase.LoadAssetAtPath(JsonPath + PokemonsJsonName, typeof(TextAsset));
            PokeDataJson pokeDataJson = JsonUtility.FromJson<PokeDataJson>(pokemonsJsonString.text);

            pokeData.PokeInfos = new TemInfo[pokeDataJson.Pokemons.Length];
            for (int i = 0; i < pokeDataJson.Pokemons.Length; i++)
            {
                TemInfo pokeInfo = pokeData.PokeInfos[i] = new TemInfo();
                PokeInfoJson pokeInfoJson = pokeDataJson.Pokemons[i];
                pokeInfoJson.Name = Regex.Replace(pokeInfoJson.Name, @"\p{Z}", "");

                pokeInfo.temName = pokeInfoJson.Name;
                pokeInfo.Id = i + 1;

                #region Fill Type
                string[] pokeTypesJson = pokeInfoJson.Type.Split(new[] {", "}, StringSplitOptions.None);
                pokeInfo.Type = new TemType[pokeTypesJson.Length];

                for (int j = 0; j < pokeTypesJson.Length; j++)
                {
                    pokeTypesJson[j] = pokeTypesJson[j].Replace(" ", string.Empty);
                    pokeInfo.Type[j] = (TemType)Enum.Parse(typeof(TemType), pokeTypesJson[j]);
                }
                #endregion

                pokeInfo.Weight = pokeInfoJson.Weight;
                pokeInfo.Height = pokeInfoJson.Height;
                pokeInfo.MaxCp = pokeInfoJson.MaxCp;
                pokeInfo.CpMultiFromEvo = pokeInfoJson.EvoCpMulti;
                pokeInfo.BaseAttack = pokeInfoJson.BaseAttack;
                pokeInfo.BaseDefense = pokeInfoJson.BaseDefense;
                pokeInfo.BaseStamina = pokeInfoJson.BaseStamina;
                pokeInfo.BaseHp = pokeInfoJson.BaseHP;
                pokeInfo.Rarity = pokeInfoJson.Rarity;
                pokeInfo.CaptureRate = pokeInfoJson.CaptureRate;
                pokeInfo.FleeRate = pokeInfoJson.FleeRate;
                pokeInfo.Class = (PokeClass) Enum.Parse(typeof (PokeClass), pokeInfoJson.Class);
                pokeInfo.LvlToEvolve = pokeInfoJson.CandyToEvolve;

                int eggDistance = 0;
                if (int.TryParse(pokeInfoJson.EggDistanceToHatch.Replace("km", string.Empty), out eggDistance))
                    pokeInfo.EggDistanceType = (EggType)eggDistance;

                #region Fill Primary Moves
                string[] primaryMovesString = pokeInfoJson.PossiblePrimaryMoves.Split(new[] { ", " }, StringSplitOptions.None);
                
                pokeInfo.PrimaryMovesIds = new int[primaryMovesString.Length];
                for (int j = 0; j < pokeInfo.PrimaryMovesIds.Length; j++)
                {
                    pokeInfo.PrimaryMovesIds[j] = pokeData.GetPrimaryMoveId(primaryMovesString[j]);
                }
                #endregion

                #region Fill Secondary Moves
                string[] secondaryMovesString = pokeInfoJson.PossibleSecondaryMoves.Split(new[] { ", " }, StringSplitOptions.None);

                pokeInfo.SecondaryMovesIds = new int[secondaryMovesString.Length];
                for (int j = 0; j < pokeInfo.SecondaryMovesIds.Length; j++)
                {
                    pokeInfo.SecondaryMovesIds[j] = pokeData.GetSecondaryMoveId(secondaryMovesString[j]);
                }
                #endregion

                pokeInfo.Resistance = pokeData.TypeChart.GetResistance(pokeInfo.Type);
                pokeInfo.Weaknesses = pokeData.TypeChart.GetWeaknesses(pokeInfo.Type);
                
            }

            #region Fill Evo FromTo
            for (int i = 0; i < pokeData.PokeInfos.Length; i++)
            {
                TemInfo pokeInfo = pokeData.PokeInfos[i];
                PokeInfoJson pokeInfoJson = pokeDataJson.Pokemons[i];

                if (!string.IsNullOrEmpty(pokeInfoJson.EvoFrom))
                    pokeInfo.EvoFromId = pokeData.GetPokeInfoIdByName(Regex.Replace(pokeInfoJson.EvoFrom, @"\p{Z}", ""));
                else pokeInfo.EvoFromId = -1;
                if (!string.IsNullOrEmpty(pokeInfoJson.EvoTo))
                    pokeInfo.EvoToId = pokeData.GetPokeInfoIdByName(Regex.Replace(pokeInfoJson.EvoTo, @"\p{Z}", ""));
                else pokeInfo.EvoToId = -1;
            }
            #endregion

            Debug.Log("Loading complete!");
            #endregion

            #region Fill Pokemon stats Rates
            Vector3 maxStatsRates = Vector3.zero; //x-Attack, y-Defense, z-Hp
            for (int i = 0; i < pokeData.PokeInfos.Length; i++)
            {
                TemInfo pInfo = pokeData.PokeInfos[i];
                if (pInfo.BaseAttack > maxStatsRates.x)
                    maxStatsRates.x = pInfo.BaseAttack;
                if (pInfo.BaseDefense > maxStatsRates.y)
                    maxStatsRates.y = pInfo.BaseDefense;
                if (pInfo.BaseStamina > maxStatsRates.z)
                    maxStatsRates.z = pInfo.BaseStamina;
            }

            for (int i = 0; i < pokeData.PokeInfos.Length; i++)
            {
                TemInfo pInfo = pokeData.PokeInfos[i];
				//pInfo.AttackRate = (float) pInfo.BaseAttack / maxStatsRates.x;
                //pInfo.DefenseRate = (float) pInfo.BaseDefense / maxStatsRates.y;
                //pInfo.StaminaRate = (float) pInfo.BaseStamina / maxStatsRates.z;
            }
            #endregion

            #region Fill Move stats Rates
            Vector3 maxMoveRates = Vector3.zero; //x-Attack, y-Cooldown, z-DPS

            //Primary
            for (int i = 0; i < pokeData.PrimaryMoves.Length; i++)
            {
                PrimaryMove move = pokeData.PrimaryMoves[i];
                if (move.attack > maxMoveRates.x)
                    maxMoveRates.x = move.attack;
                if (move.hold > maxMoveRates.y)
                    maxMoveRates.y = move.hold;
                if (move.staminaUsed > maxMoveRates.z)
                    maxMoveRates.z = move.staminaUsed;
            }

            for (int i = 0; i < pokeData.PrimaryMoves.Length; i++)
            {
                PrimaryMove move = pokeData.PrimaryMoves[i];
            }

            //Secondary
            maxMoveRates = Vector3.zero;
            for (int i = 0; i < pokeData.SecondaryMoves.Length; i++)
            {
                SecondaryMove move = pokeData.SecondaryMoves[i];
                if (move.attack > maxMoveRates.x)
                    maxMoveRates.x = move.attack;
                if (move.hold > maxMoveRates.y)
                    maxMoveRates.y = move.hold;
                if (move.staminaUsed > maxMoveRates.z)
                    maxMoveRates.z = move.staminaUsed;
            }

            for (int i = 0; i < pokeData.SecondaryMoves.Length; i++)
            {
                SecondaryMove move = pokeData.SecondaryMoves[i];
            }
            #endregion

            EditorUtility.SetDirty(pokeData);
            Debug.Log("Loading Poke info complete!");

        }
        catch (Exception)
        {
            Debug.LogError("Error while loading data from json");
            throw;
        }

    }

    [MenuItem("Editor Tools/Load Poke Images from JSON")]
    static void LoadPokeImagesFromJson()
    {
        try
        {
            PokeData pokeData = (PokeData)AssetDatabase.LoadAssetAtPath(PokeDataObjPath, typeof(PokeData));

            TextAsset pokemonsJsonString = (TextAsset)AssetDatabase.LoadAssetAtPath(JsonPath + PokemonsJsonName, typeof(TextAsset));
            PokeDataJson pokeDataJson = JsonUtility.FromJson<PokeDataJson>(pokemonsJsonString.text);

            for (int i = 0; i < pokeDataJson.Pokemons.Length; i++)
            {
                string imgUrl = pokeDataJson.Pokemons[i].ImageURL;
                string filePath = PokeImagesFullPath + i.ToString() + ".png";

                if (File.Exists(filePath))
                    continue;

                File.WriteAllBytes(filePath, LoadImageBytesFromUrl(imgUrl));
            }

            AssetDatabase.Refresh();

            for (int i = 0; i < pokeData.PokeInfos.Length; i++)
            {
                pokeData.PokeInfos[i].Image = (Sprite) AssetDatabase.LoadAssetAtPath(PokeImagesPath + i.ToString() + ".png", typeof(Sprite));
            }

            EditorUtility.SetDirty(pokeData);
            Debug.Log("Loading images complete!");
        }
        catch (Exception)
        {
            Debug.LogError("Error while loading data from json");
            throw;
        }
    }

    [MenuItem("Editor Tools/Set Poke Images from folder")]
    static void SetPokeImagesFromFolder()
    {
        try
        {
            PokeData pokeData = (PokeData)AssetDatabase.LoadAssetAtPath(PokeDataObjPath, typeof(PokeData));

            for (int i = 0; i < pokeData.PokeInfos.Length; i++)
            {
                pokeData.PokeInfos[i].Image = (Sprite)AssetDatabase.LoadAssetAtPath(PokeImagesPath + i.ToString() + ".png", typeof(Sprite));
            }

            Debug.Log("Setting poke images from folder completed!");
        }
        catch (Exception)
        {
            Debug.LogError("Error while setting Pokes from folder");
            throw;
        }
    }


    [MenuItem("Editor Tools/Apply UI Materials")]
    static void ApplyUiMats()
    {
        Material uiMat = (Material)AssetDatabase.LoadAssetAtPath(UiMaterialPath, typeof(Material));
        Material uiFontMat = (Material)AssetDatabase.LoadAssetAtPath(UiFontMaterialPath, typeof(Material));
        if (!uiMat || !uiFontMat)
        {
            Debug.LogError("UI material was not found");
            return;
        }

        Image[] images = GameObject.FindObjectsOfType<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].material = uiMat;
        }

        Text[] texts = FindObjectsOfType<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].material = uiFontMat;
        }

        Debug.Log("Applying materials complete!");
    }

    [MenuItem("Editor Tools/Prepare all panels _F1")]
    static void PrepareAllPanels()
    {
        string[] allPanels =
        {
            "PokemonList", "Pokemon", "PokemonPage", "SettingsPanel",
            "SearchPanel", "SortPanel", "BasePanel", "MenuPanel",
            "QuickMovePage", "ChargeMovePage", "MovePanel",
            "ListStateLine"
        };
        
        for (int i = 0; i < allPanels.Length; i++)
        {
            GameObject panel = GameObject.Find(allPanels[i]);
            if (panel)
                panel.SetActive(false);
        }
    }

    [MenuItem("Editor Tools/Set Active _F2")]
    static void SetActive()
    {
        if (Selection.activeGameObject)
            Selection.activeGameObject.SetActive(true);
    }

    [MenuItem("Editor Tools/Set Non Active _F3")]
    static void SetNonActive()
    {
        if (Selection.activeGameObject)
            Selection.activeGameObject.SetActive(false);
    }

    [MenuItem("Editor Tools/Delete PlayerPrefs _F12")]
    static void DeletaPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    static byte[] LoadImageBytesFromUrl(string imageUrl)
    {
        using (var webClient = new WebClient())
        {
            byte[] imageBytes = webClient.DownloadData(imageUrl);

            if (imageBytes == null)
            {
                Debug.LogError("Error while loading image from URL");
                return null;
            }
            if (imageBytes.Length == 0)
            {
                Debug.LogError("Error while loading image from URL");
                return null;
            }

            return imageBytes;
        }
    }

    [Serializable]
    public class PrimaryMovesDataJson
    {
        public PrimaryMovesJson[] PrimaryMoves;
    }

    [Serializable]
    public class PrimaryMovesJson
    {
        public string Name;
        public string Type;
        public int Atk;
        public float Cooldown;
        public float DPS;
        public int EnergyGain;
    }

    [Serializable]
    public class SecondaryMovesDataJson
    {
        public SecondaryMovesJson[] SecondaryMoves;
    }

    [Serializable]
    public class SecondaryMovesJson : PrimaryMovesJson
    {
        public int ChargeCount;
        public float CritChance;
        public float DodgeWindow;
    }

    [Serializable]
    public class PokeDataJson
    {
        public PokeInfoJson[] Pokemons;
    }

    [Serializable]
    public class PokeInfoJson
    {
        public string Name;
        public string EvoFrom;
        public string EvoTo;
        public string Type;
        public float Weight;
        public float Height;
        public float MaxCp;
        public int BaseAttack;
        public int BaseDefense;
        public int BaseStamina;
        public int BaseHP;
        public float CaptureRate;
        public float FleeRate;
        public string Class;
        public int CandyToEvolve;
        public string EggDistanceToHatch;
        public float Rarity;
        public float EvoCpMulti;
        public string PossiblePrimaryMoves;
        public string PossibleSecondaryMoves;
        public string ImageURL;
    }

    [Serializable]
    public class PokeTypeChartJson
    {
        public TypeChartJson[] TypeChart;
    }

    [Serializable]
    public class TypeChartJson
    {
        public string Type;
        public string Strenghts;
        public string Weaknesses;
    }

}
