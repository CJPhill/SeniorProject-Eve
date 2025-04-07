using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public CardSlot AssignedSlot { get; set; }
    public Transform playArea;
    public float swapRange = 100f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();  // Ensure we are getting the right canvas
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Disable raycasting during the drag to avoid blocking UI interactions
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert screen space mouse position to local space in the canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        // Update the card's position based on the mouse position
        rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable raycasting after the drag ends
        canvasGroup.blocksRaycasts = true;

        // Snap to closest slot
        CardSlot[] allSlots = FindObjectsOfType<CardSlot>();
        CardSlot closest = null;
        float closestDist = float.MaxValue;

        foreach (var slot in allSlots)
        {
            float dist = Vector2.Distance(transform.position, slot.transform.position);
            if (dist < closestDist && slot != AssignedSlot)
            {
                closest = slot;
                closestDist = dist;
            }
        }

        if (closest != null && closestDist < swapRange)
        {
            // Swap cards between the slots
            if (closest.currentCard != null)
            {
                var temp = closest.currentCard;
                closest.currentCard = this;
                AssignedSlot.currentCard = temp;

                temp.AssignedSlot = AssignedSlot;
                temp.transform.SetParent(AssignedSlot.transform);
                temp.ResetPosition();
            }
            else
            {
                AssignedSlot.currentCard = null;
                closest.currentCard = this;
            }

            AssignedSlot = closest;
            transform.SetParent(closest.transform);
            ResetPosition();
        }
        else if (IsInPlayArea())
        {
            PlayCard();
        }
        else
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        // If you want to snap the card back to the assigned slot:
        if (AssignedSlot != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    private bool IsInPlayArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            playArea as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );
    }

    private void PlayCard()
    {
        // Trigger action, move to graveyard
        Debug.Log($"Played: {GetComponent<CardDisplay>().CardData.cardName}");
        AssignedSlot.currentCard = null;
        Destroy(gameObject);
        // Optionally: notify HandManager to refill hand
    }
}
