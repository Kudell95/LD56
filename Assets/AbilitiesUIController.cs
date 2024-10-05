using Unity.VisualScripting;
using UnityEngine;

public class AbilitiesUIController : MonoBehaviour
{
    public ActionController ActionCtrler;


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


    

}
