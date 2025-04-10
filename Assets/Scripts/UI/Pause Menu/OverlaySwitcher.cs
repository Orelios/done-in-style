using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverlaySwitcher : MonoBehaviour
{
    [SerializeField] private Canvas overlayCanvas;
    [SerializeField] private List<GameObject> overlays;
    [SerializeField] private string mainOverlayHash;
    [SerializeField] private string keybindsOverlayHash;
    [SerializeField] private string borderSelectionOverlayHash;

    public void EnableKeybindsOverlay()
    {
        overlayCanvas.sortingOrder = 4;
        DisableAllOverlaysExcept(FindIndexByName(keybindsOverlayHash));
    }

    public void EnableBorderSelectionOverlay()
    {
        overlayCanvas.sortingOrder = 4;
        DisableAllOverlaysExcept(FindIndexByName(borderSelectionOverlayHash));
    }

    public void EnableMainOverlay()
    {
        overlayCanvas.sortingOrder = 2;
        DisableAllOverlaysExcept(FindIndexByName(mainOverlayHash));
    }

    private int FindIndexByName(string indexName)
    {
        var result = overlays.Select((overlay, index) => new{overlay, index})
            .Where(x => x.overlay.name == indexName)
            .Select(x => x.index)
            .FirstOrDefault();
        
        return result;
    }

    private void DisableAllOverlaysExcept(int excludedIndex)
    {
        overlays.Select((overlay, index) => new { overlay = overlay, index })
            .ToList()
            .ForEach(x => x.overlay.SetActive(x.index == excludedIndex));
    }
}
