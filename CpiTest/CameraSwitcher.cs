using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : Singleton<CameraSwitcher>
{
    public event Action CameraSwitched;

    [SerializeField] private List<PlayerCamera> _cameras;
    [Space]
    [SerializeField] private Button _switchButton;

    private int _currentCameraIndex = 0;

    public float CurrentCameraRotatation => _cameras[_currentCameraIndex].transform.rotation.eulerAngles.y;

    public Vector3 CurrentCameraOffset => _cameras[_currentCameraIndex].CameraOffset;

    private void Start()
    {
        _switchButton.onClick.AddListener(SwitchCamera);
    }

    public void PlayEventAnimation(Camera camera, float duration)
    {
        _cameras[_currentCameraIndex].GameEventAnimationPlay(camera, duration);
    }

    public void PlayShakeAnimation(float duration)
    {
        _cameras[_currentCameraIndex].Shake(duration);
    }

    public void SwitchCamera()
    {
        _currentCameraIndex = (_currentCameraIndex + 1) % _cameras.Count;

        foreach (var cam in _cameras)
            cam.gameObject.SetActive(_currentCameraIndex == _cameras.IndexOf(cam));
        
        CameraSwitched?.Invoke();
    }
}