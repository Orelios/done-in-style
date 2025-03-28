using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Changing Graphic Colours")]
    [SerializeField] private Graphic[] graphicsToColorize;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    
    [Header("Changing UGUI Text")]
    [SerializeField] private bool shouldChangeTitle;
    [SerializeField] private TextMeshProUGUI titleText;

    public void ChangeColorOnPointerEnter()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = selectedColor;
        }
    }

    public void ChangeColorOnPointerExit()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = unselectedColor;
        }
    }
    
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
