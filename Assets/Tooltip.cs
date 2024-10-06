using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public string Title;
    [Multiline]
    public string Description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(Title, Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }

    private void OnMouseEnter() {
        TooltipManager.Instance.ShowTooltip(Title, Description);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
    }
}
