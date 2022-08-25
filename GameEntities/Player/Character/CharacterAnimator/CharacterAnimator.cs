using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] protected PlayerMover _playerMover;
    [SerializeField] protected Animator _animator;

    private readonly int _animHashMoving = Animator.StringToHash("Moving");

    private void Update()
    {
        SetMovingState(_playerMover.IsPlayerMoving);
    }

    private void SetMovingState(bool isMoving)
    {
        _animator.SetBool(_animHashMoving, isMoving);
    }
}