using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChecklistManager : MonoBehaviour
{
    private static ChecklistManager _instance;
    public static ChecklistManager Instance { get { return _instance; } }


    public GameObject itemPrefab;
    private List<GameObject> itemPool = new List<GameObject>();
    private List<GameObject> activeItems = new List<GameObject>();

    public TextMeshProUGUI gestureName;
    public TextMeshProUGUI gestureDescription;
    public float distance;
    private float height;

    public TouchButton exitButton;

    private void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdateItems(string _gestureName, string _gestureDescription, string[] _messages, KeyStates[] _states, Sprite[] _sprites)
    {
        gestureName.text = "Gesture: " + _gestureName;
        gestureDescription.text = _gestureDescription;
        //hide all active items
        foreach (GameObject item in activeItems)
        {
            item.SetActive(false);
            itemPool.Add(item);
        }
        activeItems.Clear();

        //show all needed items
        for (int i = 0; i < _messages.Length; i++)
        {
            GameObject item = null;

            //spawn or get from pool            
            if (itemPool.Count == 0)
            {
                item = Instantiate(itemPrefab,this.transform);
                activeItems.Add(item);
            }
            else if (itemPool.Count != 0)
            {
                item = itemPool[0];
                itemPool.RemoveAt(0);
                activeItems.Add(item);
                item.SetActive(true);
            }

            //set y position, align with other elements
            RectTransform rTransform = item.GetComponent<RectTransform>();
            float height = rTransform.rect.y;
            rTransform.localPosition = Vector3.up * (height + distance) * i;

            //set text and state of checkbox on item
            item.GetComponent<ChecklistItem>().SetDisplay(_messages[i], _states[i], _sprites[i]);
        }
    }
    public void UpdateOnlyExplicitItems(string _gestureName, string _gestureDescription, string[] _messages, KeyStates[] _states, Sprite[] _sprites)
    {
        gestureName.text = "Repeat: " + _gestureName;
        gestureDescription.text = "Repeat the gesture from memory";
        //hide all active items
        foreach (GameObject item in activeItems)
        {
            item.SetActive(false);
            itemPool.Add(item);
        }
        activeItems.Clear();

        //show all needed items
        int j = 0;
        for (int i = 0; i < _messages.Length; i++)
        {
            if (_states[i] == KeyStates.None) continue;
            
            GameObject item = null;

            //spawn or get from pool            
            if (itemPool.Count == 0)
            {
                item = Instantiate(itemPrefab,this.transform);
                activeItems.Add(item);
            }
            else if (itemPool.Count != 0)
            {
                item = itemPool[0];
                itemPool.RemoveAt(0);
                activeItems.Add(item);
                item.SetActive(true);
            }

            //set y position, align with other elements
            RectTransform rTransform = item.GetComponent<RectTransform>();
            float height = rTransform.rect.y;
            rTransform.localPosition = Vector3.up * (height + distance) * j;
            j++;

            //set text and state of checkbox on item
            item.GetComponent<ChecklistItem>().SetDisplay(_messages[i], _states[i], null);
        }
    }

    public void Clear()
    {
        gestureName.text = "";
        gestureDescription.text = "";
        //hide all active items
        foreach (GameObject item in activeItems)
        {
            if(item)item.SetActive(false);
            if(item)itemPool.Add(item);
        }
        activeItems.Clear();
    }


}
