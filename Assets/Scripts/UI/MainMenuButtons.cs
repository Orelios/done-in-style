using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D highlightedCursor;
    
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite buttonHighlightedImage;
    [SerializeField] private Sprite buttonNormalImage;   
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(highlightedCursor, new(50, 0), CursorMode.Auto);
        if (buttonImage != null)
        {
            buttonImage.sprite = buttonHighlightedImage;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(normalCursor, new(33, 2), CursorMode.Auto);
        if (buttonImage != null)
        {
            buttonImage.sprite = buttonNormalImage;
        }
    }
}
