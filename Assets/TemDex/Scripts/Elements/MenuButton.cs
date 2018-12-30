using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MenuButton : MonoBehaviour
{
    public Vector2 ShowPos;
    public Vector2 HidePos;

    [Header("Obj refs")]
    [SerializeField] private Text _buttonNameText;

    private RectTransform _rectTransform;
    private Color _defTextColor;
    private Color _hideTextColor;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _defTextColor = _buttonNameText.color;
        _hideTextColor = _defTextColor;
        _hideTextColor.a = 0;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(SwitchYield(true));
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
            StartCoroutine(SwitchYield(false));
    }

    IEnumerator SwitchYield(bool show)
    {
        float lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime*UIManager.ShowHideTimeMulti;
            if (show)
            {
                _rectTransform.anchoredPosition = Vector2.Lerp(HidePos, ShowPos, Mathf.SmoothStep(0, 1, lt));
                _buttonNameText.color = Color.Lerp(_hideTextColor, _defTextColor, lt);
            }
            else
            {
                _rectTransform.anchoredPosition = Vector2.Lerp(ShowPos, HidePos, Mathf.SmoothStep(0, 1, lt));
                _buttonNameText.color = Color.Lerp(_defTextColor, _hideTextColor, lt);
            }

            yield return new WaitForEndOfFrame();

        }

        if (!show)
            gameObject.SetActive(false);
    }

}
