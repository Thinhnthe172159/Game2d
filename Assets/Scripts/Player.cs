using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isDead = false;
    private bool isDamaged = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return; // Nếu chết thì không cho di chuyển nữa

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        // Chuyển đổi trạng thái Idle <-> Run
        if (playerInput.magnitude > 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        // Lật hình nhân vật khi di chuyển trái/phải
        if (playerInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage()
    {
        if (isDead || isDamaged) return;

        isDamaged = true;
        animator.SetTrigger("IsTakeADame");

        // Sau 0.5s quay lại trạng thái trước đó
        Invoke(nameof(ResetDamage), 0.5f);
    }

    void ResetDamage()
    {
        isDamaged = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("IsDeath");

        // Dừng di chuyển
        rb.linearVelocity = Vector2.zero;
        moveSpeed = 0;
    }
}
