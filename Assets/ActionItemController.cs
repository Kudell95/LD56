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
        //TODO: store counts of different types of items. and update count if applicable.
        CountContainer.SetActive(false);
        TitleText.text = ability.Name;
        DescriptionText.text = ability.Description;
        EffectText.text = ability.EffectDescription;
        ActionImage.sprite = ability.AbilityImage;

        m_Ability = ability;
        //TODO: hook up button, probably to an event that will fire.
    }



}
