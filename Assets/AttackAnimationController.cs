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
    public void PlayAttackAnimation(bool flip = false, Action onApexAction = null)
    {
        CurrentAnimation.SetActive(true);
        CurrentAnimation.GetComponent<SpriteRenderer>().flipX = flip;   

        Animator anim = CurrentAnimation.gameObject.GetComponent<Animator>();
        Animation animclip = CurrentAnimation.gameObject.GetComponent<Animation>();

        anim.SetTrigger("Attack");

        LeanTween.delayedCall(animclip.clip.length/2, onApexAction);

        LeanTween.delayedCall(animclip.clip.length,()=>{
            CurrentAnimation.SetActive(false);
        });

    }


}
