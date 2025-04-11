using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Cursors")] 
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D highlightedCursor;
    
    [Header("Changing Graphic Colours")]
    [SerializeField] private Graphic[] graphicsToColorize;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    
    [Header("Changing Graphic Transparency")]
    [SerializeField] private Graphic[] graphicsToTransparency;
    [SerializeField, Range(0, 1f)] private float selectedTransparency;
    [SerializeField, Range(0, 1f)] private float unselectedTransparency;
    
    [Header("Changing UGUI Text")]
    [SerializeField] private TextMeshProUGUI textToChange;
    [SerializeField] private float horizontalPositionOnEnter;
    [SerializeField] private float horizontalPositionOnExit;
    
    [Header("Changing Graphic Image")]
    [SerializeField] private Graphic[] graphicsToChangeImage;
    [SerializeField] private Sprite[] originalImages;
    [SerializeField] private Sprite[] newImages;
    
    private bool _isMouseDown;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isMouseDown)
        {
            Cursor.SetCursor(highlightedCursor, new(50, 0), CursorMode.Auto);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isMouseDown)
        {
            Cursor.SetCursor(normalCursor, new(33, 2), CursorMode.Auto);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isMouseDown = true;
            Cursor.SetCursor(highlightedCursor, new(50, 0), CursorMode.Auto);
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isMouseDown = false;
            Cursor.SetCursor(normalCursor, new(33, 2), CursorMode.Auto);
        }
    }
    
    public void ChangeColorOnPointerEnter()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = selectedColor;
        }
    }

    public void ChangeTransparencyOnPointerEnter()
    {
        foreach (var graphic in graphicsToTransparency)
        {
            graphic.color = new(graphic.color.r, graphic.color.g, graphic.color.b, selectedTransparency);
        }
    }

    public void MoveTextOnPointerEnter()
    {
        var movedPosition = textToChange.rectTransform.anchoredPosition;
        movedPosition.x = horizontalPositionOnEnter;
        textToChange.rectTransform.anchoredPosition = movedPosition;
    }

    public void ChangeImageOnPointerEnter()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            var index = 0;
        
            foreach (var graphic in graphicsToChangeImage)
            {
                var graphicImage = graphic as Image;
            
                graphicImage.sprite = newImages[index];
            
                index++;
            }
        }
    }
    
    public void ChangeColorOnPointerExit()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = unselectedColor;
        }
    }
    
    public void ChangeTransparencyOnPointerExit()
    {
        foreach (var graphic in graphicsToTransparency)
        {
            graphic.color = new(graphic.color.r, graphic.color.g, graphic.color.b, unselectedTransparency);
        }
    }

    public void MoveTextOnPointerExit()
    {
        var movedPosition = textToChange.rectTransform.anchoredPosition;
        movedPosition.x = horizontalPositionOnExit;
        textToChange.rectTransform.anchoredPosition = movedPosition;
    }
    
    public void ChangeImageOnPointerExit()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            var index = 0;
        
            foreach (var graphic in graphicsToChangeImage)
            {
                var graphicImage = graphic as Image;
            
                graphicImage.sprite = originalImages[index];
            
                index++;
            }
        }
    }

    public void SelectOnPointerDown()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void UnselectOnPointerUp()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
