using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionItemController : MonoBehaviour
{

    public GameObject CountContainer;
    public GameObject DisabledOverlay;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI EffectText;
    public Image ActionImage;
    public Button Button;
    private PlayerAbilitySO m_Ability;



    public void Build(PlayerAbilitySO ability)
    {
        if(ability.ActionType == Enums.ActionTypes.Ability)
            CountContainer.SetActive(false);
        else    
        {
            CountContainer.SetActive(true);
            CountText.text = GetCountText(ability);
        }

        TitleText.text = ability.Name;
        DescriptionText.text = ability.Description;
        EffectText.text = ability.EffectDescription;
        ActionImage.sprite = ability.AbilityImage;


        if(ability.ActionType == Enums.ActionTypes.Ability)
        {
            DisabledOverlay.SetActive(false);
            Button.enabled = true;    
        }
        else if (ability.ActionType == Enums.ActionTypes.Item && GameManager.ItemUsedThisRound)
        {
            DisabledOverlay.SetActive(true);
            Button.enabled = false;
        }
        else
        {
            int count = GetCount(ability);
            DisabledOverlay.SetActive(count == 0); 
            if(count == 0)       
                Button.enabled = false;    
            else
                Button.enabled = true;
        }


        m_Ability = ability;
        //TODO: hook up button, probably to an event that will fire.
    }

    public void UseAction()
    {
        if(m_Ability == null)
            return;
        
        SoundManager.Instance?.PlaySound("select");

        Player.OnUseAbility.Invoke(m_Ability);
    }

    public int GetCount(PlayerAbilitySO ability)
    {
        switch(ability.AbilityType)
        {
            case Enums.AbilityType.Heal:
                return GameManager.Instance.HealthCount;
            case Enums.AbilityType.DefenceBuff:
                return GameManager.Instance.DefenseBuffCount;
            case Enums.AbilityType.AttackBuff:
                return GameManager.Instance.AttackBuffCount;
            default:
                return 0;
        }
    }
    public string GetCountText(PlayerAbilitySO ability)
    {
        switch(ability.AbilityType)
        {
            case Enums.AbilityType.Heal:
                return $"x{GameManager.Instance.HealthCount.ToString()}";
            case Enums.AbilityType.DefenceBuff:
                return $"x{GameManager.Instance.DefenseBuffCount.ToString()}";
            case Enums.AbilityType.AttackBuff:
                return $"x{GameManager.Instance.AttackBuffCount.ToString()}";
            default:
                return "x0";
        }
    }

}
