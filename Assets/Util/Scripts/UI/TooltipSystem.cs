using UnityEngine;

public class TooltipSystem : MonoBehaviour
{

    private static TooltipSystem Instance;
    public Tooltip tooltip;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public static void Show(string content, string header = "" , Sprite image = null)
    {
        Instance.tooltip.gameObject.SetActive(true);
        Instance.tooltip.SetText(content, header, image); 

    }

    public static void Hide()
    {
        Instance.tooltip.gameObject.SetActive(false);
    }
}
