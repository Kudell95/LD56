using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    public static ModalWindow Main { get; private set; }


    [Header("Title")]
    public TextMeshProUGUI TitleText;
    public GameObject TitlePanel;

    [Header("Horizontal-Content")]
    public GameObject HorizontalContentPanel;
    public TextMeshProUGUI HorizontalText;
    public GameObject HorizontalImage;

    [Header("Vertical-Content")]
    public GameObject VerticalContentPanel;
    public TextMeshProUGUI VerticalText;
    public GameObject VerticalImage;

    [Header("Buttons")]
    public TextMeshProUGUI YesButtonText;
    public TextMeshProUGUI NoButtonText;
    public TextMeshProUGUI AlternateButtonText;
    public GameObject YesButton;
    public GameObject NoButton;
    public GameObject AlternateButton;


    public Action YesAction;
    public Action NoAction;
    public Action AlternateAction;

    public UnityEvent OnModalOpen;
    public UnityEvent OnModalClose;



    private void Awake()
    {
        Main = this;
        gameObject.SetActive(false);
    }


    public void DisplayYesNoScreen(string Title, string Content, Action onYes, Action onNo, Sprite Image = null)
    {
        DisplayMessage("Yes", "No", null, Title, Content, Image, onYes, onNo, null, false);
    }

    public void DisplayYesNoLargeImage(string Title, string Content, Action onYes, Action onNo, Sprite Image = null)
    {
        DisplayMessage("Yes", "No", null, Title, Content, Image, onYes, onNo, null, true);
    }


    public void DisplayConfirmScreen(string title, string content, Action onYesAction = null)
    {
        DisplayMessage("Ok", null, null, title, content, null, onYesAction, null, null, false);
    }



    public void DisplayMessage(string yesText, string noText, string alternateText, string titleText, string ContentText, Sprite Image, Action yesAction, Action noAction, Action alternateAction, bool IsVertical)
    {
        //prepare the screen
        gameObject.SetActive(true);
        OnModalOpen.Invoke();

        if (YesButton.GetComponent<Button>() != null)
        {
            YesButton.GetComponent<Button>().Select();
        }

        if (IsVertical)
        {
            if (Image == null)
                VerticalImage.SetActive(false);
            else
            {
                if (VerticalImage.GetComponent<Image>() != null)
                    VerticalImage.GetComponent<Image>().sprite = Image;

                VerticalImage.SetActive(true);
            }

            VerticalText.text = ContentText;


            VerticalContentPanel.SetActive(true);
            HorizontalContentPanel.SetActive(false);

        }
        else
        {
            if (Image == null)
                HorizontalImage.SetActive(false);
            else
            {
                if (HorizontalImage.GetComponent<Image>() != null)
                    HorizontalImage.GetComponent<Image>().sprite = Image;

                VerticalImage.SetActive(true);
            }

            HorizontalText.text = ContentText;

            VerticalContentPanel.SetActive(false);
            HorizontalContentPanel.SetActive(true);
        }

        TitleText.text = titleText;

        YesButtonText.text = yesText;


        if (!string.IsNullOrEmpty(noText))
        {
            NoButtonText.text = noText;
            NoButton.SetActive(true);
        }
        else
            NoButton.SetActive(false);

        if (!string.IsNullOrEmpty(alternateText))
        {
            AlternateButtonText.text = alternateText;
            AlternateButton.SetActive(true);
        }
        else
            AlternateButton.SetActive(false);

        YesAction = yesAction;
        NoAction = noAction;
        AlternateAction = alternateAction;

    }


    private void Hide()
    {
        OnModalClose.Invoke();
        gameObject.SetActive(false);
    }


    public void OnYes()
    {
        Hide();
        if (YesAction != null)
            YesAction.Invoke();
    }

    public void OnNo()
    {
        Hide();
        if (NoAction != null)
            NoAction.Invoke();
    }

    public void OnAlternate()
    {
        Hide();
        if (AlternateAction != null)
            AlternateAction.Invoke();
    }

}
