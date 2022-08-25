using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SpaceYRocketAnimation : AdditionalAnimation
{
    [SerializeField] private PhysicalMoney _moneyTemplate;
    [SerializeField] private ParticleSystem _particleSystem;

    [Space]
    [SerializeField] private float _yTarget;
    [SerializeField] private float _time;

    public override float Duration => _time;

    private void Start()
    {
        Enabled = true;
    }

    public override void Play()
    {
        if (!Enabled)
            return;

        StartCoroutine(SpawnMoney());

        _particleSystem.Play();

        CameraSwitcher.Instance.PlayShakeAnimation(7f);

        transform.DOMoveY(_yTarget, _time)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                StopAllCoroutines();

                Enabled = false;
                _time = 0;

                gameObject.SetActive(false);
            });
    }

    private IEnumerator SpawnMoney()
    {
        var moneySpawnDelay = 0.1f;

        while(true)
        {
            if (moneySpawnDelay <= 0)
            {
                var money = Instantiate(_moneyTemplate, transform.position, Quaternion.identity);

                var randomDirection = new Vector3
                {
                    x = Random.Range(0f, 1f),
                    y = Random.Range(0f, 1f),
                    z = Random.Range(0f, 1f)
                };

                money.AddForce(5f, randomDirection);
                moneySpawnDelay = 0.1f;
            }

            moneySpawnDelay -= Time.deltaTime;
            yield return null;
        }
    }
}