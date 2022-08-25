#pragma warning disable
using DG.Tweening;
using UnityEngine;

public class CrowdHuman : CrowdAgent
{
    [SerializeField] private Animator _animator;

    private Vector3 _moveTargetPosition;

    private readonly string _animatorMovingStateKey = "IsWalking";

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && _targetWayNodePosition != null)
        {
            Gizmos.DrawSphere(_moveTargetPosition, 0.5f);
            Gizmos.DrawLine(_targetWayNodePosition, _moveTargetPosition);
        }
    }

    private void Start()
    {
        base.Start();

        Move();
    }

    private void Move()
    {
        _animator.SetBool(_animatorMovingStateKey, false);

        _moveTargetPosition = _targetWayNodePosition + GetRandomOffset();

        var lookAtRotation = Quaternion.LookRotation((_moveTargetPosition - transform.position).normalized);

        DOTween.Sequence()
            .AppendInterval(Random.Range(0f, 2f))
            .AppendCallback(() =>
            {
                _animator.SetBool(_animatorMovingStateKey, true);
            })
            .Append(transform.DORotateQuaternion(lookAtRotation, 0.5f).SetEase(Ease.Linear))
            .Join(transform.DOMove(_moveTargetPosition, CalculateMovingTime()).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                SetNextNode();
                Move();
            });
    }

    public Vector3 GetRandomOffset()
    {
        var baseVector = Vector2.one;

        baseVector = baseVector.Rotate(Random.Range(0, 360f));

        const float maxLenght = 2f;

        baseVector *= Random.Range(0, maxLenght);

        return new Vector3(baseVector.x, 0, baseVector.y);
    }

    private float CalculateMovingTime()
    {
        return Vector3.Distance(_targetWayNodePosition, transform.position) * 0.4f;
    }
}