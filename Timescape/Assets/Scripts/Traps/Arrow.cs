using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 5f;
    public int direction = -1;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(arrowSpeed * direction, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == "Terrain" )
        {
            Destroy(gameObject);
        }
        if( collision.tag == "Player" )
        {
            GameManager.Instance.TriggerLose();
            Destroy(gameObject);
        }
    }
}
