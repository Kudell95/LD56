
using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Player : MonoBehaviour
{
    private int m_CurrentHealth;

    public int CurrentHealth {get{
        return m_CurrentHealth;
    }
    set{
        m_CurrentHealth = value;
        UpdateHealthVisuals();
    }}

    private int CurrentMaxHealth;


    public List<PlayerAbilitySO> AbilityList = new List<PlayerAbilitySO>();

    public List<PlayerAbilitySO> ItemList = new List<PlayerAbilitySO>();
    public bool Dead;

    public static Action<PlayerAbilitySO> OnUseAbility;

    [HideInInspector]
    public Vector3 StartingPosition;

    public Transform PlayerAttackAnimationPoint;

    public SpriteRenderer PlayerSpriteRenderer;

    public AnimationHelpers AnimationHelper;
    public HealthModifierText HealthText;
    public Slider HealthSlider;

    public TextMeshProUGUI tmpHealthText;

    public bool HasTempModifiers {get{
        return CurrentModifiers.Count(x=>x.TemporaryModifier) > 0; 
    }}

    // public static Action<List<Modifier>> ModifierAdded;
    // public static Action<List<Modifier>> ModifierRemoved;

    public static Action<List<Modifier>> ModfiierUpdated;

    public List<Modifier> CurrentModifiers = new List<Modifier>();



    private void Awake()
    {
        TurnManager.OnTurnStart += OnTurnStarted;
        TurnManager.OnTurnEnd += OnTurnEnded;
        OnUseAbility += onUseAbility;
        StartingPosition = transform.position;
    }

    public void onUseAbility(PlayerAbilitySO ability)
    {
        TweenCallback FinalAction = ()=>{
            LeanTween.delayedCall(0.1f,()=>{
                if(GameManager.Instance.IsPlayerTurn)
                    GameManager.EndTurn();
            });
        };

        switch(ability.AbilityType)
        {
            case Enums.AbilityType.Attack:
            case Enums.AbilityType.AttackRange:
                PerformAttack(ability, FinalAction);
                break;
            case Enums.AbilityType.Heal:
                PerformHeal(ability, FinalAction);
                GameManager.ItemUsedThisRound = true;
                break;
            case Enums.AbilityType.DefenceBuff:
                PerformDefenceBuff(ability, FinalAction);
                GameManager.ItemUsedThisRound = true;
                break;
            case Enums.AbilityType.AttackBuff:
                PerformAttackBuff(ability, FinalAction);
                GameManager.ItemUsedThisRound = true;
                break;            
        }
        Debug.Log(ability.AbilityType + " " + ability.Name);
        
    }

    public void PerformHeal(PlayerAbilitySO ability, TweenCallback onComplete)
    {
        if(ability.AbilityType != Enums.AbilityType.Heal)
            return;

        if(GameManager.Instance.HealthCount <= 0)
            return;

        float healperc = (float)ability.Amount / 100;


        int healAmount = Mathf.FloorToInt(CurrentMaxHealth  * healperc);
         LeanTween.delayedCall(0.5f,()=>{            
            
            if(CurrentHealth + healAmount >= CurrentMaxHealth){
                HealthText.ShowHeal(CurrentMaxHealth - CurrentHealth);  
                CurrentHealth = CurrentMaxHealth;
            }
            else
            {
                CurrentHealth += healAmount;
                HealthText.ShowHeal(healAmount);  
            }
            GameManager.Instance.HealthCount--;            
        });
    }
    
    public void PerformAttackBuff(PlayerAbilitySO ability, TweenCallback onComplete)
    {
        Modifier mod = new Modifier();
        mod.Amount = ability.Amount;
        mod.ModifierType = Enums.ModifierTypes.AttackBoost;
        mod.TemporaryModifier = true;
        mod.Description = ability.Description;
        mod.EffectDescription = ability.EffectDescription;
        mod.Name = ability.Name;
        mod.ModifierIcon = ability.ModifierImage;
        GameManager.Instance.AttackBuffCount--;
        AddModifier(mod);
        Debug.Log("Buffing Attack");
    }

    public void PerformDefenceBuff(PlayerAbilitySO ability, TweenCallback onComplete)
    {
        Modifier mod = new Modifier();
        mod.Amount = ability.Amount;
        mod.ModifierType = Enums.ModifierTypes.DefenceBoost;
        mod.TemporaryModifier = true;
        mod.Description = ability.Description;
        mod.EffectDescription = ability.EffectDescription;
        mod.Name = ability.Name;
        mod.ModifierIcon = ability.ModifierImage;
        GameManager.Instance.DefenseBuffCount--;
        AddModifier(mod);
    }

    public void PerformAttack(PlayerAbilitySO abilitySO, TweenCallback onComplete)
    {
        LeanTween.delayedCall(0.5f,()=>{
            transform.DOMove(PlayerAttackAnimationPoint.position,0.1f).SetEase(Ease.OutElastic).OnComplete(()=>{
                DamageInformation dmginfo = GetDamage(abilitySO);
                
                AttackAnimationController.Instance.PlayAttackAnimation(false, ()=>{
                    if(!GameManager.Instance.EnemyObject.TakeDamage(dmginfo.StandardDamage, dmginfo.PoisonDamage, dmginfo.BonusDamage)){
                        LeanTween.delayedCall(1.2f, ()=>{
                            onComplete?.Invoke();
                        });
                    }
                });
                
                transform.DOMove(StartingPosition, 0.2f).SetEase(Ease.OutQuart);
            });
        });
    }   
    
    public void RemoveTemporaryModifiers()
    {
        for(int i = CurrentModifiers.Count - 1; i >= 0; i--)
        {
            if(CurrentModifiers[i].TemporaryModifier){

                CurrentModifiers.RemoveAt(i);
                Debug.Log("removing modifier at " + i);
            }
        }

        ModfiierUpdated?.Invoke(CurrentModifiers);
    }

    public void TakeDamage(int standardDamage, int poisonDamage, int bonusDamage)
    {
        int totalDmg = standardDamage + poisonDamage + bonusDamage;

        if(totalDmg == 0)
			AnimationHelper.OnHit(transform,PlayerSpriteRenderer,Color.white);
		else
			AnimationHelper.OnHit(transform,PlayerSpriteRenderer);

       

        if((CurrentHealth - totalDmg) <= 0)
        {
            HealthText.ShowDamage(CurrentHealth);
            CurrentHealth = 0;
            Die();

            return;
        }
        else
        {
            HealthText.ShowDamage(totalDmg);
            CurrentHealth -= totalDmg;


        }
    }


    public void UpdateHealthVisuals()
    {
            float perc = (float)CurrentHealth / (float)CurrentMaxHealth;

            HealthSlider.value = perc;

            tmpHealthText.text = CurrentHealth.ToString();
    }


    public DamageInformation GetDamage(PlayerAbilitySO ability)
    {
        DamageInformation damage = new DamageInformation();
        if(ability.UseRange)
        {
            damage.StandardDamage = UnityEngine.Random.Range(ability.MinAmount, ability.MaxAmount + 1);
        }
        else
        {
            damage.StandardDamage = ability.Amount;
        }

        //TODO: retrieve bonus damage here, apply to standard + add in poison damage.

        return damage;
    }





    private void OnTurnStarted(Enums.TurnStates turnType)
    {
        if(turnType == Enums.TurnStates.OpponentDeadTurn)
        {
            RemoveTemporaryModifiers();

            return;
        }


        if(turnType != Enums.TurnStates.InitialTurn && turnType != Enums.TurnStates.PlayerTurn)
            return;


        switch(turnType)
        {
            case Enums.TurnStates.InitialTurn:
            {
                InitPlayer();
                return;
            }
        }




    }

    private void OnTurnEnded(Enums.TurnStates turnType)
    {
        GameManager.ItemUsedThisRound = false;
        
    }

    //this will probably go in an event somewhere.
    public void InitPlayer()
    {
        CurrentHealth = GameManager.Config.StartingPlayerHealth;
        CurrentMaxHealth = GameManager.Config.StartingPlayerHealth;
        Dead = false;
        OnSpawn(()=>{
            GameManager.Instance.TurnManagerObject.EndTurn();
        });
    }


    public void OnSpawn(TweenCallback oncomplete)
    {
        float startingScaleY = transform.localScale.y;
        transform.DOScaleY(0,0f).OnComplete(()=>{
            transform.DOScaleY(startingScaleY,1f).SetEase(Ease.OutBack).OnComplete(oncomplete);
        });
    }



    private void OnDestroy() 
    {
        TurnManager.OnTurnStart -= OnTurnStarted;
        TurnManager.OnTurnEnd -= OnTurnEnded;    
        OnUseAbility -= onUseAbility;
    }


    public void AddModifier(Modifier modifier)
    {
        CurrentModifiers.Add(modifier);
        ModfiierUpdated?.Invoke(CurrentModifiers);

    }

    public void RemoveModifier(Modifier modifier)
    {
        CurrentModifiers.Remove(modifier);   
        ModfiierUpdated?.Invoke(CurrentModifiers);
    }

    public void RemoveModifier(int index)
    {
        CurrentModifiers.RemoveAt(index);   
        ModfiierUpdated?.Invoke(CurrentModifiers);
    }

    private void Die(){
        SoundManager.Instance.PlaySound("EnemyDeath");
		// TurnBasedManager.Instance.StartTurn(Enums.TurnStates.PlayerDeadTurn,false,false);
		GameManager.StartTurn(Enums.TurnStates.PlayerDeadTurn, false);

		transform.DOShakeScale(1f).OnComplete(()=>
		{
			transform.DOScaleX(0,0.2f).OnComplete(()=>
			{
                //show death screen...
			});
		});		
    }
    



}


public class DamageInformation{
    public int StandardDamage;
    public int PoisonDamage;
    public int BonusDamage;
}