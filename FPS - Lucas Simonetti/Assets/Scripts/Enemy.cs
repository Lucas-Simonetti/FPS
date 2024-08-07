using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Vida do inimigo
    public float health = 100f;

    // Velocidade de movimento do inimigo
    public float moveSpeed = 5f;

    // Tamanho da área circular de perseguição
    public float pursuitRadius = 10f;

    // Referência ao player
    private Transform player;

    // Dano do tiro
    public float bulletDamage = 10f;

    // Referência ao componente Rigidbody do inimigo
    private Rigidbody enemyBody;

    void Start()
    {
        // Encontrar o objeto Player na cena
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Obter o componente Rigidbody do inimigo
        enemyBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Verificar se o player está dentro da área circular de perseguição
        if (Vector3.Distance(transform.position, player.position) <= pursuitRadius)
        {
            // Movimentar o inimigo em direção ao player
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calcular a direção do player em relação ao inimigo
        Vector3 direction = (player.position - transform.position).normalized;

        // Movimentar o inimigo na direção do player
        enemyBody.velocity = direction * moveSpeed;
    }

    // Função chamada quando o inimigo é atingido por um tiro do player
    void OnTriggerEnter(Collider other)
    {
        // Verificar se o objeto que colidiu é uma bala do player
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Tomar dano do tiro
            TakeDamage(10f);

            //Destruir tiro do player
            Destroy(other.gameObject);
        }
    }

    // Função que aplica dano ao inimigo
    void TakeDamage(float damage)
    {
        // Reduzir a vida do inimigo
        health -= damage;

        // Verificar se o inimigo morreu
        if (health <= 0f)
        {
            Die();
        }
    }

    // Função chamada quando o inimigo morre
    void Die()
    {
        // Destruir o inimigo
        Destroy(gameObject);
    }

    // Gizmo para desenhar a área circular de perseguição
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pursuitRadius);
    }
}