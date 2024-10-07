using DG.Tweening;
using TMPro;
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

    public TextMeshProUGUI PanelTitleText;

    public bool Locked = false;


    public enum WindowTypes {
        Ability,
        Item
    }


    private void Awake() {
        m_CG = Panel.GetComponent<CanvasGroup>();
        ForceClosed();

        Player.OnUseAbility += OnPlayerAbilityUsed;

        TurnManager.OnTurnStart += OnTurnStart;
    }

    public void OnTurnStart(Enums.TurnStates turn)
    {
        if(turn == Enums.TurnStates.PlayerTurn)
            Locked = false;
        else
            Locked = true;
    }

    public void ClearActionItems()
    {
        foreach(Transform child in ActionItemParent){
            Destroy(child.gameObject);
        }
    }

    public void PopulateAbilities()
    {
        PanelTitleText.text = "Select Ability";
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
        PanelTitleText.text = "Select Item";
        CurrentWindow = WindowTypes.Item;
        ClearActionItems();

        foreach(PlayerAbilitySO item in GameManager.Instance.PlayerObject.ItemList)
        {
            SpawnAndBuildAction(item);
        }
    }


    public void OnPlayerAbilityUsed(PlayerAbilitySO ability)
    {
        ForceClosed();
        if(CurrentWindow == WindowTypes.Ability)
            Locked = true;

    }

    public void ShowPanel()
    {
        if(fading || Locked)
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

    public void HideFromButton(){
        SoundManager.Instance?.PlaySound("cancel");
        HidePanel();
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


    private void OnDestroy() {
        Player.OnUseAbility -= OnPlayerAbilityUsed;
    }
    


}
