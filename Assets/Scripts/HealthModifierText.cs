using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HealthModifierText : MonoBehaviour
{
	
	public float MaxPositionModifier = 1.0f;
	public float MinPositionModifier = 0.1f;
	
	
	public Color DamageColour;
	public Color HealColour;
	public Color MissColour;
	public Color BlockedColour;
	public Color BuffedColour;

	public Color PoisonDamageColour;

	
	
	public GameObject HealParticlePrefab;
	public GameObject DamageParticlePrefab;
	
	private Vector3 _startPosition;
	
	public GameObject TextPrefab;

	public GameObject PoisonTextPrefab;
	public GameObject BonusDMGTextPrefab;
	
	private void Start()
	{
		_startPosition = transform.position;
	}
	
	public void ShowDamage(DamageInformation damage, int BlockedDamage = 0)
	{
		if(damage.StandardDamage == 0)
		{
			Show(MissColour, "Miss");
		}
		else if(BlockedDamage > 0 && damage.StandardDamage == 0)
		{
			Show(BlockedColour, "Blocked");
		}
		else
		{
			string text = $"-{damage.StandardDamage.ToString()}";
			string bonusText = $"-{damage.BonusDamage.ToString()}";
			string poisonText = $"-{damage.PoisonDamage.ToString()}";

			Show(DamageColour, text);
			
			if(damage.BonusDamage > 0)
				Show(BuffedColour, bonusText);
			
			if(damage.PoisonDamage > 0)
				Show(PoisonDamageColour, poisonText);


			var particle = Instantiate(DamageParticlePrefab,transform);
			Destroy(particle,1.5f);
			if(BlockedDamage > 0)
				Show(BlockedColour,$"Blocked {BlockedDamage}");
		}
	}


	public void ShowDamage(int damage, bool buffedDamage = false, int BlockedDamage = 0)
	{
		if(damage == 0)
		{
			Show(MissColour, "Miss");
		}
		else if(BlockedDamage > 0 && damage == 0)
		{
			Show(BlockedColour, "Blocked");
		}
		else
		{
			string text = $"-{damage.ToString()}";
			if(buffedDamage)
				Show(BuffedColour, text);
			else
				Show(DamageColour, text);
			var particle = Instantiate(DamageParticlePrefab,transform);
			Destroy(particle,1.5f);
			if(BlockedDamage > 0)
				Show(BlockedColour,$"Blocked {BlockedDamage}");
		}
	}
	
	public void ShowHeal(int heal)
	{
		string text = $"+{heal.ToString()}";
		Show(HealColour, text);
		var particle = Instantiate(HealParticlePrefab,transform);
		Destroy(particle,1.5f);
	}
	
	
	public void Show(Color color, string text)
	{
		var textObject = Instantiate(TextPrefab,transform);
		textObject.transform.position = _startPosition;
		TMP_Text _text = textObject.GetComponent<TMP_Text>();
		_text.transform.position = new Vector3(_startPosition.x + Random.Range(MinPositionModifier, MaxPositionModifier), _startPosition.y + Random.Range(MinPositionModifier, MaxPositionModifier), _startPosition.z);
		_text.color = color;
		_text.text = text;
		
		_text.DOFade(1, 0.01f);
		_text.transform.DOLocalMoveY(1,5);
		LeanTween.delayedCall(0.5f, ()=>
		{
			Hide(_text);
		});
	}
	
	
	public void Hide(TMP_Text _text)
	{
		_text.DOFade(0, 0.5f).OnComplete(()=>
		{
			_text.transform.DOKill();
			_text.transform.position = _startPosition;
			Destroy(_text.gameObject);
		});
	}
	
}
