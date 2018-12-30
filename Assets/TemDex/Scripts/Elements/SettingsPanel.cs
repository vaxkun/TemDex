using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPanel : PokePanel
{
    [Header("Obj refs")]
    [SerializeField] private Text _appNameText;

    void Start()
    {
        _appNameText.text =
            AppManager.Instance.AppName + " " +
            UIManager.GetFormattedText(AppManager.Instance.AppVer, TextColorType.GreyLight, false, -1);
    }

    public void OnReportClick()
    {
        Application.OpenURL(AppManager.Instance.IssueReportUrl);
    }

    public void OnDevLinkClick()
    {
        Application.OpenURL(AppManager.Instance.DevTwitterLink);
    }
	
}
