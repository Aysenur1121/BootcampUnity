using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisappearedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform targetTransform;
    public float hoverDuration = 0.5f;
    public Vector2 hoverOffset = new Vector2(100f, 0);

    private Vector2 originalPosition;
    private bool isHovering;

    private void Start()
    {
        originalPosition = targetTransform.anchoredPosition;
        isHovering = false;
    }

    private void Update()
    {
        if (isHovering)
        {
            Vector2 targetPosition = originalPosition + hoverOffset;
            targetTransform.anchoredPosition = Vector2.Lerp(targetTransform.anchoredPosition, targetPosition, Time.deltaTime / hoverDuration);
        }
        else
        {
            targetTransform.anchoredPosition = Vector2.Lerp(targetTransform.anchoredPosition, originalPosition, Time.deltaTime / hoverDuration);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}

