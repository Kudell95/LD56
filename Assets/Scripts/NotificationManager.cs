using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    public GameObject NotificationPrefab;
    public Transform NotificationParent;
    public float NotificationDeletionTime;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            Notify("Hello World!", true);
    }

    public void Notify(string message, bool Urgent = false)
    {
        GameObject notification = Instantiate(NotificationPrefab, NotificationParent);
        notification.GetComponent<NotificationMessage>().Message(message, Urgent);

        if(GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().Play();
    }


    





}
