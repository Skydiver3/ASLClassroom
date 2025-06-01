using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset keyTemplate;
    UIDocument _document;
    VisualElement _keyAnchorElement;
    Label _gestureTitleLabel;

    [SerializeField] private ComplexPose templateCP;
    [SerializeField] private Gesture templateGesture;
    // Start is called before the first frame update
    void Start()
    {
        _document = GetComponent<UIDocument>();
        _keyAnchorElement = _document.rootVisualElement.Q("KeyAnchor");
        _gestureTitleLabel = _document.rootVisualElement.Q("GestureTitle") as Label;

        DisplayStep(templateGesture, templateGesture.complexPoses[0]);
    }

    public void DisplayStep(Gesture displayedGesture, ComplexPose displayedCP)
    {
        _gestureTitleLabel.text = displayedGesture.name;
        for (int i = 0; i < displayedCP.keys.Count; i++)
        {
            VisualElement newKey = keyTemplate.Instantiate();
            Debug.Log(newKey);
            Debug.Log(_keyAnchorElement);
            _keyAnchorElement.Insert(_keyAnchorElement.childCount, newKey);
            Label foundLabel = newKey.Children().First().Children().First() as Label;
            foundLabel.text = displayedCP.keys[i].description;
        }

    }

   
}
