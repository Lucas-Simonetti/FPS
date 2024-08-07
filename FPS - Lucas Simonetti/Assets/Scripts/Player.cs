using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe do jogador
public class Player : MonoBehaviour
{
    // Componentes do jogador
    [Header("Componentes")]
    Rigidbody playerBody; // Corpo do jogador (Rigidbody)

    // Movimenta��o do jogador
    [Header("Movimenta��o")]
    float inputX; // Input horizontal do jogador
    float inputZ; // Input vertical do jogador
    public float speed; // Velocidade do jogador
    Vector3 movement; // Vetor de movimenta��o do jogador

    // Pulo do jogador
    [Header("Pulo")]
    float inputY; // Input de pulo do jogador
    public float jumpForce; // For�a do pulo do jogador
    bool isGrounded; // Verifica se o jogador est� no ch�o
    public GameObject checkGround; // Objeto que verifica se o jogador est� no ch�o
    public float checkRadius; // Raio de verifica��o do ch�o
    public LayerMask whatIsGround; // Layer do ch�o

    // C�mera do jogador
    [Header("Camera")]
    Transform cameraT; // Transform da c�mera
    float verticalLookRotation; // Rota��o vertical da c�mera
    float mouseX; // Input de movimento horizontal da c�mera
    float mouseY; // Input de movimento vertical da c�mera
    public float mouseSensitivityX; // Sensibilidade de movimento horizontal da c�mera
    public float mouseSensitivityY; // Sensibilidade de movimento vertical da c�mera
    float inputAim; // Input para dar zoom na mira, por enquanto estamos usando para mostrar o cursor

    // Atirar
    [Header("Tiros")]
    float inputShoot;
    public Transform shootPoint; // Ponto de tiro
    public GameObject bullet; // Bala
    public float timeBetweenShots; // Tempo entre tiros
    bool canShoot; // Verifica se o jogador pode atirar novamente

    // Inicializa��o do jogador
    void Start()
    {
        // Obter o componente Rigidbody do jogador
        playerBody = GetComponent<Rigidbody>();
        // Obter a transform da c�mera
        cameraT = Camera.main.transform;
        // Tirar o cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Inicializar a vari�vel canShoot
        canShoot = true;
    }

    // Atualiza��o do jogador (chamada a cada frame)
    void Update()
    {
        // Obter os inputs da c�mera
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // Verificar se o jogador est� no ch�o
        isGrounded = Physics.CheckSphere(checkGround.transform.position, checkRadius, whatIsGround);

        // Rotacionar o jogador com a c�mera
        RotateWithCamera();

        // Movimentar o jogador para frente baseado na rota��o
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        // Checar se o jogador apertou o bot�o de pular
        inputY = Input.GetAxis("Jump");

        // Converter a movimenta��o para o sistema de coordenadas do jogador
        movement = transform.TransformDirection(new Vector3(inputX * speed, 0, inputZ * speed));

        // Mostrar e sumir mouse
        inputShoot = Input.GetAxis("Fire1");
        inputAim = Input.GetAxis("Fire2");
        if (Cursor.visible == false && inputAim != 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Cursor.visible == true && inputShoot != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Checar se o jogador apertou o bot�o de atirar
        if (inputShoot != 0)
        {
            Shoot();
        }
    }

    // Atualiza��o f�sica do jogador (chamada a cada frame f�sico)
    void FixedUpdate()
    {
        // Atualizar a velocidade do jogador
        playerBody.velocity = new Vector3(movement.x, playerBody.velocity.y, movement.z);
        if (isGrounded && inputY != 0)
        {
            // Adicionar for�a de pulo ao jogador
            playerBody.AddForce(new Vector3(movement.x, jumpForce, movement.z));
        }
    }

    // Rotacionar o jogador com a c�mera
    public void RotateWithCamera()
    {
        // Rotacionar o jogador com o input da c�mera
        transform.Rotate(Vector3.up, mouseX * mouseSensitivityX);

        // Limitar a rota��o vertical da c�mera
        verticalLookRotation += mouseY * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Rotacionar a c�mera com a rota��o vertical
        cameraT.localEulerAngles = new Vector3(-verticalLookRotation, 0, 0);
    }

    // Atirar
    void Shoot()
    {
        if (canShoot)
        {
            // Criar uma bala no ponto de tiro
            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, shootPoint.rotation);

            // Resetar o tempo entre tiros
            canShoot = false;
            Invoke(nameof(ResetShoot), timeBetweenShots);
        }
    }

    // Resetar o tempo entre tiros
    void ResetShoot()
    {
        canShoot = true;
    }

    // Desenhar gizmos para debug
    private void OnDrawGizmos()
    {
        // Desenhar uma esfera para representar a �rea de verifica��o do ch�o
        Gizmos.DrawSphere(checkGround.transform.position, checkRadius);
        // Mudar a cor dos gizmos para vermelho
        Gizmos.color = Color.red;
    }
}