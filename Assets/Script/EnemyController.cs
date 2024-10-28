using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;

    // ����
    public bool vertical;
    // �ƶ�ʱ��
    public float changeTime = 3.0f;

    Rigidbody2D enemyRigidbody2d;
    float timer;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = enemyRigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
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
}