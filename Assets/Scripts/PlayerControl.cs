using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Animator animator;
    public float speed = 1.5f;
    private Rigidbody2D rb;

    private float directionX;
    private float directionY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetAxis("Horizontal");
        directionY = Input.GetAxis("Vertical");

        animator.SetFloat("MoveX", directionX);
        animator.SetFloat("MoveY", directionY);
    }


    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x += (speed * directionX * Time.deltaTime);
        pos.y += (speed * directionY * Time.deltaTime);
        rb.MovePosition(pos);
    }

}
