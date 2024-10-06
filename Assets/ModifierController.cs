using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ModifierController : MonoBehaviour
{
    public GameObject ModifierIconPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.ModfiierUpdated += OnModifierUpdated;
    }


    public void OnModifierUpdated(List<Modifier> modifiers)
    {
        ClearIcons();

        foreach(Modifier modifier in modifiers)
        {
            GameObject obj = Instantiate(ModifierIconPrefab, transform);

            if(modifier.ModifierIcon != null)
                obj.GetComponent<Image>().sprite = modifier.ModifierIcon;

            Tooltip tool = obj.GetComponent<Tooltip>();

            tool.Title = modifier.Name;
            tool.Description = modifier.Description + "\n" + modifier.EffectDescription;
        }
        
    }


    public void ClearIcons()
    {
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
    }
    
    private void OnDestroy() {
        Player.ModfiierUpdated -= OnModifierUpdated;
    }

}
