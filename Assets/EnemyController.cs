using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public EnemySO CurrentEnemy;


    private int m_CurrentHealth;

    public int CurrentHealth {get { return m_CurrentHealth;}
    set{ 
        m_CurrentHealth = value;      
        UpdateHealthVisuals();
    }}
    private int MaxHealth;

    //SHould this go on game manager? don't think there's a need.
    public EnemyDB AvailableEnemies;

    public SpriteRenderer EnemySpriteRenderer;

    private Vector3 m_OriginalScale;

    public Slider HealthSlider;

    public AnimationHelpers AnimationHelper;

    public HealthModifierText HealthText;

    public Transform EnemyAttackAnimationPoint;

    public Vector3 OriginalPosition;

    public GameObject HealthSliderParent;
    public TextMeshProUGUI HealthTextUGUI;


    

    private void Awake() {
        TurnManager.OnTurnEnd += OnEndTurn;
        TurnManager.OnTurnStart += OnStartTurn;
        m_OriginalScale = transform.localScale;
        
    }

    private void Start() {
        transform.DOScaleY(0,0f);
        OriginalPosition = transform.position;
        HealthSliderParent.GetComponent<CanvasGroup>().DOFade(0,0);
    }


    public void UpdateHealthVisuals()
    {
            float perc = (float)CurrentHealth / (float)MaxHealth;

            HealthSlider.value = perc;

            HealthTextUGUI.text = CurrentHealth.ToString();
    }

    private void OnStartTurn(Enums.TurnStates turn)
    {
        switch(turn)
        {
            case Enums.TurnStates.OpponentSpawnTurn:
                if(GameManager.Instance?.GameSpawnType == Enums.InsectSpawnTypes.Random)
                    LeanTween.delayedCall(1f,()=>{
                        SpawnNewEnemy();
                    });
                else{
                    SpawnNewEnemy(GameManager.Instance.GetNextEnemy());
                }   
                    return;
            case Enums.TurnStates.OpponentDeadTurn:
                    //Todo: check if sequential and current enemy is boss, if so, skip normal death and display victory screen and bug out (pun fully intended).
                    //otherwise if sequential and !boss OR random, play death animation and trigger shop screen.

                    if(GameManager.Instance.GameSpawnType == Enums.InsectSpawnTypes.Sequential && GameManager.Instance.LastLinearEnemy)
                    {
                        GameManager.Instance.GameSpawnType = Enums.InsectSpawnTypes.Random;

                        //todo: show screen instead.
                    }

                    
                    LeanTween.delayedCall(2.2f, ()=>{
                        GameManager.EndTurn();
                    });



                break;
            case Enums.TurnStates.OpponentTurn:
                //TODO: add turn logic.
                LeanTween.delayedCall(0.5f,()=>{
                    PerformTurn();
                });
                


                break;
            default:
                return;
        }
    }


    public void PerformTurn()
    {
        var ability = CurrentEnemy.GetRandomAbility();
        switch(ability.AbilityCategory)
        {
            case Enums.OpponentAbilityCategory.Attack:
                LeanTween.delayedCall(0.5f,()=>{
                    transform.DOMove(EnemyAttackAnimationPoint.position,0.1f).SetEase(Ease.OutElastic).OnComplete(()=>{
                        DamageInformation dmginfo = GetDamage(ability);
                        
                        AttackAnimationController.Instance.PlayAttackAnimation(true, ()=>{
                            GameManager.Instance.PlayerObject.TakeDamage(dmginfo.StandardDamage, dmginfo.PoisonDamage, dmginfo.BonusDamage);
                        });
                        
                        transform.DOMove(OriginalPosition, 0.2f).SetEase(Ease.OutQuart).OnComplete(()=>{
                            LeanTween.delayedCall(1.4f, ()=>{
                                //In case we've killed the enemy and moved on
                                if(GameManager.Instance.IsOpponentTurn)
                                    GameManager.EndTurn();
                            });
                        });
                    });
                });
                break;

            case Enums.OpponentAbilityCategory.Heal:
                break;
        }
    }


    public DamageInformation GetDamage(EnemyAbilitySO ability)
    {
        DamageInformation dmginfo = new DamageInformation();
        dmginfo.StandardDamage = ability.value;

        return dmginfo;

    }


    /// <summary>
    /// returns true if enemy dead
    /// </summary>
    /// <param name="standardDamage"></param>
    /// <param name="poisionDamage"></param>
    /// <param name="bonusDamage"></param>
    /// <returns></returns>
    public bool TakeDamage(int standardDamage, int poisionDamage, int bonusDamage)
    {
        int totalDmg = standardDamage + poisionDamage + bonusDamage;

        if(totalDmg == 0)
			AnimationHelper.OnHit(transform,EnemySpriteRenderer,Color.white);
		else
			AnimationHelper.OnHit(transform,EnemySpriteRenderer);

       

        if((CurrentHealth - totalDmg) <= 0)
        {
            HealthText.ShowDamage(CurrentHealth);
            CurrentHealth = 0;
            //TODO: DIE
            HealthSlider.value = 0;
            Die();

            return true;
        }
        else
        {
            HealthText.ShowDamage(totalDmg);
            CurrentHealth -= totalDmg;

        }


        return false;
    }

    private void OnEndTurn(Enums.TurnStates turn)
    {
        switch(turn)
        {
            default:
                return;
        }
    }

    public void SpawnNewEnemy()
    {
        SpawnNewEnemy(GetRandomEnemy());
    }

    public void SpawnNewEnemy(EnemySO enemy)
    {   

        CurrentEnemy = enemy;
        MaxHealth = enemy.Health;
        CurrentHealth = MaxHealth;
        //ensure scaled to 0 on z;
        transform.DOScaleY(0,0f).OnComplete(()=>{
            //update image
            EnemySpriteRenderer.sprite = enemy.CharacterSprite;

            HealthSliderParent.GetComponent<CanvasGroup>().DOFade(1,1f);
            transform.DOScale(m_OriginalScale, 1f).SetEase(Ease.OutBack).OnComplete(()=>{
                
                GameManager.EndTurn();
            });
        });
    }

    //accessor method for the enemy db
    public EnemySO GetRandomEnemy()
    {
        if(AvailableEnemies == null || AvailableEnemies.EnemyList == null || AvailableEnemies.EnemyList.Count == 0)
            return null;

        return AvailableEnemies.GetRandomEnemy();
    }


    private void OnDestroy() {
        TurnManager.OnTurnEnd -= OnEndTurn;
        TurnManager.OnTurnStart -= OnStartTurn;
    }

    public void Die()
    {
        SoundManager.Instance.PlaySound("EnemyDeath");
		// TurnBasedManager.Instance.StartTurn(Enums.TurnStates.PlayerDeadTurn,false,false);
        
        HealthSliderParent.GetComponent<CanvasGroup>().DOFade(0,1f);
		transform.DOShakeScale(1f).OnComplete(()=>
		{
			transform.DOScaleX(0,0.2f).OnComplete(()=>
			{
				GameManager.StartTurn(Enums.TurnStates.OpponentDeadTurn);
			});
		});		
    }
    

}
