using DG.Tweening;
using UnityEngine;

public class Money : MonoBehaviour
{
    private Tween _currentAnimation;

    public void PlayCollectAnimation(Transform newParent, Vector3 targetPosition)
    {
        transform.SetParent(newParent);

        var baseRotation = new Vector3(0, 90f, 0);

        _currentAnimation = DOTween.Sequence()
            .Append(transform.DOLocalJump(targetPosition, 1f, 1, 0.5f))
            .Join(transform.DOLocalRotate(baseRotation, 0.5f))
            .SetEase(Ease.OutSine);
    }

    private void OnDestroy()
    {
        _currentAnimation.Kill();

        transform.DOKill();
    }
}