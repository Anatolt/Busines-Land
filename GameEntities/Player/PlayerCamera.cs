using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
	[SerializeField] private float _smoothness = 0.1f;
	[SerializeField] private Transform _playerTransfrom;

	private Camera _cameraComponent;

	private Vector3 velocity;
	private bool _isAnimationPlaying = false;

	public Vector3 CameraOffset { get; private set; }

	private Tween _currentAnimation = null;

	private void Awake()
	{
		_cameraComponent = GetComponent<Camera>();

		CameraOffset = transform.position - _playerTransfrom.position;
	}

	private void LateUpdate()
	{
		if (!_isAnimationPlaying)
			MoveToPlayer();
	}

	public void GameEventAnimationPlay(Camera camera, float duration)
	{
		if (!_cameraComponent.orthographic)
			return;

		_currentAnimation.Kill();

		_isAnimationPlaying = true;

		var baseCameraSize = _cameraComponent.orthographicSize;

		_currentAnimation = DOTween.Sequence()
			.Append(transform.DOMove(camera.transform.position, 0.5f))
			.Join(_cameraComponent.DOOrthoSize(camera.orthographicSize, 0.5f))
			.AppendInterval(duration)
			.AppendCallback(() => _isAnimationPlaying = false)
			.Append(_cameraComponent.DOOrthoSize(baseCameraSize, 0.2f));
	}

	public void Shake(float duration)
	{
		transform.DOShakePosition(duration, 0.1f).SetEase(Ease.InOutSine).SetDelay(0.4f);
	}

	[ContextMenu("Test shake")]
	public void TestShake() => Shake(10);

	private void MoveToPlayer()
    {
		if (!_playerTransfrom)
			return;

		transform.position = Vector3.SmoothDamp(
			current: transform.position,
			target: _playerTransfrom.position + CameraOffset,
			currentVelocity: ref velocity,
			smoothTime: _smoothness);
	}
}