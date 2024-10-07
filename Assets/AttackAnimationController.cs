using System;
using System.Collections;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    public static AttackAnimationController Instance;
    
    public GameObject CurrentAnimation;
    


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);    


        CurrentAnimation.SetActive(false);
    }



    public void PlayAttackAnimation(Enums.AttackAbilityNames attack = Enums.AttackAbilityNames.None, bool flip = false, Action onApexAction = null)
    {
       
       

        int i = (int)attack;
        if(i == 0)
            i = UnityEngine.Random.Range(1,4);

        Debug.Log(i);

        CurrentAnimation.SetActive(true);
        CurrentAnimation.GetComponent<SpriteRenderer>().flipX = flip;  

        Animator anim = CurrentAnimation.gameObject.GetComponent<Animator>();

        // anim.SetTrigger("Attack");

        switch(i)
        {
            case 1:    
                anim.SetTrigger("Slash");
                break;
            case 2:
                anim.SetTrigger("Vice");
                break;
            case 3:
                anim.SetTrigger("Sting");                
                break;
            default:
                break;
        }

        LeanTween.delayedCall(0.15f, onApexAction);

        LeanTween.delayedCall(0.30f,()=>{
            CurrentAnimation.SetActive(false);
        });

    }


}
