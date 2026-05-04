using UnityEngine;
// Importa a biblioteca principal do Unity (GameObject, Transform, Rigidbody, etc.)

using UnityEngine.InputSystem;
// Importa o novo sistema de Input (Input System)

public class PlayerController : MonoBehaviour
{
    // Referência ao Rigidbody2D do personagem (responsável pela física)
    private Rigidbody2D rb;

    // Armazena o valor do input de movimento (eixo X e Y)
    private Vector2 moveInput;

    // Velocidade de movimento horizontal do personagem
    public float speed = 5f;

    // Força aplicada no pulo
    public float jumpForce = 8f;

    // Controla se o personagem está no chão (evita pulo infinito)
    private bool isGrounded = true;

    void Awake()
    {
        // Awake é chamado antes do Start
        // Aqui pegamos o componente Rigidbody2D do objeto
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Esse método é chamado automaticamente pelo Input System
        // quando a ação "Move" é executada

        // Lê o valor do input (ex: teclado ou controle)
        // e armazena no vetor moveInput
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Esse método é chamado quando a ação "Jump" é acionada

        // context.started verifica se o botão acabou de ser pressionado
        // isGrounded garante que só pode pular se estiver no chão
        if (context.started && isGrounded)
        {
            // Aplica uma velocidade vertical ao Rigidbody
            // Mantém a velocidade horizontal atual (rb.linearVelocity.x)
            // e altera apenas o eixo Y (pulo)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // Após pular, define como falso para evitar múltiplos pulos
            isGrounded = false;
        }
    }

    void Update()
    {
        // Update é chamado a cada frame

        // Aplica movimento horizontal baseado no input
        // moveInput.x varia de -1 a 1 (esquerda/direita)
        // Multiplicamos pela velocidade (speed)

        // Mantemos a velocidade vertical atual (rb.linearVelocity.y)
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Esse método é chamado quando o personagem colide com outro objeto

        // Verifica se o objeto possui a tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Se colidir com o chão, permite pular novamente
            isGrounded = true;
        }
    }
}