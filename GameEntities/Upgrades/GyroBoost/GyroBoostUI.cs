using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GyroBoostUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _bar;

    public void SetOnButtonClickedAction(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

    public void SetBarFillAmount(float value)
    {
        _bar.fillAmount = Mathf.Lerp(_bar.fillAmount, value, 1f);
    }
}