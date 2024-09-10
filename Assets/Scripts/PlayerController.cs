using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform leftBoundary;
    public Transform rightBoundary;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Проверяем нажатия на клавиши на ПК
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        // Проверка на касание экрана на мобильных устройствах
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                // Если касание происходит на левой половине экрана
                if (touch.position.x < Screen.width / 2)
                {
                    horizontalInput = -1f;
                }
                // Если касание происходит на правой половине экрана
                else if (touch.position.x >= Screen.width / 2)
                {
                    horizontalInput = 1f;
                }
            }
        }

        // Движение игрока
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Ограничение движения игрока
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBoundary.position.x, rightBoundary.position.x),
            transform.position.y,
            transform.position.z
        );
    }
}