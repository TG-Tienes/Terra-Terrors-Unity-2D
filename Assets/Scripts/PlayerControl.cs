using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Animator animator;
    public float speed = 1.5f;
    private Rigidbody2D rb;

    private float directionX;
    private float directionY;
    Vector2 lookDirection = new Vector2(1, 0);

    public Canvas playerInventoryCanvas;
    private bool playerInventoryCanvas_isActive = false;

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

        Vector2 move = new Vector2(directionX, directionY);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("MoveX", lookDirection.x);
        animator.SetFloat("MoveY", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerInventoryCanvas_isActive = !playerInventoryCanvas_isActive;
            playerInventoryCanvas.gameObject.SetActive(playerInventoryCanvas_isActive);
        }
    }


    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x += (speed * directionX * Time.deltaTime);
        pos.y += (speed * directionY * Time.deltaTime);
        rb.MovePosition(pos);
    }

}
