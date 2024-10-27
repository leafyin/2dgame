using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + 4f * horizontal * Time.deltaTime;
        position.y = position.y + 4f * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //空格回到中心点
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 position = transform.position;
            position.x = 0;
            position.y = 0;
            transform.position = position;
        }

    }
}
