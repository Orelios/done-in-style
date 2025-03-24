using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Graphic[] graphicsToColorize;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private bool shouldChangeTitle;
    [SerializeField] private TextMeshProUGUI titleText;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = selectedColor;
        }

        if (shouldChangeTitle)
        {
            titleText.text = gameObject.name;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = unselectedColor;
        }
        
        if (shouldChangeTitle)
        {
            titleText.text = string.Empty;
        }
    }

    private void OnDisable()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = unselectedColor;
        }
        
        if (shouldChangeTitle)
        {
            titleText.text = string.Empty;
        }
    }
}
