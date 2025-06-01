using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float horizontalInput;

    [Header("Componentes")]
    public Rigidbody2D rb;

    private void Update()
    {
        GetInput();
        HandleMovement();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }
    }
}