using UnityEngine;
using DG.Tweening;

public class CompanyAnimator
{
    private Transform _modelTransform;

    private AdditionalAnimation _upgradeAdditionalAnimation;

    public float BuildAnimationDuration => 0.04f * _modelTransform.childCount + 0.35f;
    public float UpgradeAnimationDuration => 0.4f + (_upgradeAdditionalAnimation != null ? _upgradeAdditionalAnimation.Duration * 0.8f : 0);

    public CompanyAnimator(Transform modelTransform, AdditionalAnimation upgradeAdditionalAnimation = null)
    {
        _modelTransform = modelTransform;
        _upgradeAdditionalAnimation = upgradeAdditionalAnimation;
    }

    public void PlayBuildAnimation()
    {
        var totalDelay = 0f;
        var delayStep = 0.06f;

        for (var i = 0; i < _modelTransform.childCount; i++)
        {
            var model = _modelTransform.GetChild(i);

            if (model == _modelTransform)
                continue;

            var basePositionY = model.transform.position.y;

            model.transform.position = new Vector3()
            {
                x = model.transform.position.x,
                y = basePositionY + 50f,
                z = model.transform.position.z
            };

            model.gameObject.SetActive(false);

            model.transform.DOMoveY(basePositionY, 0.35f)
                .SetDelay(totalDelay += delayStep)
                .SetEase(Ease.OutQuad)
                .OnStart(() =>
                {
                    model.gameObject.SetActive(true);

                    AudioPlayer.Instance.PlayLandingSound();
                });
        }
    }

    public void PlayUpgradeAnimation(ParticleSystem particles)
    {
        var force = 0.1f;

        _modelTransform.DOPunchScale(Vector2.up * force, 0.4f, 1)
            .SetEase(Ease.OutBack)
            .SetDelay(0.4f)
            .OnStart(() => particles.Play());

        _upgradeAdditionalAnimation?.Play();
    }
}