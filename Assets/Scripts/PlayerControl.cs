using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public Canvas miniMapCanvas;
    public Canvas playerInventoryCanvas;
    private bool playerInventoryCanvas_isActive;

    //Handle EXP Bar
    private int level = 1;
    private int[] levelList = { 100, 200, 300 };
    public float invincibleTime = 10.0f;
    private float currentTime;
    int expCurrent = 0;
    public int exp
    {
        get { return expCurrent; }
        set { expCurrent = value; }
    }

    //Handle Mana Bar
    public int manaMax = 50;
    int manaCurrent;

    public int mana
    {
        get { return manaCurrent; }
        set { manaCurrent = value; }
    }

    //Handle Blood Bar 
    public int bloodMax = 100;
    int bloodCurrent;

    public int blood
    {
        get { return bloodCurrent; }
        set { bloodCurrent = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentTime = invincibleTime;
        ExpBar.instance.SetValue(0);

        manaCurrent = manaMax;

        // playerInventoryCanvas_isActive = playerInventoryCanvas.isActiveAndEnabled;
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
            miniMapCanvas.gameObject.SetActive(!playerInventoryCanvas_isActive);
        }

        recoverInTimeRange();
    }


    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x += (speed * directionX * Time.deltaTime);
        pos.y += (speed * directionY * Time.deltaTime);
        rb.MovePosition(pos);
    }

    public void handleExp(int dataRxp)
    {
        expCurrent += dataRxp;
        if (expCurrent <= levelList[level - 1])
        {
            Debug.Log(expCurrent);
            Debug.Log(levelList[level - 1]);

            ExpBar.instance.SetValue(expCurrent / (float)levelList[level - 1]);
        }

        else if (level - 1 < levelList.Length)
        {

            if (expCurrent > levelList[level - 1])
            {
                level += 1;
                ExpBar.instance.SetLevel(level);
                expCurrent = 0;
            }
        }
    }

    public void handleBlood(int dataBlood)
    {
        bloodCurrent += dataBlood;
        //Set data mana
        if (bloodCurrent > -1 && bloodCurrent < bloodMax + 1)
        {
            ManaBar.instance.SetValue(bloodCurrent / (float)bloodMax);
        }
    }

    public void handleMana(int dataMana)
    {
        manaCurrent += dataMana;
        //Set data mana
        if (manaCurrent > -1 && manaCurrent < manaMax + 1)
        {
            ManaBar.instance.SetValue(manaCurrent / (float)manaMax);
        }
    }

    void recoverInTimeRange()
    {
        //Handle add exp in time range;
        currentTime -= Time.deltaTime;
        if (currentTime < 0 && manaCurrent < manaMax)
        {
            manaCurrent += 1;
            if (manaCurrent <= manaMax)
                ManaBar.instance.SetValue(manaCurrent / (float)manaMax);
            currentTime = invincibleTime;
        }
    }
}
