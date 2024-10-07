using Unity.VisualScripting;
using UnityEngine;

public class AbilitiesUIController : MonoBehaviour
{
    public ActionController ActionCtrler;
    public ActionController.WindowTypes ButtonType;


    public void OpenPanel(){
        if(ButtonType == ActionController.WindowTypes.Ability)
            OpenAbilitiesPanel();
        else
            OpenItemsPanel();
    }

    public void OpenAbilitiesPanel()
    {
        if(GameManager.Instance.TurnManagerObject.CurrentTurnState != Enums.TurnStates.PlayerTurn)
            return;

        if(ActionCtrler.Active && ActionCtrler.CurrentWindow == ActionController.WindowTypes.Ability)
            return;
        
        SoundManager.Instance?.PlaySound("select");
        

        ActionCtrler.PopulateAbilities();
        ActionCtrler.ShowPanel();
    }

    public void OpenItemsPanel()
    {
        if(GameManager.Instance.TurnManagerObject.CurrentTurnState != Enums.TurnStates.PlayerTurn)
            return;

        if(ActionCtrler.Active && ActionCtrler.CurrentWindow == ActionController.WindowTypes.Item)
            return;
        
        SoundManager.Instance?.PlaySound("select");


        ActionCtrler.PopulateItems();
        ActionCtrler.ShowPanel();
    }


    

}
