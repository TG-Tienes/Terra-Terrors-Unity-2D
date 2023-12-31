using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    public float speed = 1.5f;
    private Rigidbody2D rb;

    public Sprite defaultRotateWeapon;
    private GameObject rotateWeapon;

    private float directionX;
    private float directionY;
    Vector2 lookDirection = new Vector2(1, 0);

    public Canvas miniMapCanvas;
    public Canvas playerInventoryCanvas;
    private bool playerInventoryCanvas_isActive;

    public static PlayerStats playerStats;

    //Handle EXP Bar
    private int level;
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
    public int manaMax;
    int manaCurrent;

    public int mana
    {
        get { return manaCurrent; }
        set { manaCurrent = value; }
    }

    //Handle Blood Bar 
    public int bloodMax;
    int bloodCurrent;

    public int blood
    {
        get { return bloodCurrent; }
        set { bloodCurrent = value; }
    }

     private struct QuestInfo
    {
        public int questType;
        public string questName;
        public string questDescription;
        public int amount;
    }

    private QuestInfo[] quests = {
        new QuestInfo { questType = 2, questName = "Collect Special Items", questDescription = "Collect unique, elusive items scattered throughout the realm. These special artifacts possess mystical qualities and are crucial for unlocking hidden powers or crafting powerful gear.", amount = 3 },
        new QuestInfo { questType = 1, questName = "Kill Boss", questDescription = "Overcome the challenge posed by the level-specific mini-bosses. These adversaries, while smaller in stature, hold significant power and guard valuable treasures or pathways deeper into the world.", amount = 1 },
        new QuestInfo { questType = 0, questName = "Kill 10 Monsters", questDescription = "Engage and eliminate ten formidable creatures wandering the lands. These adversaries range from ferocious beasts to cunning foes, each presenting a unique threat and offering valuable rewards upon their defeat.", amount = 5 }
    };

    private AudioSource _walkAudio;
    private AudioSource _shootingAudio;

    // Start is called before the first frame update
    void Start()
    {
        _walkAudio = transform.GetChild(2).GetChild(0).GetComponent<AudioSource>();
        _shootingAudio = transform.GetChild(2).GetChild(1).GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentTime = invincibleTime;
        ExpBar.instance.SetValue(0);

        playerStats = StatsManager.instance.playerStats;
        level = playerStats.level;
        manaMax = playerStats.mana;
        bloodMax = playerStats.health;

        rotateWeapon = transform.GetChild(0).GetChild(0).gameObject;

        manaCurrent = manaMax;
        bloodCurrent = bloodMax;
        // playerInventoryCanvas_isActive = playerInventoryCanvas.isActiveAndEnabled;
        AddQuest();
    }


    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetAxis("Horizontal");
        directionY = Input.GetAxis("Vertical");

        SpriteRenderer weaponSpriteRenderer = rotateWeapon.GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.sprite = (EquipmentManager.instance.currentWeapon != null) ? EquipmentManager.instance.currentWeapon.sprite : defaultRotateWeapon;

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
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (EquipmentManager.instance.currentEquipment[3] != null)
            {
                EquipmentManager.instance.currentWeapon = EquipmentManager.instance.currentEquipment[3];
                WeaponSlotController.instance.UpdateWeaponSlots(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (EquipmentManager.instance.currentEquipment[4] != null)
            {
                EquipmentManager.instance.currentWeapon = EquipmentManager.instance.currentEquipment[4];
                WeaponSlotController.instance.UpdateWeaponSlots(1);
            }
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

    private Quest CreateQuest(string questName, string questDescription, int type, int amount) {
        Quest q = new Quest();
        q.questName = questName;
        q.questDescription = questDescription;
        q.expReward = UnityEngine.Random.Range(100,1000);
        q.goldReward = UnityEngine.Random.Range(5,20);
        q.questCategory = 0;
        q.objective = new Quest.Objective();
        q.objective.type = (Quest.Objective.Type)type;
        q.objective.amount = amount;
        return q;
    }

   private void AddQuest()
    {
       foreach (var quest in quests)
        {
            QuestLog.AddQuest(CreateQuest(quest.questName, quest.questDescription, quest.questType, quest.amount));
        }
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
        if (dataBlood < 0) {
            animator.SetTrigger("Hit");
            dataBlood *= (int) (100f / (100 + StatsManager.instance.playerStats.defense));
        }
        bloodCurrent += dataBlood;

        if (bloodCurrent <= 0)
        {
            BloodBar.instance.SetValue(0);
            animator.SetTrigger("Dead");
        }

        //Set data mana
        if (bloodCurrent > -1 && bloodCurrent < bloodMax + 1)
        {
            BloodBar.instance.SetValue(bloodCurrent / (float)bloodMax);
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

    public void playWalkAudio()
    {
        _walkAudio.Play();
    }
}
