using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MainMenuButton HoveredButton;

    private Coroutine buttonScaling;
    private Coroutine moveMiddleButton;

    private MainMenuButton[] allButtons;

    public RectTransform RectTrans { get; private set; }
    public Vector3 StartPos { get; private set; }

    private const float animSpeed = 5f; // Shared speed for scaling and movement

    private void Start()
    {
        allButtons = transform.parent.GetComponentsInChildren<MainMenuButton>();
        RectTrans = GetComponent<RectTransform>();
        StartPos = RectTrans.localPosition;
    }

    private void UpdateHoveredButton()
    {
        foreach (var _buttons in allButtons)
        {
            _buttons.HoveredButton = this;
        }
    }

    #region Pointer Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateHoveredButton();

        if (HoveredButton != this) return;

        DecreaseScales();

        Vector3 _hoverScale = new(1.2f, 1.2f, 1.2f);
        ScaleButton(transform.localScale, _hoverScale);

        if (allButtons[1] == this)
        {
            MoveMiddleButton(RectTrans.localPosition, StartPos);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoveredButton != this) return;

        ResetTransforms();
    }
    #endregion

    #region Coroutine base Functions
    private static IEnumerator AnimateTransform(System.Action<float> _Update, float _speed)
    {
        float _progress = 0f;

        while (_progress < 1f)
        {
            _Update(_progress);
            _progress += Time.unscaledDeltaTime * _speed;
            yield return null;
        }

        _Update(1f);
    }

    private void RestartCoroutine(ref Coroutine _coroutineVar, IEnumerator _coroutine)
    {
        if (_coroutineVar != null)
        {
            StopCoroutine(_coroutineVar);
        }

        _coroutineVar = StartCoroutine(_coroutine);
    }
    #endregion

    #region Movement
    public void MoveMiddleButton(Vector3 _startPos, Vector3 _targetPos)
    {
        RestartCoroutine(ref moveMiddleButton, MoveButton(_startPos, _targetPos));
    }

    private IEnumerator MoveButton(Vector3 _startPos, Vector3 _targetPos)
    {
        yield return AnimateTransform( _progress => RectTrans.localPosition = Vector3.Lerp(_startPos, _targetPos, _progress), animSpeed);
    }
    #endregion


    #region Scaling
    private void ResetTransforms()
    {
        foreach (var _buttons in allButtons)
        {
            _buttons.ScaleButton(_buttons.transform.localScale, Vector3.one);
        }

        allButtons[1].MoveMiddleButton(allButtons[1].RectTrans.localPosition, allButtons[1].StartPos);
    }

    private void ScaleButton(Vector3 _startScale, Vector3 _targetScale)
    {
        RestartCoroutine(ref buttonScaling, ScalingButton(_startScale, _targetScale));
    }

    private IEnumerator ScalingButton(Vector3 _startScale, Vector3 _targetScale)
    {
        yield return AnimateTransform( _progress => transform.localScale = Vector3.Lerp(_startScale, _targetScale, _progress), animSpeed);
    }

    private void DecreaseScales()
    {
        foreach (var _button in allButtons)
        {
            if (_button == this) continue;

            Vector3 _reducedScale = new(0.89f, 0.89f, 1);
            _button.ScaleButton(_button.transform.localScale, _reducedScale);

            if (_button != allButtons[1])
            {
                Vector3 _leftPosOffset = new(-100, -324, 0);
                Vector3 _rightPosOffset = new(100, -324, 0);
                Vector3 _endPos = _button.RectTrans.localPosition.x < 0 ? _leftPosOffset : _rightPosOffset;

                allButtons[1].MoveMiddleButton(allButtons[1].RectTrans.localPosition, _endPos);
            }
        }
    }
    #endregion
}