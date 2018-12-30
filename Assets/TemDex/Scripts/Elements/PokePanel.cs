using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PokePanel : MonoBehaviour {

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    public bool IsActive { get; private set; }

    public virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Show(YieldComplete onYieldComplete)
    {
        gameObject.SetActive(true);
        StartCoroutine(SwitchPanelYield(true, onYieldComplete));
    }

    public virtual void Hide(YieldComplete onYieldComplete)
    {
        if (gameObject.activeSelf)
            StartCoroutine(SwitchPanelYield(false, onYieldComplete));
    }

    public virtual IEnumerator SwitchPanelYield(bool show, YieldComplete onYieldComplete)
    {
        float lt = 0;
        while (lt < 1)
        {
            lt += Time.deltaTime * UIManager.ShowHideTimeMulti;

            if (show)
            {
                _rectTransform.localScale = Vector3.Lerp(UIManager.HideSize, Vector3.one, lt);
                _canvasGroup.alpha = Mathf.SmoothStep(0, 1, lt);
            }
            else
            {
                _rectTransform.localScale = Vector3.Lerp(Vector3.one, UIManager.HideSize, lt);
                _canvasGroup.alpha = Mathf.SmoothStep(1, 0, lt);
            }

            yield return new WaitForEndOfFrame();
        }

        if (!show)
            gameObject.SetActive(false);

        IsActive = show;
        onYieldComplete();
    }
}
