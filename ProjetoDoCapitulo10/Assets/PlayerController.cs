using UnityEngine; // Biblioteca base do Unity
using UnityEngine.InputSystem; // Sistema de entrada moderno

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody; // Controla física e movimento

    private Animator anim;
    
    private Vector2 moveInput; // Guarda direção do movimento (X e Y)

    public GameObject playerSprite;

    public float speed = 5f; // Define a velocidade horizontal
    public float jumpForce = 8f; // Define a força aplicada no pulo

    public float sensorRadius = 0.15f; // Define o raio do sensor que verifica contato com o chão

    private void Start()
    {
        // Captura o Rigidbody2D do objeto ao iniciar
        rigidBody = GetComponent<Rigidbody2D>();

        anim = playerSprite.GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Recebe o input de movimento (teclado ou controle)
        // Exemplo: A/D ou setas retornam valores no eixo X
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Executa apenas no momento em que o botão é pressionado e o personagem esta no chão
        if (context.started && OnGround())
        {
            // Mantém velocidade horizontal e aplica força no eixo Y
            rigidBody.linearVelocity = new Vector2(
                rigidBody.linearVelocity.x,
                jumpForce
            );
        }
    }

    void Update()
    {
        // Ajusta a direção visual do personagem
        if (moveInput.x > 0.0f)
        {
            transform.eulerAngles = new Vector2(0.0f, 0.0f); // Olhando para direita
        }
        else if (moveInput.x < 0.0f)
        {
            transform.eulerAngles = new Vector2(0.0f, 180.0f); // Olhando para esquerda
        }

        Animations();
    }

    private void FixedUpdate()
    {
        // Movimento físico contínuo no eixo X
        // Usa FixedUpdate por ser ligado à física
        rigidBody.linearVelocity = new Vector2(
            moveInput.x * speed,
            rigidBody.linearVelocity.y
        );
    }

    // Detecta se o personagem está tocando o chão usando uma área circular de detecção
    // Essa função utiliza Physics2D.OverlapCircle, que cria uma circunferência virtual na posição do objeto
    // e verifica se há colisão com qualquer objeto que esteja na Layer "Ground".
    // Retorna true se houver contato com o chão e false caso contrário.
    bool OnGround()
    {
        return Physics2D.OverlapCircle(transform.position, sensorRadius, 
            1 << LayerMask.NameToLayer("Ground"));
    }

    private void OnDrawGizmos()
    {
        // Define a cor dos Gizmos que serão desenhados na Scene View
        Gizmos.color = Color.yellow;

        // Desenha uma esfera na posição do objeto, usando o sensorRadius como raio
        // Essa esfera representa visualmente a área de detecção do sensor de chão
        Gizmos.DrawSphere(transform.position, sensorRadius);
    }

    void Animations()
    {
        anim.SetFloat("pMove", Mathf.Abs(moveInput.x));
        anim.SetBool("pJump", OnGround());
    }

}