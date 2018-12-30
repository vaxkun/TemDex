using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ElementSelection : MonoBehaviour
{
    public ElementSelected OnElementSelected = selected => {};

    public int ElementId = 0;
    public SelectionType SelectionType = SelectionType.PanelHighlight;

    public bool Selected
    {
        get { return _selected; }
        set
        {
            if (!_image)
                _image = GetComponent<Image>();

            if (value)
            {
                switch (SelectionType)
                {
                    case SelectionType.PanelHighlight:
                        _image.sprite = UIManager.Instance.UIRes.HighlightPanelImage[1];
                        break;
                    case SelectionType.Favorites:
                        _image.sprite = UIManager.Instance.UIRes.FavSprites[1];
                        break;
                }
            }
            else
            {
                switch (SelectionType)
                {
                    case SelectionType.PanelHighlight:
                        _image.sprite = UIManager.Instance.UIRes.HighlightPanelImage[0];
                        break;
                    case SelectionType.Favorites:
                        _image.sprite = UIManager.Instance.UIRes.FavSprites[0];
                        break;
                }
            }
            _selected = value;
        }
    }
    [SerializeField]
    private bool _selected;

    [Header("Obj refs")]
    [SerializeField]
    private Image _iconImage;

    private Image _image;

    public void SwitchElement()
    {
        Selected = !Selected;

        OnElementSelected(Selected);
    }
}

public enum SelectionType
{
    PanelHighlight, Favorites
}
