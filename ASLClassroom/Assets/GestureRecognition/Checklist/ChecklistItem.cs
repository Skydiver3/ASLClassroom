using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An item of the checklist display that holds the instruction text, an image for displaying the key thumbnail, and the checkbox. Spawned by ChecklistManager.
/// </summary>
public class ChecklistItem : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject emptyBox;
    public GameObject checkedBox;
    public GameObject crossedBox;
    public Image image;

    public void SetDisplay(string message, KeyStates state, Sprite sprite)
    {
        messageText.text = message;

        if (sprite) image.enabled = true;
        else image.enabled = false;
        image.sprite = sprite;

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
