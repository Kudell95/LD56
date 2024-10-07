using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public Image ActionImage;
    public Button Button;

    public TextMeshProUGUI TypeText;

    public ShopItemSO ShopItem;

    public void Build(ShopItemSO item)
    {
           TitleText.text = item.Name;
           DescriptionText.text = item.Description + "\n" + item.EffectDescription;

           ActionImage.sprite = item.DisplayImage;
           ShopItem = item;

           TypeText.text = item.ShopItemType.ToString();
    }

    public void SelectItem()
    {
        ShopController.Instance.UseShopItem(ShopItem);
    }

}
