using Unity.VisualScripting;
using UnityEngine;

public class AbilitiesUIController : MonoBehaviour
{
    public ActionController ActionCtrler;


    public void OpenAbilitiesPanel()
    {
        if(GameManager.Instance.TurnManagerObject.CurrentTurnState != Enums.TurnStates.PlayerTurn)
            return;

        //TODO: actually populate it.
        ActionCtrler.PopulateActionItems();
        ActionCtrler.ShowPanel();
    }


    

}
