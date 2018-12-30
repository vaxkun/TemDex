using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class PokeTypeLabel : MonoBehaviour {

    [Header("Obj refs")]
    [SerializeField] private Image _image;

	public void SetType(TemType type)
	{
	    _image.sprite = AppManager.Instance.PokeData.PokeTypeSprites[(int) type];
	}

    public void EnableObj()
    {
        gameObject.SetActive(true);
    }

    public void DisableObj()
    {
        gameObject.SetActive(false);
    }
}
