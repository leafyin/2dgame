using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // 速度
    public float speed = 3.0f;

    // 生命值
    public int maxHealth = 5;
    int currentHealth;

    public int health { get { return currentHealth;  } }

    Rigidbody2D rigidbody2d;

    // 位置
    float horizontal;
    float vertical;

    // 无敌时间
    public float timeInvincible = 2.0f;

    // 无敌状态
    bool isInvincible;

    // 无敌时间计时器
    float invincibleTimer;

    Animator animator;

    // 初始ruby看向的方向
    public Vector2 lookDirection = new Vector2(1, 0);

    // 子弹
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;

        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        // 移动时判断最后停止的方向并归一化
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //空格回到中心点
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 position = transform.position;
            position.x = 0;
            position.y = 0;
            transform.position = position;
        }

        // 计算无敌时间，递减
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // 发射子弹
        if (Input.GetKeyDown(KeyCode.J))
        {
            Launch();
        }

        // 射线投射
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, 
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            // 这里为什么是hit 为布尔值
            if (hit != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (hit.collider != null)
                {
                    character.DisplayDialog();
                    //Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                }
            }
        }

    }

    public void ChangeHealth(int amount, bool type)
    {
        if (type)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        }
        else
        {
            // 受击动画
            animator.SetTrigger("Hit");

            // 判断是否无敌
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            currentHealth = Mathf.Clamp(currentHealth - amount, -1, maxHealth);
            Debug.Log("Ruby has been damaged -" + amount + "hp,current hp is" + currentHealth);
            if (currentHealth == 0)
            {
                Debug.Log("Ruby is dead!");
                Destroy(gameObject);
            }
        }
        // 调用设置血条动画
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, 
            rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}
