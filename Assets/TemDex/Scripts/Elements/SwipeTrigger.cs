using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeTrigger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SwipeRight OnSwipeRight = () => { };
    public SwipeLeft OnSwipeLeft = () => { };

    [Range(0f, 1f)]
    public float HeightTreshold = 0.1f;
    [Range(0f, 1f)]
    public float MinSwipeArea = 0.2f;

    private Vector2 _beginDragPos;

    void Start()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragVal = eventData.position - _beginDragPos;
        Vector2 dragNormalized = new Vector2(dragVal.x / Screen.width, dragVal.y / Screen.height);

        if (Mathf.Abs(dragNormalized.y) > HeightTreshold)
            return;

        if (Mathf.Abs(dragNormalized.x) < MinSwipeArea)
            return;

        if (dragNormalized.x < 0)
            OnSwipeLeft();
        else OnSwipeRight();

    }

}
