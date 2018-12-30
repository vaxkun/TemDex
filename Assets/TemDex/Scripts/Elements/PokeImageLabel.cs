using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PokeImageLabel : MonoBehaviour {

	[Header("Obj refs")]
    [SerializeField] private Image _pokeImage;
    [SerializeField] private Text _pokeNameText;
    [SerializeField] private Text _candyEvoText;

    public void Set(Sprite pokeImage, string pokeName)
    {
        _pokeImage.sprite = pokeImage;
        _pokeNameText.text = pokeName;
    }

    public void Set(Sprite pokeImage, string pokeName, int candyEvo)
    {
        if (_pokeNameText)
            _pokeNameText.text = pokeName;
        _pokeImage.sprite = pokeImage;
        
        if (_candyEvoText)
            _candyEvoText.text = candyEvo.ToString();
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
