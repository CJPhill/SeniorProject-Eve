using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;

    public CardSlot AssignedSlot { get; set; }
    public Transform playArea;
    public float swapRange = 100f;

    private Vector2 dragOffset;
    private TurnManager turnManager;
    private GraveyardManager graveyardManager;
    private GameObject placeholder;

    private CardData cardData;
    public CardData CardData => cardData;
    private int cardMana;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();
        canvas = GetComponentInParent<Canvas>();
        graveyardManager = GameObject.FindAnyObjectByType<GraveyardManager>();
        turnManager = GameObject.FindAnyObjectByType<TurnManager>();

        if (playArea == null)
        {
            var playAreaObject = GameObject.FindGameObjectWithTag("PlayArea");
            if (playAreaObject != null)
                playArea = playAreaObject.transform;
        }
    }

    public void setup(CardData cardData)
    {
        this.cardData = cardData;
        cardMana = cardData.manaCost;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        layoutElement.ignoreLayout = true;

        // Create placeholder to keep layout spacing
        placeholder = new GameObject("Placeholder");
        var rt = placeholder.AddComponent<RectTransform>();
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = layoutElement.preferredWidth;
        le.preferredHeight = layoutElement.preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        int index = transform.GetSiblingIndex();
        placeholder.transform.SetParent(transform.parent);
        placeholder.transform.SetSiblingIndex(index);

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

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
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        layoutElement.ignoreLayout = false;

        if (placeholder != null)
        {
            int newIndex = placeholder.transform.GetSiblingIndex();
            transform.SetParent(placeholder.transform.parent);
            transform.SetSiblingIndex(newIndex);
            Destroy(placeholder);
        }

        if (IsInPlayArea())
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
        if (turnManager.canPlayCard(cardMana))
        {
            graveyardManager.AddToGraveyard(cardData);
            turnManager.useCard(cardData);
            AssignedSlot.currentCard = null;
            Destroy(this.gameObject);
        }
        else
        {
            ResetPosition();
        }
    }
}
