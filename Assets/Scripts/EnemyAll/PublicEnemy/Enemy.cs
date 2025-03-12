using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    public AIPath aiPath;
    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected Player player;
    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image hpBar;

    [SerializeField] protected float enterDamage = 30f;
    [SerializeField] protected float stayDamage = 1f;
    [SerializeField] private CircleCollider2D attackRange;
    private bool playerInRange = false;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] private float obstacleCheckDistance = 1f; // Khoảng cách kiểm tra vật cản
    [SerializeField] private LayerMask obstacleLayer; // Lớp vật cản (tường, chướng ngại vật)

    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D
        UpdateHpBar();
    }

    protected virtual void Update()
    {
       if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if(aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            playerInRange = false;
        }
    }
}
