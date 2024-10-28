using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // �ٶ�
    public float speed = 3.0f;

    // ����ֵ
    public int maxHealth = 5;
    int currentHealth;

    public int health { get { return currentHealth;  } }

    Rigidbody2D rigidbody2d;

    // λ��
    float horizontal;
    float vertical;

    // �޵�ʱ��
    public float timeInvincible = 2.0f;

    // �޵�״̬
    bool isInvincible;

    // �޵�ʱ���ʱ��
    float invincibleTimer; 

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;

        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
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

        //�ո�ص����ĵ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 position = transform.position;
            position.x = 0;
            position.y = 0;
            transform.position = position;
        }

        // �����޵�ʱ�䣬�ݼ�
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
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
            // �ж��Ƿ��޵�
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
        
    }
}
