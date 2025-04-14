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
    private Vector2 dragOffset;
    private TurnManager turnManager;
    private GraveyardManager graveyardManager;

    private CardData cardData;
    public CardData CardData => cardData;
    private int cardMana;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();  // Ensure we are getting the right canvas
        graveyardManager = GameObject.FindAnyObjectByType<GraveyardManager>();
        if (playArea == null)
        {
            var playAreaObject = GameObject.FindGameObjectWithTag("PlayArea");
            if (playAreaObject != null)
                playArea = playAreaObject.transform;
        }
        turnManager = GameObject.FindAnyObjectByType<TurnManager>();


    }

    public void setup (CardData cardData)
    {
        this.cardData = cardData;
        cardMana = cardData.manaCost;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out var localPoint
        );

        dragOffset = rectTransform.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out var localPoint
        );

        rectTransform.anchoredPosition = localPoint + dragOffset;
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
        Debug.Log("Checking if in play area");
        return RectTransformUtility.RectangleContainsScreenPoint(
            playArea as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );
    }

    private void PlayCard()
    {
        Debug.Log("Play Card");

        if (turnManager.canPlayCard(cardMana))
        {
            int currentMana = turnManager.PlayerManaCheck();
            int maxMana = turnManager.PlayerMaxCheck();
            Debug.Log("current card mana: " + cardMana);
            Debug.Log("Current Player Mana: " + currentMana);
            Debug.Log("Current Max Mana: " + maxMana);
            // Trigger action, move to graveyard
            //Debug.Log($"Played: {GetComponent<CardDisplay>().CardData.cardName}"); //NOTES: Currently bugs if run needs update on display
            graveyardManager.AddToGraveyard(cardData);
            turnManager.useCard(cardData);
            AssignedSlot.currentCard = null;
            Destroy(this.gameObject);
            // Optionally: notify HandManager to refill hand
        }
        else
        {
            ResetPosition();
        }

    }






}
