using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistItem : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject emptyBox;
    public GameObject checkedBox;
    public GameObject crossedBox;

    public void SetDisplay(string message, KeyStates state)
    {
        messageText.text = message;

        checkedBox.SetActive(false);
        crossedBox.SetActive(false);
        emptyBox.SetActive(false);

        if (message == null) return;

        switch (state)
        {
            case KeyStates.Hit:
                checkedBox.SetActive(true);
                break;
            case KeyStates.Fail:
                crossedBox.SetActive(true);
                break;
            case KeyStates.None:
                emptyBox.SetActive(true);
                break;
            default:
                break;
        }
    }
}
