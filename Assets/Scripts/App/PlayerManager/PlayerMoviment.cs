using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    
    [Header("Movimentação")]
    public float moveSpeed = 5f; // Velocidade de movimento horizontal
    public float jumpForce = 10f; // Força do pulo
    private float horizontalInput; // Entrada horizontal
    private bool isGrounded; // Indica se o jogador está no chão
    private bool isJumping; // Impede múltiplos pulos no ar

    
    [Header("Componentes")]
    public Rigidbody2D rb; // Referência ao Rigidbody2D
    
    // Captura os inputs do jogador
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Entrada para movimento lateral
    }

    // Gerencia o movimento horizontal do jogador
    private void HandleMovement()
    {
        // Aplica a velocidade no eixo X enquanto mantém a velocidade atual no eixo Y
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Ajusta a direção do sprite baseado na entrada horizontal
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
            if (transform.localScale.x > 0) transform.localScale = new Vector3(
                -transform.localScale.x, transform.localScale.y, 1); // se a escala local estiver virada para a direita, a inverte

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
            if (transform.localScale.x < 0) transform.localScale = new Vector3(
                -transform.localScale.x, transform.localScale.y, 1);// se a escala local estiver virada para a esquerda, a inverte
        
    }
}
