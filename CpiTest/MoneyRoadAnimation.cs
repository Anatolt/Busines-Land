using DG.Tweening;
using UnityEngine;

public class MoneyRoadAnimation : MonoBehaviour
{
    [SerializeField] private bool _playOnAwake = false;

    private void Start()
    {
        if (_playOnAwake)
            PlayMoneySpawnAnimation();
    }

    public void PlayMoneySpawnAnimation()
    {
        var allMoney = GetComponentsInChildren<Money>();

        var totalDelay = 0f;
        var delayStep = 0.04f;

        for (var i = 0; i < allMoney.Length; i++)
        {
            var model = allMoney[i];

            var basePositionY = model.transform.position.y;

            model.transform.position = new Vector3()
            {
                x = model.transform.position.x,
                y = basePositionY + 50f,
                z = model.transform.position.z
            };

            model.gameObject.SetActive(false);

            model.transform.DOMoveY(basePositionY, 0.45f)
                .SetDelay(totalDelay += delayStep)
                .SetEase(Ease.OutQuad)
                .OnStart(() =>
                {
                    model.gameObject.SetActive(true);
                });
        }
    }
}