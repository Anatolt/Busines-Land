using System.Collections;
using UnityEngine;

public class GyroBoost : MonoBehaviour
{
    [SerializeField] private GyroBoostUI _gyroBoostUI;
    [Space]
    [SerializeField] private float _boostDuration = 10f;
    [SerializeField] private float _boostValue = 10f;

    private float _currentDuration = 0f;

    private bool _enabled = false;

    private void Start()
    {
        _gyroBoostUI.SetOnButtonClickedAction(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_enabled)
            return;

        EnableBoost();
    }

    private void EnableBoost()
    {
        _enabled = true;

        CharacterSwitcher.Instance.Switch(CharacterType.ElonGyro);

        //Player.Instance.BoostSpeed(_boostValue, _boostDuration);

        StartCoroutine(BoostCoroutine());
    }

    private void DisableBoost()
    {
        _enabled = false;

        CharacterSwitcher.Instance.Switch(CharacterType.Elon);
    }

    private IEnumerator BoostCoroutine()
    {
        _currentDuration = _boostDuration;

        while (_currentDuration > 0)
        {
            yield return null;
            yield return null;

            _currentDuration -= Time.deltaTime;

            UpdateUI();
        }

        DisableBoost();

        yield return null;
    }

    private void UpdateUI()
    {
        _gyroBoostUI.SetBarFillAmount(_currentDuration / _boostDuration);
    }
}