using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private static LTDescr delay;
    public string header;
    public Sprite image = null;
    [TextArea]
    public string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.5f, () =>
        {
           TooltipSystem.Show(content, header, image);
        });

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }

    public void OnMouseEnter()
    {
        delay = LeanTween.delayedCall(0.5f, () =>
        {
           TooltipSystem.Show(content, header, image);
        });
    }

    public void OnMouseExit()
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }

}
