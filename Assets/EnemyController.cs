using DG.Tweening;
using NUnit.Framework;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemySO CurrentEnemy;

    //SHould this go on game manager? don't think there's a need.
    public EnemyDB AvailableEnemies;

    public SpriteRenderer EnemySpriteRenderer;

    private Vector3 m_OriginalScale;


    private void Awake() {
        TurnManager.OnTurnEnd += OnEndTurn;
        TurnManager.OnTurnStart += OnStartTurn;
        m_OriginalScale = transform.localScale;
        
    }

    private void Start() {
        transform.DOScaleY(0,0f);
    }

    private void OnStartTurn(Enums.TurnStates turn)
    {
        switch(turn)
        {
            case Enums.TurnStates.OpponentSpawnTurn:
                if(GameManager.Instance?.GameSpawnType == Enums.InsectSpawnTypes.Random)
                    SpawnNewEnemy();
                    return;
            case Enums.TurnStates.OpponentDeadTurn:
                    //Todo: check if sequential and current enemy is boss, if so, skip normal death and display victory screen and bug out (pun fully intended).
                    //otherwise if sequential and !boss OR random, play death animation and trigger shop screen.
                break;
            default:
                return;
        }
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
        //ensure scaled to 0 on z;
        transform.DOScaleY(0,0f).OnComplete(()=>{
            //update image
            EnemySpriteRenderer.sprite = enemy.CharacterSprite;

            transform.DOScaleY(m_OriginalScale.y, 1f).SetEase(Ease.OutBack).OnComplete(()=>{
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

}
