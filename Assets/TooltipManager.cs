using DG.Tweening;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{   
    public static TooltipManager Instance;

    public TextMeshProUGUI TitleText;
    
    public TextMeshProUGUI DescriptionText;

    private CanvasGroup _cg;

    private void Start() {
        Cursor.visible = true;
        _cg = GetComponent<CanvasGroup>();
        _cg.alpha = 0;
    }

    private void Update() {
        transform.position = Input.mousePosition;
    }

    public void ShowTooltip(string title, string message)
    {
        // gameObject.SetActive(true);
        TitleText.text = title;
        DescriptionText.text = message;
        _cg.DOFade(1,0.3f);
    }

    public void HideTooltip()
    {       
        _cg.DOFade(0,0.3f);
        TitleText.text = "";
        DescriptionText.text = "";
    }



    private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

}
