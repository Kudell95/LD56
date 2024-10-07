
using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using System.ComponentModel;

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
                    if(!GameManager.Instance.EnemyObject.TakeDamage(dmginfo)){
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

        damage.PoisonDamage = GetPoisonDamage(ability.AttackAbilityName);

        damage.BonusDamage = GetBonusDamage(damage.StandardDamage);

        HandleLifeSteal(ability.AttackAbilityName);

        if(ability.AttackAbilityName == Enums.AttackAbilityNames.Vice)
        {
            float roll = UnityEngine.Random.Range(0f,100f);
            Debug.Log(roll);
            if(roll <= 40f)
            {
                damage.SkipTurn = true;
            }
        }

        return damage;
    }


    public int GetBonusDamage(int damage)
    {
        if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.AttackBoost) == 0)
            return 0;

        float totalPerc = 0;

        foreach(Modifier mod in CurrentModifiers)
        {
            if(mod.ModifierType != Enums.ModifierTypes.AttackBoost)
                continue;


            totalPerc += (float)mod.Amount;
        }  

        float perc = totalPerc / 100;

        int bonusDamage =  Mathf.CeilToInt(damage * perc);

        return bonusDamage;
    }



    public float GetDodgePercentage()
    {
        if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.DodgePercentage) == 0)
            return 5;


        float perc = 0;

        foreach(Modifier modifier in CurrentModifiers)
        {
            if(modifier.ModifierType != Enums.ModifierTypes.DodgePercentage)
                continue;

            perc += (float)modifier.Amount;
        }

        if(perc < 5)
            perc += 5;

        if(perc > 40f)
            perc = 40f;

        
        return perc;

    }


    public int GetReducedDamage(int damage)
    {
        
        if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.DodgePercentage) == 0)
            return damage;


        float perc = 0;

        foreach(Modifier modifier in CurrentModifiers)
        {
            if(modifier.ModifierType != Enums.ModifierTypes.DodgePercentage)
                continue;

            perc += (float)modifier.Amount / 100;
        }

        if(perc > 0.5f)
            perc = 0.5f;

        damage -= Mathf.FloorToInt(damage * perc);
        return damage <= 0 ? 0 : damage;
    }


    public void HandleHealAfterFight()
    {
        if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.HealAfterFight) == 0)
            return;

        if(CurrentHealth >= CurrentMaxHealth)
            return;

        float totalPerc = 0;
        foreach(Modifier modifier in CurrentModifiers)
        {
            if(modifier.ModifierType != Enums.ModifierTypes.HealAfterFight)
                continue;

            totalPerc += (float)modifier.Amount;
        }


        if(totalPerc == 0)
            return;

        float perc = totalPerc/100;

        int healAmount = Mathf.CeilToInt(CurrentMaxHealth  * perc);
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
        });
    }


    public int GetPoisonDamage(Enums.AttackAbilityNames abilityName)
    {
        int dmg = 0;
        if(abilityName == Enums.AttackAbilityNames.None)
            return 0;


        switch(abilityName)
        {
            case Enums.AttackAbilityNames.Slash:
            {
                if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.SlashPoisonDamage) == 0)
                    return 0;
                
                foreach(Modifier modifier in CurrentModifiers)
                {
                    if(modifier.ModifierType != Enums.ModifierTypes.SlashPoisonDamage)
                        continue;
                    if(!modifier.UseRange)
                        dmg += modifier.Amount;
                    else
                        dmg += UnityEngine.Random.Range(modifier.MinAmount, modifier.MaxAmount + 1);
                }

                return dmg;
            }
            case Enums.AttackAbilityNames.Sting:
            {
                if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.StingPoisonDamage) > 0)
                {
                
                    foreach(Modifier modifier in CurrentModifiers)
                    {
                        if(modifier.ModifierType != Enums.ModifierTypes.StingPoisonDamage)
                            continue;
                        if(!modifier.UseRange)
                            dmg += modifier.Amount;
                        else
                            dmg += UnityEngine.Random.Range(modifier.MinAmount, modifier.MaxAmount + 1);
                    }
                }

                float roll = UnityEngine.Random.Range(0,100);
                Debug.Log(roll);

                if(roll <= 40f)
                {
                    dmg += 3;
                }

                return dmg;
            }
            case Enums.AttackAbilityNames.Vice:
            {
                if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.VicePoisonDamage) == 0)
                    return 0;
                
                foreach(Modifier modifier in CurrentModifiers)
                {
                    if(modifier.ModifierType != Enums.ModifierTypes.VicePoisonDamage)
                        continue;
                    if(!modifier.UseRange)
                        dmg += modifier.Amount;
                    else
                        dmg += UnityEngine.Random.Range(modifier.MinAmount, modifier.MaxAmount + 1);
                }

                return dmg;
            }



        }


        return dmg;
    }

    public void HandleLifeSteal(Enums.AttackAbilityNames abilityName)
    {
        if(abilityName == Enums.AttackAbilityNames.None)
            return;

        switch(abilityName)
        {
            case Enums.AttackAbilityNames.Slash:
            {
                if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.SlashLifeSteal) == 0)
                    return;

                if(CurrentHealth >= CurrentMaxHealth)
                    return;

                float totalPerc = 0;
                foreach(Modifier modifier in CurrentModifiers)
                {
                    if(modifier.ModifierType != Enums.ModifierTypes.SlashLifeSteal)
                        continue;

                    totalPerc += modifier.Amount;
                }


                if(totalPerc == 0)
                    return;

                float perc = totalPerc/100;

                int healAmount = Mathf.FloorToInt(CurrentMaxHealth  * perc);
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
                });
                break;
            }
            case Enums.AttackAbilityNames.Vice:
            {
                if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.ViceLifeSteal) == 0)
                    return;

                if(CurrentHealth >= CurrentMaxHealth)
                    return;

                float totalPerc = 0;
                foreach(Modifier modifier in CurrentModifiers)
                {
                    if(modifier.ModifierType != Enums.ModifierTypes.ViceLifeSteal)
                        continue;

                    totalPerc += modifier.Amount;
                }


                if(totalPerc == 0)
                    return;

                float perc = totalPerc/100;

                int healAmount = Mathf.FloorToInt(CurrentMaxHealth  * perc);
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
                });
                break;
            }
            case Enums.AttackAbilityNames.Sting:
            {
                if(CurrentModifiers.Count(x=> x.ModifierType == Enums.ModifierTypes.StingLifeSteal) == 0)
                    return;

                if(CurrentHealth >= CurrentMaxHealth)
                    return;

                float totalPerc = 0;
                foreach(Modifier modifier in CurrentModifiers)
                {
                    if(modifier.ModifierType != Enums.ModifierTypes.StingLifeSteal)
                        continue;

                    totalPerc += modifier.Amount;
                }


                if(totalPerc == 0)
                    return;

                float perc = totalPerc/100;

                int healAmount = Mathf.FloorToInt(CurrentMaxHealth  * perc);
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
                });
                break;
            }
        }
    }


    private void OnTurnStarted(Enums.TurnStates turnType)
    {
        if(turnType == Enums.TurnStates.OpponentDeadTurn)
        {
            RemoveTemporaryModifiers();
            HandleHealAfterFight();

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

    public void HealToMaxHealth(){
         HealthText.ShowHeal(CurrentMaxHealth - CurrentHealth);  
         CurrentHealth = CurrentMaxHealth;
    }


    public void HandleMaxLifeModifier(Modifier modifier)
    {
        float perc = (float)modifier.Amount / 100;

        int health = 0;

        health = Mathf.FloorToInt(CurrentMaxHealth * perc);

        CurrentMaxHealth += health;

        UpdateHealthVisuals();
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
                GameManager.Instance.DeathUI.Show();
			});
		});		
    }


    
    public void UseShopItem(ShopItemSO item)
    {
        //add modifier OR add 

        switch(item.ShopItemType)
        {
            case Enums.ShopItemType.Item:
                switch(item.ModifierType)
                {
                    case Enums.ModifierTypes.Heal:
                        GameManager.Instance.HealthCount++;
                        break;
                    case Enums.ModifierTypes.AttackBoost:
                        GameManager.Instance.AttackBuffCount++;
                        break;
                    case Enums.ModifierTypes.DefenceBoost:
                        GameManager.Instance.DefenseBuffCount++;
                        break;
                }
                
            break;
            case Enums.ShopItemType.Modifier:
            {
                Modifier modifier = BuildModifier(item); 
                AddModifier(modifier);

                if(modifier.ModifierType == Enums.ModifierTypes.MaxHealth)
                {
                    HandleMaxLifeModifier(modifier);
                }
                
                break;
            }
        }
    }


    public Modifier BuildModifier(ShopItemSO shopItem)
    {
        Modifier modifier = new Modifier();
        modifier.Amount = shopItem.Amount;
        modifier.Description = shopItem.Description;
        modifier.Name = shopItem.Name;
        modifier.EffectDescription = shopItem.EffectDescription;
        modifier.UseRange = shopItem.UseRange;
        modifier.MinAmount = shopItem.MinAmount;
        modifier.MaxAmount = shopItem.MaxAmount;
        modifier.ModifierType = shopItem.ModifierType;
        modifier.ModifierIcon = shopItem.ModifierImage;

        return modifier;
    }



}


public class DamageInformation{
    public int StandardDamage;
    public int PoisonDamage;
    public int BonusDamage;
    public bool SkipTurn;
}