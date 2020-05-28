using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private Transform canvas;
    private Transform hand;
    private Vector2 original;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvas = GameObject.Find("畫布").transform;
        hand = GameObject.Find("手牌區域").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        original = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rect.anchoredPosition = original;
    }
}
