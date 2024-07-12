using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe do jogador
public class Player : MonoBehaviour
{
    // Componentes do jogador
    [Header("Componentes")]
    Rigidbody playerBody; // Corpo do jogador (Rigidbody)

    // Movimentação do jogador
    [Header("Movimentação")]
    float inputX; // Input horizontal do jogador
    float inputZ; // Input vertical do jogador
    public float speed; // Velocidade do jogador
    Vector3 movement; // Vetor de movimentação do jogador

    // Pulo do jogador
    [Header("Pulo")]
    float inputY; // Input de pulo do jogador
    public float jumpForce; // Força do pulo do jogador
    bool isGrounded; // Verifica se o jogador está no chão
    public GameObject checkGround; // Objeto que verifica se o jogador está no chão
    public float checkRadius; // Raio de verificação do chão
    public LayerMask whatIsGround; // Layer do chão

    // Câmera do jogador
    [Header("Camera")]
    Transform cameraT; // Transform da câmera
    float verticalLookRotation; // Rotação vertical da câmera
    float mouseX; // Input de movimento horizontal da câmera
    float mouseY; // Input de movimento vertical da câmera
    public float mouseSensitivityX; // Sensibilidade de movimento horizontal da câmera
    public float mouseSensitivityY; // Sensibilidade de movimento vertical da câmera
    float mouse2;

    //Atirar
    [Header("Tiros")]
    float inputShoot;

    // Inicialização do jogador
    void Start()
    {
        // Obter o componente Rigidbody do jogador
        playerBody = GetComponent<Rigidbody>();
        // Obter a transform da câmera
        cameraT = Camera.main.transform;
        //Tirar o cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Atualização do jogador (chamada a cada frame)
    void Update()
    {
        // Obter os inputs da câmera
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // Verificar se o jogador está no chão
        isGrounded = Physics.CheckSphere(checkGround.transform.position, checkRadius, whatIsGround);

        // Rotacionar o jogador com a câmera
        RotateWithCamera();

        // Movimentar o jogador para frente baseado na rotação
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        //Checar se o jogador apertou o botão de pular
        inputY = Input.GetAxis("Jump");

        // Converter a movimentação para o sistema de coordenadas do jogador
        movement = transform.TransformDirection(new Vector3(inputX * speed, 0, inputZ * speed));

        inputShoot = Input.GetAxis("Fire1");
        mouse2 = Input.GetAxis("Fire2");
        if(Cursor.visible == false && mouse2 != 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(Cursor.visible == true && inputShoot != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Atualização física do jogador (chamada a cada frame físico)
    void FixedUpdate()
    {
        // Atualizar a velocidade do jogador
        playerBody.velocity = new Vector3(movement.x, playerBody.velocity.y, movement.z);
        if (isGrounded && inputY != 0)
        {
            // Adicionar força de pulo ao jogador
            playerBody.AddForce(new Vector3(movement.x, jumpForce, movement.z));
        }
    }

    // Rotacionar o jogador com a câmera
    public void RotateWithCamera()
    {
        // Rotacionar o jogador com o input da câmera
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        // Atualizar a rotação vertical da câmera
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        // Limitar a rotação vertical da câmera entre -60 e 60 graus
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        // Atualizar a rotação da câmera
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    // Desenhar gizmos para debug
    private void OnDrawGizmos()
    {
        // Desenhar uma esfera para representar a área de verificação do chão
        Gizmos.DrawSphere(checkGround.transform.position, checkRadius);
        // Mudar a cor dos gizmos para vermelho
        Gizmos.color = Color.red;
    }
}