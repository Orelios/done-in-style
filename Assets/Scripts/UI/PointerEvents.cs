using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEvents : MonoBehaviour
{
    [Header("Changing Graphic Colours")]
    [SerializeField] private Graphic[] graphicsToColorize;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    
    [Header("Changing UGUI Text")]
    [SerializeField] private TextMeshProUGUI textToChange;
    [SerializeField] private float horizontalPositionOnEnter;
    [SerializeField] private float horizontalPositionOnExit;

    public void ChangeColorOnPointerEnter()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = selectedColor;
        }
    }

    public void MoveTextOnPointerEnter()
    {
        var movedPosition = textToChange.rectTransform.anchoredPosition;
        movedPosition.x = horizontalPositionOnEnter;
        textToChange.rectTransform.anchoredPosition = movedPosition;
    }

    public void ChangeColorOnPointerExit()
    {
        foreach (var graphic in graphicsToColorize)
        {
            graphic.color = unselectedColor;
        }
    }

    public void MoveTextOnPointerExit()
    {
        var movedPosition = textToChange.rectTransform.anchoredPosition;
        movedPosition.x = horizontalPositionOnExit;
        textToChange.rectTransform.anchoredPosition = movedPosition;
    }
}
