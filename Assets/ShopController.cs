using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject ShopBackground;
    public GameObject NormalBackground;
    public GameObject ShopUI;

    public GameObject ActionPanel;

    public ShopItemDB ShopDB;

    private WeightedShopItemList ItemList;

    public Transform ShopItemParent;
    public static ShopController Instance;

    public GameObject ShopItemPrefab;

    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    private void Start() {
        ShopUI.SetActive(false);
        ItemList = ShopDB.ShopItemList.Clone();
    }

    public void Show()
    {
        SceneTransitionManager.Instance.FadeToEvent(0.5f,()=>{
                PopulateShop();
                NormalBackground.SetActive(false);
                ShopBackground.SetActive(true);
                ActionPanel.SetActive(false);
                LeanTween.delayedCall(1.5f,()=>{
                    ShopUI.SetActive(true);
                });
        });
    }


    public void Hide()
    {
        ShopUI.SetActive(false);

        SceneTransitionManager.Instance.FadeToEvent(0.5f,()=>{
                NormalBackground.SetActive(true);
                ShopBackground.SetActive(false);
                ActionPanel.SetActive(true);

                LeanTween.delayedCall(1f,()=>{
                    GameManager.EndTurn();
                });
                
        });
    }

    public void PopulateShop()
    {
        ClearShop();

        ShopItemSO firstItem = ItemList.RandomElementByWeight(x=> x.Value).Key.Clone();

        GameObject card = Instantiate(ShopItemPrefab, ShopItemParent);

        card.GetComponent<ShopItemController>().Build(firstItem);

        ShopItemSO item = ItemList.RandomElementByWeight(x=> x.Value).Key.Clone();
        if(item == firstItem)
        {
            int i = 0;
            while(item == firstItem)
            {
                item = ItemList.RandomElementByWeight(x=> x.Value).Key.Clone();

                if(i == 20)
                    break;

                i++;
            }
        }

        card = Instantiate(ShopItemPrefab, ShopItemParent);

        card.GetComponent<ShopItemController>().Build(item);

    }


    public void ClearShop()
    {
        foreach(Transform child in ShopItemParent)
        {
            Destroy(child.gameObject);
        }
    }


    public void UseShopItem(ShopItemSO item)
    {
        if(item.SingleUse && ItemList.ContainsKey(item))
        {
            ItemList.Remove(item);
        }

        GameManager.Instance.PlayerObject.UseShopItem(item);
        //TODO: Animation

        Hide();
    }


}
