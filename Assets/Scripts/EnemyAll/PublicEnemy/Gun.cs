using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private float roteOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.5f; // Enemy bắn chậm hơn
    private float nextShot;
    [SerializeField] private int maxAmmo = 24;
    private int currentAmmo;

    [SerializeField] private float attackRange = 5f; // Phạm vi tấn công
    private Transform player;

    void Start()
    {
        currentAmmo = maxAmmo;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Tìm Player theo tag
    }

    void Update()
    {
        if (player == null) return;

        RotateGun();
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Shoot();
        }
    }

    void RotateGun()
    {
        Vector3 displacement = transform.position - player.position;
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + roteOffset);
    }

    void Shoot()
    {
        if (Time.time > nextShot && currentAmmo > 0)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
        }
    }
}
