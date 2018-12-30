using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SortLine : MonoBehaviour
{
    public SortMethod Method;

    [Header("Obj refs")]
    [SerializeField]
	private Image[] _sortButtons;

    void Awake()
    {
        for (int i = 0; i < _sortButtons.Length; i++)
        {
            int id = i;
            _sortButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            _sortButtons[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.Instance.SortPokeList(Method, (SortOrder) id);
                UIManager.Instance.ShowListState();
            });
            
        }
    }

    public void SetChecked(int buttonId)
    {
        for (int i = 0; i < _sortButtons.Length; i++)
        {
            if (buttonId == i)
                _sortButtons[i].sprite = UIManager.Instance.UIRes.CheckBoxSprites[1];
            else _sortButtons[i].sprite = UIManager.Instance.UIRes.CheckBoxSprites[0];
        }
    }

    public void UncheckAll()
    {
        for (int i = 0; i < _sortButtons.Length; i++)
        {
            _sortButtons[i].sprite = UIManager.Instance.UIRes.CheckBoxSprites[0];
        }
    }
}
