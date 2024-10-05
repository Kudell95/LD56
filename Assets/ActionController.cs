using DG.Tweening;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public Transform ActionItemParent;
    public GameObject Panel;

    private CanvasGroup m_CG;

    public bool Active;

    private bool fading;

    public GameObject ActionItemCardPrefab;

    //could put shop here?
    public WindowTypes CurrentWindow = WindowTypes.Ability;

    public enum WindowTypes {
        Ability,
        Item
    }


    private void Awake() {
        m_CG = Panel.GetComponent<CanvasGroup>();
        ForceClosed();
    }

    public void ClearActionItems()
    {
        foreach(Transform child in ActionItemParent){
            Destroy(child.gameObject);
        }
    }

    public void PopulateAbilities()
    {
        ClearActionItems();
        CurrentWindow = WindowTypes.Ability;
        foreach(PlayerAbilitySO ability in GameManager.Instance.PlayerObject.AbilityList)
        {
            SpawnAndBuildAction(ability);
        }
    }


    public void SpawnAndBuildAction(PlayerAbilitySO ability)
    {
        GameObject spawnedCard = Instantiate(ActionItemCardPrefab, ActionItemParent);

        ActionItemController card = spawnedCard.GetComponent<ActionItemController>();
        card.Build(ability);
    }

    public void PopulateItems()
    {
        CurrentWindow = WindowTypes.Item;

    }


    public void ShowPanel()
    {
        if(fading)
            return;
        // Panel.SetActive(true);

        fading = true;
        Active = true;
        m_CG.DOFade(0,0).OnComplete(()=>{
            m_CG.DOFade(1,0.2f).OnComplete(()=>{
                m_CG.interactable = true;
                m_CG.blocksRaycasts = true;
                fading = false;
            });
        });
    }

    public void HidePanel()
    {
        if(fading)
            return;

        fading = true;
        Active = false;

        m_CG.interactable = false;
        m_CG.blocksRaycasts = false;
        Panel.GetComponent<CanvasGroup>().DOFade(1,0).OnComplete(()=>{
            Panel.GetComponent<CanvasGroup>().DOFade(0,0.2f).OnComplete(()=>{
                ClearActionItems(); 
                fading = false;
            });
        });

    }


    public void ForceClosed(){
        ClearActionItems();
        m_CG.alpha = 0;
        m_CG.interactable = false;
        m_CG.blocksRaycasts = false;
        Active = false;
        fading = false;
    }

    public void TogglePanel(){
        if(Panel.activeSelf)
            HidePanel();
        else
            ShowPanel();
    }
    


}
