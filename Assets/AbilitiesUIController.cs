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
        
        //TODO: add way to change type. as we'll need to wait until it's closed. - Might not be needed, as 
        // if(ActionCtrler)

        ActionCtrler.PopulateAbilities();
        ActionCtrler.ShowPanel();
    }

    public void OpenItemsPanel(){
        if(GameManager.Instance.TurnManagerObject.CurrentTurnState != Enums.TurnStates.PlayerTurn)
            return;

        if(ActionCtrler.Active && ActionCtrler.CurrentWindow == ActionController.WindowTypes.Item)
            return;
        
        //TODO: add way to change type. as we'll need to wait until it's closed. - Might not be needed, as 
        // if(ActionCtrler)

        ActionCtrler.PopulateItems();
        ActionCtrler.ShowPanel();
    }


    

}
