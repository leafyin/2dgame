using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;

    // 方向
    public bool vertical;
    // 移动时间
    public float changeTime = 3.0f;

    Rigidbody2D enemyRigidbody2d;
    float timer;
    int direction = 1;
    bool broken = true;

    Animator animator;
    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = enemyRigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        enemyRigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(1, false);
        }
    }

    public void Fix()
    {
        broken = false;
        // 修复好机器人之后吧刚体去掉
        GetComponent<Rigidbody2D>().simulated = false;
        // 播放修复好动画
        animator.SetTrigger("Fixed");
        Debug.Log("SmokeParticle is stop");
        smokeEffect.Stop();
    }
}
