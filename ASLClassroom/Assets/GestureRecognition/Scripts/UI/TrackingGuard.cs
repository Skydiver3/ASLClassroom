using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackingGuard : MonoBehaviour
{
    [SerializeField] private OVRHand _handL;
    [SerializeField] private OVRHand _handR;

    private bool _disclaimerActive = false;
    private Coroutine _showDisclaimerCoroutine;
    [SerializeField] private float speed = 0.1f;

    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _text;

    private float _progress = 0;

    void Update()
    {
        if (!_handR.IsTracked || !_handL.IsTracked) TriggerDisclaimer();
        else if (_disclaimerActive) TriggerHideDisclaimer();
    }

    private void TriggerDisclaimer()
    {
        if (_disclaimerActive) return;
        _disclaimerActive = true;
        if(_showDisclaimerCoroutine!=null) StopCoroutine(_showDisclaimerCoroutine);
        _showDisclaimerCoroutine = StartCoroutine(ShowDisclaimer());
    }
    private void TriggerHideDisclaimer()
    {
        if (_showDisclaimerCoroutine != null) StopCoroutine(_showDisclaimerCoroutine);
        _disclaimerActive = false;
        _showDisclaimerCoroutine = StartCoroutine(HideDisclaimer());
    }

    private IEnumerator ShowDisclaimer()
    {
        SetActive(true);
        float j = _progress;

        for (float i = j; i < 1; i += speed)
        {
            yield return new WaitForFixedUpdate();

            _progress = i;
            SetVisibility(i);
        }
        yield return new WaitForFixedUpdate();
        SetVisibility(1);
    }
    private IEnumerator HideDisclaimer()
    {
        float j = _progress;

        for (float i = j; i > 0; i -= speed)
        {
            yield return new WaitForFixedUpdate();

            _progress = i;
            SetVisibility(i);
        }
        yield return new WaitForFixedUpdate();
        SetVisibility(0);
        SetActive(false);
    }

    private void SetVisibility(float i)
    {
        if(_text) _text.alpha = i;
        if (_background)
        {
            Color c = _background.color;
            c.a = i;
            _background.color = c;
        }
    }
    private void SetActive(bool _enabled)
    {
        _background.gameObject.SetActive(_enabled);
        _text.gameObject.SetActive(_enabled);
    }
}
