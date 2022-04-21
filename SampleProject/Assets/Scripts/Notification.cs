using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    public void Notify(string text){
        StartCoroutine(showPanel(text));
    }

    IEnumerator showPanel(string text){
        notificationPanel.SetActive(true);
        notificationText.text = text;
        yield return new WaitForSeconds(3);
        notificationPanel.SetActive(false);
    }
}
