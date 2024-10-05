using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationMessage : MonoBehaviour
{
    public TextMeshProUGUI NotificationText;
    public Image NotificationImage;
    public Sprite NormalExclamationMark;
    public Sprite UrgentExclamationMark;
    public GameObject DismissButton;
    public AudioClip NormalNotificationSound;
    public AudioClip UrgentNotificationSound;

    public float Opacity = 1;

    float timer;
    bool deleting;

    public void RemoveNotification()
    {
        deleting = true;
        HideMessage();
    }

    public void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= NotificationManager.Instance.NotificationDeletionTime && !deleting)
        {
            RemoveNotification();
        }
    }


    public void Message(string message, bool urgent)
    {
        if (urgent)
            UrgentMessage(message);
        else
            NormalMessage(message);
    }

    public void UrgentMessage(string Message)
    {
        NotificationText.text = Message;
        NotificationImage.sprite = UrgentExclamationMark;
    }

    public void NormalMessage(string Message)
    {
        NotificationText.text = Message;
        NotificationImage.sprite = NormalExclamationMark;
    }


    public void HideMessage()
    {
        NotificationText.gameObject.SetActive(false);
        NotificationImage.gameObject.SetActive(false);
        DismissButton.SetActive(false);
        GetComponent<Image>().color = new Color(0, 0, 0, 0);

        LeanTween.size(GetComponent<RectTransform>(), new Vector2(775, 0), 0.2f).setOnComplete(() =>
        {
            LeanTween.cancel(this.gameObject);
            Destroy(this.gameObject);
        });


    }


}
