using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CharacterController _characterController;
    [Space]
    [SerializeField] private float _speed = 5.5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _gravity = 10f;

    public bool IsPlayerMoving => _joystick.direction != Vector2.zero;

    public void Move()
    {
        Vector2 joystickDirection = _joystick.direction;

        Vector3 moveDirection = Vector3.zero;

        joystickDirection = joystickDirection.Rotate(-CameraSwitcher.Instance.CurrentCameraRotatation);

        if (_characterController.isGrounded)
        {
            var movingDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);

            if (movingDirection != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(movingDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

                moveDirection = transform.forward * _speed;
            }

            else
            {
                moveDirection = Vector3.zero;
            }
        }

        moveDirection.y -= (_gravity * Time.deltaTime);
        _characterController.Move(moveDirection * Time.deltaTime);
    }

    public void AddSpeed(float value) => _speed += value;

    public void RemoveSpeed(float value) => _speed -= value;
}