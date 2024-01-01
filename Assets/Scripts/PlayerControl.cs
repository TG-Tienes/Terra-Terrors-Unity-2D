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

    public Sprite defaultBulletSprite;
    public Sprite defaultRotateWeapon;
    private GameObject rotateShooting;
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
        public string imagePath;
    }

    private QuestInfo[] quests = {
       new QuestInfo { questType = 2, questName = "Collect Special Items", questDescription = "Find unique items scattered across the realm.", amount = 3, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/BlueCrystalMoving1.png?alt=media&token=412896f0-8b84-4fac-a54f-731d9ff4c922"},
        new QuestInfo { questType = 1, questName = "Kill Boss", questDescription = "Conquer level-specific mini-bosses guarding valuable treasures.", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Minotaur.png?alt=media&token=a4edc1b4-c1a6-4243-b5a7-b0c9488d8128"},
        new QuestInfo { questType = 0, questName = "Kill 10 Monsters", questDescription = "Eliminate ten formidable creatures wandering the lands.", amount = 5, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/gobin.png?alt=media&token=bad4e6bd-c593-4b63-bba7-f4fdf5e5b189"},
        new QuestInfo { questType = 3, questName = "Talk to NPC", questDescription = "Engage in a conversation with an important character.", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Hera.png?alt=media&token=de2dcc53-fd03-4fb7-9ec1-982a4b466e07"},
    };

    private AudioSource _walkAudio;
    private AudioSource _shootingAudio;
    private AudioSource _openInventoryAudio;
    private AudioSource _closeInventoryAudio;

    // Start is called before the first frame update
    void Start()
    {
        GameObject sceneAudioManager = GameObject.Find("SceneAudioManager").gameObject;

        _walkAudio = transform.GetChild(2).GetChild(0).GetComponent<AudioSource>();
        _shootingAudio = transform.GetChild(2).GetChild(1).GetComponent<AudioSource>();

        _openInventoryAudio = sceneAudioManager.transform.GetChild(3).GetComponent<AudioSource>();
        _closeInventoryAudio = sceneAudioManager.transform.GetChild(4).GetComponent<AudioSource>();


        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentTime = invincibleTime;
        ExpBar.instance.SetValue(0);

        playerStats = StatsManager.instance.playerStats;
        level = playerStats.level;
        manaMax = playerStats.mana;
        bloodMax = playerStats.health;

        rotateShooting = transform.GetChild(0).gameObject;
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

        Shooting bulletShooting = rotateShooting.GetComponent<Shooting>();
        bulletShooting.bullet.gameObject.GetComponent<SpriteRenderer>().sprite =
            (EquipmentManager.instance.currentWeapon != null) ? EquipmentManager.instance.currentWeapon.bulletSprite : defaultBulletSprite;

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

            if(playerInventoryCanvas_isActive)
                _openInventoryAudio.Play();
            else
                _closeInventoryAudio.Play();
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

  private Quest CreateQuest(string questName, string questDescription, int type, int amount, string imagePath) {
        Quest q = new Quest();
        q.questName = questName;
        q.questDescription = questDescription;
        q.expReward = UnityEngine.Random.Range(100,1000);
        q.goldReward = UnityEngine.Random.Range(5,20);
        q.questCategory = 0;
        q.imagePath = imagePath;
        q.objective = new Quest.Objective();
        q.objective.type = (Quest.Objective.Type)type;
        q.objective.amount = amount;
        return q;
    }

   private void AddQuest()
    {
       foreach (var quest in quests)
        {
            QuestLog.AddQuest(CreateQuest(quest.questName, quest.questDescription, quest.questType, quest.amount, quest.imagePath));
        }
    }

    public void handleExp(int dataRxp)
    {
        Debug.Log(dataRxp);
        Debug.Log(level);
        Debug.Log(levelList[level - 1]);

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
        if (dataBlood < 0)
        {
            animator.SetTrigger("Hit");
            dataBlood *= (int)(100f / (100 + StatsManager.instance.playerStats.defense));
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
