using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        float y =  transform.localScale.y;

        transform.DOScaleY(0,0).OnComplete(()=>{
            transform.DOScaleY(y,0.3f);
        });


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
    }

    public void NormalMessage(string Message)
    {
        NotificationText.text = Message;
    }


    public void HideMessage()
    {
        transform.DOScaleY(0,0.3f).OnComplete(()=>{
            NotificationText.gameObject.SetActive(false);
            // NotificationImage.gameObject.SetActive(false);
            Destroy(this.gameObject);
        });
        


    }


}
