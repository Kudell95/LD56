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

    private void Start() 
    {
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
                   if(GameManager.Instance.GameSpawnType == Enums.InsectSpawnTypes.Random && ((GameManager.LevelCount + 1) - GameManager.Instance.LinearEnemyCount) % 4 == 0)
                    {
                        if(GameManager.DifficultyModifier + GameManager.Config.DifficultyIncrement > GameManager.Config.MaxDifficultyModifier)
                            GameManager.DifficultyModifier = GameManager.Config.MaxDifficultyModifier;
                        else
                            GameManager.DifficultyModifier += GameManager.Config.DifficultyIncrement;                            
                    }


                   if(GameManager.LevelCount != 0 && (GameManager.LevelCount + 1) % 2 == 0 && !GameManager.Instance.LastLinearEnemy)
                    {   
                        ShopController.Instance.Show();

                    }
                    else if(GameManager.Instance.GameSpawnType == Enums.InsectSpawnTypes.Sequential && GameManager.Instance.LastLinearEnemy)
                    {
                        GameManager.Instance.GameSpawnType = Enums.InsectSpawnTypes.Random;

                        GameManager.Instance.VictoryScreen.Show();
                    }
                    else
                    {                        
                            LeanTween.delayedCall(2.2f, ()=>{
                                GameManager.EndTurn();
                            });
                    }

                    



                break;
            case Enums.TurnStates.OpponentTurn:

                if(GameManager.SkipNextOpponentTurn){
                    LeanTween.delayedCall(1.2f, ()=>{
                                GameManager.SkipNextOpponentTurn = false;
                                GameManager.EndTurn();
                            });

                    return;
                }
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
                SoundManager.Instance?.PlaySound("attack");

                LeanTween.delayedCall(0.5f,()=>{
                    transform.DOMove(EnemyAttackAnimationPoint.position,0.1f).SetEase(Ease.OutElastic).OnComplete(()=>{
                        DamageInformation dmginfo = GetDamage(ability);
                        
                        AttackAnimationController.Instance.PlayAttackAnimation(Enums.AttackAbilityNames.None,true, ()=>{
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

        if(GameManager.DifficultyModifier > 0)
            dmginfo.StandardDamage += Mathf.CeilToInt(dmginfo.StandardDamage * GameManager.DifficultyModifier); 


        float percentageMiss = GameManager.Instance.PlayerObject.GetDodgePercentage();

        dmginfo.StandardDamage = GameManager.Instance.PlayerObject.GetReducedDamage(dmginfo.StandardDamage);

        if(UnityEngine.Random.Range(0f, 100f) <= percentageMiss)
        {
            dmginfo.StandardDamage = 0;
        }

        return dmginfo;

    }


    /// <summary>
    /// returns true if enemy dead
    /// </summary>
    /// <param name="standardDamage"></param>
    /// <param name="poisionDamage"></param>
    /// <param name="bonusDamage"></param>
    /// <returns></returns>
    public bool TakeDamage(DamageInformation damageInformation)
    {
        if(damageInformation == null)
            return false;

        int totalDmg = damageInformation.StandardDamage + damageInformation.PoisonDamage + damageInformation.BonusDamage;

        SoundManager.Instance?.PlaySound("take damage");

        Debug.Log("total damge dealt: " + totalDmg.ToString());

        if(totalDmg == 0)
			AnimationHelper.OnHit(transform,EnemySpriteRenderer,Color.white);
		else
			AnimationHelper.OnHit(transform,EnemySpriteRenderer);

       
        if(damageInformation.SkipTurn)
            GameManager.SkipNextOpponentTurn = true;

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
            HealthText.ShowDamage(damageInformation);
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
        GameManager.SkipNextOpponentTurn = false;
        CurrentEnemy = enemy;
        MaxHealth = enemy.Health;
        if(GameManager.DifficultyModifier > 0)
        {
            MaxHealth += Mathf.CeilToInt(MaxHealth * GameManager.DifficultyModifier);
        }

        CurrentHealth = MaxHealth;
        //ensure scaled to 0 on z;
        transform.DOScaleY(0,0f).OnComplete(()=>{
            //update image
            EnemySpriteRenderer.sprite = enemy.CharacterSprite;
            EnemySpriteRenderer.gameObject.GetComponent<Animator>().runtimeAnimatorController = enemy.CharacterAnimator; 
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
        SoundManager.Instance?.PlaySound("enemy death");

		// TurnBasedManager.Instance.StartTurn(Enums.TurnStates.PlayerDeadTurn,false,false);
        
        HealthSliderParent.GetComponent<CanvasGroup>().DOFade(0,1f);
		transform.DOShakeScale(1f).OnComplete(()=>
		{
			transform.DOScaleX(0,0.2f).OnComplete(()=>
			{
                bool notify = (GameManager.LevelCount == 0 || (GameManager.LevelCount + 1) % 2 != 0) && !GameManager.Instance.LastLinearEnemy;
				GameManager.StartTurn(Enums.TurnStates.OpponentDeadTurn, notify);
			});
		});		
    }
    

}
