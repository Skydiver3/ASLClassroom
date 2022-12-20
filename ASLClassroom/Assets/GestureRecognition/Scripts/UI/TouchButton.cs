using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchButton : MonoBehaviour
{
    private ButtonInnerTrigger innerTrigger;

    public bool interactive = false;

    [SerializeField] private UnityEvent onHoverEnter;
    [SerializeField] private UnityEvent onHoverExit;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _hoverColor = new Color(0.7f, 0.7f, 0.7f);
    [SerializeField] private Color _touchColor = new Color(0.6f,0.6f,0.6f);
    [SerializeField] private Color _disabledColor = new Color(0.6f,0.6f,0.6f,0.4f);
    [HideInInspector] public MeshRenderer _buttonRenderer;
    private Material _buttonMaterial;

    [SerializeField] private GameObject buttonText;


    private void Awake()
    {
        _buttonRenderer = GetComponentInChildren<MeshRenderer>();
        _buttonMaterial = _buttonRenderer.material;

    }
    public void SetActive(bool active)
    {
        interactive = active;
        if (buttonText) buttonText.SetActive(active);

        if (active)
        {
            SetMaterialColor(_defaultColor);
            onTriggerEnter.AddListener(SetButtonColorTouch);
            onTriggerExit.AddListener(SetButtonColorHover);
        }
        else
        {
            SetMaterialColor(_disabledColor);
            onTriggerEnter.RemoveListener(SetButtonColorTouch);
            onTriggerExit.RemoveListener(SetButtonColorHover);
        }

    }
    private void SetMaterialColor(Color color)
    {
        _buttonMaterial.SetColor("_Color", color);
    }
    private void SetButtonColorTouch()
    {
        SetMaterialColor(_touchColor);
    }
    private void SetButtonColorHover()
    {
        SetMaterialColor(_hoverColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!interactive) return;
        SetMaterialColor(_hoverColor);
        onHoverEnter?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!interactive) return;
        SetMaterialColor(_defaultColor);
        onHoverExit?.Invoke();
    }
}
