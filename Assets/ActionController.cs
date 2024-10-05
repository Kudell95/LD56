using DG.Tweening;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public Transform ActionItemParent;
    public GameObject Panel;

    private CanvasGroup m_CG;

    public bool Active;

    private bool fading;

    private void Awake() {
        m_CG = Panel.GetComponent<CanvasGroup>();
        ForceClosed();
    }

    public void ClearActionItems()
    {
        foreach(Transform child in ActionItemParent){
            DestroyImmediate(child.gameObject);
        }
    }

    public void PopulateActionItems()
    {

    }


    public void ShowPanel()
    {
        if(fading)
            return;
        // Panel.SetActive(true);

        fading = true;
        Active = true;
        m_CG.DOFade(0,0).OnComplete(()=>{
            m_CG.DOFade(1,0.2f).OnComplete(()=>{
                m_CG.interactable = true;
                m_CG.blocksRaycasts = true;
                fading = false;
            });
        });
    }

    public void HidePanel()
    {
        if(fading)
            return;

        fading = true;
        Active = false;

        m_CG.interactable = false;
        m_CG.blocksRaycasts = false;
        Panel.GetComponent<CanvasGroup>().DOFade(1,0).OnComplete(()=>{
            Panel.GetComponent<CanvasGroup>().DOFade(0,0.2f).OnComplete(()=>{
                ClearActionItems(); 
                fading = false;
            });
        });

    }


    public void ForceClosed(){
        ClearActionItems();
        m_CG.alpha = 0;
        m_CG.interactable = false;
        m_CG.blocksRaycasts = false;
        Active = false;
        fading = false;
    }

    public void TogglePanel(){
        if(Panel.activeSelf)
            HidePanel();
        else
            ShowPanel();
    }
    


}
