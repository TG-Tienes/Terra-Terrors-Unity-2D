using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
public class PlayerControl : MonoBehaviour
{
    #region Singleton
    public static PlayerControl instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    #endregion Singleton


    public Animator animator;
    public TMP_Text coinText;
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
    private bool playerInventoryCanvas_isActive = false;

    public bool consumableOnCooldown = false;
    public Slider consumableCooldownSlider;
    public float cooldownDuration = 5f;
    private float cooldownTimer;

    //Handle Level
    private int level;
    private int[] levelList = { 50, 150, 250, 500, 750, 1000, 1250, 1500, 1750, 2000, 2250, 2500, 2750, 3000, 3500, 4000, 5000, 7000, 10000, 15000, 20000, 300000, 500000 };
    public float invincibleTime = 0.1f;
    private float currentTime;

    //Handle EXP Bar
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
    public int mapType;
    public int blood
    {
        get { return bloodCurrent; }
        set { bloodCurrent = value; }
    }

    //Handle Coin
    int coinCurrent;
    public int coin
    {
        get { return coinCurrent; }
        set { coinCurrent = value; }
    }

    private struct QuestInfo
    {
        public int questType;
        public string questName;
        public string questDescription;
        public int amount;
        public string imagePath;
    }
    public GameObject _endLevelObject;
    private QuestInfo[] quests;
    // private QuestInfo[] quests = {
    //    new QuestInfo { questType = 2, questName = "Collect Special Items", questDescription = "Find unique items scattered across the realm.", amount = 3, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/BlueCrystalMoving1.png?alt=media&token=412896f0-8b84-4fac-a54f-731d9ff4c922"},
    //     new QuestInfo { questType = 1, questName = "Kill Boss", questDescription = "Conquer level-specific mini-bosses guarding valuable treasures.", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Minotaur.png?alt=media&token=a4edc1b4-c1a6-4243-b5a7-b0c9488d8128"},
    //     new QuestInfo { questType = 0, questName = "Kill Monsters", questDescription = "Eliminate ten formidable creatures wandering the lands.", amount = 3, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/gobin.png?alt=media&token=bad4e6bd-c593-4b63-bba7-f4fdf5e5b189"},
    //     new QuestInfo { questType = 3, questName = "Talk to NPC", questDescription = "Engage in a conversation with an important character. Press E to talk to NPC", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Hera.png?alt=media&token=de2dcc53-fd03-4fb7-9ec1-982a4b466e07"},
    // };

    private QuestInfo[] GetQuestsForMap()
    {
        if (mapType == 0)
        {
            return new QuestInfo[]
            {
                    new QuestInfo { questType = 2, questName = "Collect Special Items", questDescription = "Find 3 Blue Crystal Artifacts", amount = 3, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/BlueCrystalMoving1.png?alt=media&token=412896f0-8b84-4fac-a54f-731d9ff4c922"},
                    new QuestInfo { questType = 1, questName = "Kill Boss", questDescription = "Conquer level-specific boss Minotaur guarding valuable treasures.", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Minotaur.png?alt=media&token=a4edc1b4-c1a6-4243-b5a7-b0c9488d8128"},
                    new QuestInfo { questType = 0, questName = "Kill Monsters", questDescription = "Defeat formidable goblin creatures wandering the lands.", amount = UnityEngine.Random.Range(10,15), imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/gobin.png?alt=media&token=bad4e6bd-c593-4b63-bba7-f4fdf5e5b189"},
                    new QuestInfo { questType = 3, questName = "Talk to NPC", questDescription = "Engage in a conversation with an important character. Press E to talk to NPC", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Hera.png?alt=media&token=c3e02820-fe61-4ff7-8974-e1cdb9e0a851"},
                };
        }

        else
        {
            return new QuestInfo[]
            {
                    new QuestInfo { questType = 2, questName = "Collect Special Items", questDescription = "Find 4 keys ", amount = 4, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Props.png?alt=media&token=2b520c37-938f-46b1-9dfd-1e3baaa70d04"},
                    new QuestInfo { questType = 1, questName = "Kill Boss", questDescription = "Conquer a level-specific Bat boss guarding valuable treasures.", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/bat_attack.png?alt=media&token=c564dc3f-14d1-4d3d-a9fe-cbc26785ab98"},
                    new QuestInfo { questType = 0, questName = "Kill Monsters", questDescription = "Defeat formidable goblin creatures wandering the lands.", amount =  UnityEngine.Random.Range(10,15), imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/gobin.png?alt=media&token=bad4e6bd-c593-4b63-bba7-f4fdf5e5b189"},
                    new QuestInfo { questType = 3, questName = "Talk to NPC", questDescription = "Engage in a conversation with an important character. Press E to interact with the NPC.", amount = 1, imagePath = "https://firebasestorage.googleapis.com/v0/b/chat-app-ee53e.appspot.com/o/Hera.png?alt=media&token=c3e02820-fe61-4ff7-8974-e1cdb9e0a851"},
                };
        }
    }

    private AudioSource _walkAudio;
    private AudioSource _shootingAudio;
    private AudioSource _openInventoryAudio;
    private AudioSource _closeInventoryAudio;
    private AudioSource _hitAudio;

    NumberFormatter formatter;
    // Start is called before the first frame update
    void Start()
    {
        _ = LoadSprite();

        // Khởi tạo mảng quests dựa trên mapType hoặc vị trí hiện tại
        quests = GetQuestsForMap();
        GameObject sceneAudioManager = GameObject.Find("SceneAudioManager").gameObject;
        formatter = new NumberFormatter();

        _walkAudio = transform.GetChild(2).GetChild(0).GetComponent<AudioSource>();
        _shootingAudio = transform.GetChild(2).GetChild(1).GetComponent<AudioSource>();
        _hitAudio = transform.GetChild(2).GetChild(2).GetComponent<AudioSource>();

        _openInventoryAudio = sceneAudioManager.transform.GetChild(3).GetComponent<AudioSource>();
        _closeInventoryAudio = sceneAudioManager.transform.GetChild(4).GetComponent<AudioSource>();


        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        level = StatsManager.instance.playerStats.level;
        manaMax = StatsManager.instance.playerStats.mana;
        bloodMax = StatsManager.instance.playerStats.health;

        expCurrent = StatsManager.instance.playerStats.exp;
        coinCurrent = StatsManager.instance.playerStats.coin;
        manaCurrent = manaMax;
        bloodCurrent = bloodMax;


        rotateShooting = transform.GetChild(0).gameObject;
        rotateWeapon = transform.GetChild(0).GetChild(0).gameObject;

        currentTime = 0;

        Debug.Log(level);
        coinText?.SetText(formatter.FormatNumber(coinCurrent));
        ExpBar.instance.SetLevel(level);
        ExpBar.instance.SetValue(expCurrent / (float)levelList[level - 1]);
        BloodBar.instance.SetValue(1);
        ManaBar.instance.SetValue(1);

        // playerInventoryCanvas_isActive = playerInventoryCanvas.isActiveAndEnabled;
        // playerInventoryCanvas.enabled = playerInventoryCanvas_isActive;

        StartCoroutine(AddQuest());
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

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

            if (playerInventoryCanvas_isActive)
                _openInventoryAudio.Play();
            else
                _closeInventoryAudio.Play();
            // playerInventoryCanvas.enabled = playerInventoryCanvas_isActive;
            playerInventoryCanvas.gameObject.SetActive(playerInventoryCanvas_isActive);
            miniMapCanvas.gameObject.SetActive(!playerInventoryCanvas_isActive);

            if (playerInventoryCanvas_isActive)
                PauseMenuManager.pauseGame();
            else
                PauseMenuManager.unpauseGame();
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
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (EquipmentManager.instance.currentConsumable != null)
            {
                if (consumableOnCooldown == false)
                {
                    UseConsumable();
                }
            }
        }

        if (consumableOnCooldown == true)
        {
            consumableCooldown();
        }

        if (QuestLog.isFinishedAllQuests())
        {
            _endLevelObject.SetActive(true);
            PauseMenuManager.pauseGame();
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

    private Quest CreateQuest(string questName, string questDescription, int type, int amount, string imagePath)
    {
        Quest q = new Quest();
        q.questName = questName;
        q.questDescription = questDescription;
        q.expReward = UnityEngine.Random.Range(50, 100);
        q.goldReward = UnityEngine.Random.Range(500, 2000);
        q.questCategory = 0;
        q.imagePath = imagePath;
        q.objective = new Quest.Objective();
        q.objective.type = (Quest.Objective.Type)type;
        q.objective.amount = amount;
        return q;
    }

    //private void AddQuest()
    //{
    //    Debug.Log("Quest QUant: " + quests.Length);
    //    foreach (var quest in quests)
    //    {
    //        QuestLog.AddQuest(CreateQuest(quest.questName, quest.questDescription, quest.questType, quest.amount, quest.imagePath));
    //    }
    //}

    private IEnumerator AddQuest()
    {
        foreach (var quest in quests)
        {
            QuestLog.AddQuest(CreateQuest(quest.questName, quest.questDescription, quest.questType, quest.amount, quest.imagePath));
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void handleExp(int dataRxp)
    {
        expCurrent += dataRxp;
        if (expCurrent <= levelList[level - 1])
        {
            ExpBar.instance.SetValue(expCurrent / (float)levelList[level - 1]);
        }

        else if (level - 1 < levelList.Length)
        {
            if (expCurrent > levelList[level - 1])
            {
                level += 1;
                expCurrent = 0;
                ExpBar.instance.SetLevel(level);
                ExpBar.instance.SetValue(1);
                ExpBar.instance.SetValue(0);

                StatsManager.instance.playerStats.level = level;
                StatsManager.instance.playerStats.health += 20;
                StatsManager.instance.playerStats.attack += 2;
                StatsManager.instance.playerStats.defense += 2;
            }
        }

        StatsManager.instance.playerStats.exp = expCurrent;
    }

    public void handleBlood(int dataBlood)
    {
        if (dataBlood < 0)
        {
            _hitAudio.Play();
            animator.SetTrigger("Hit");
            dataBlood = (int)(dataBlood * (100f / (100 + StatsManager.instance.playerStats.defense)));
        }
        bloodCurrent += dataBlood;

        if (bloodCurrent <= 0)
        {
            BloodBar.instance.SetValue(0);
            animator.SetTrigger("Dead");

            _endLevelObject.SetActive(true);
            QuestLog.ResetQuestLog();
            QuestManager.instance.Reset();
            PauseMenuManager.unpauseGame();

        }

        //Set data blood
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

    public void handleCoin(int dataCoin)
    {
        coinCurrent += dataCoin;
        StatsManager.instance.playerStats.coin = coinCurrent;
        coinText.SetText(formatter.FormatNumber(coinCurrent));
    }

    public void handleAttack(int dataAttack)
    {
        StatsManager.instance.playerStats.attack += dataAttack;
    }

    public void handleDefense(int dataDefense)
    {
        StatsManager.instance.playerStats.defense += dataDefense;
    }

    void recoverInTimeRange()
    {
        //Handle add exp in time range;
        if (currentTime > invincibleTime && manaCurrent < manaMax)
        {
            manaCurrent += 20;
            if (manaCurrent >= manaMax)
                manaCurrent = manaMax;
            ManaBar.instance.SetValue((float)manaCurrent / manaMax);
            currentTime = 0;
        }
    }

    public void playWalkAudio()
    {
        _walkAudio.Play();
    }

    public void UseConsumable()
    {
        handleBlood(EquipmentManager.instance.currentConsumable.healthBoost);
        handleMana(EquipmentManager.instance.currentConsumable.manaBoost);

        EquipmentManager.instance.currentConsumable.quantity -= 1;
        if (EquipmentManager.instance.currentConsumable.quantity == 0)
        {
            EquipmentManager.instance.currentConsumable = null;
            EquipmentManager.instance.onEquipmentChangedCallback?.Invoke();
        }

        consumableOnCooldown = true;
        consumableCooldownSlider.gameObject.SetActive(true);
    }

    public void consumableCooldown()
    {
        // Increment the timer based on the duration
        cooldownTimer += Time.deltaTime / cooldownDuration;

        // Ensure the timer stays within the range [0, 1]
        cooldownTimer = Mathf.Clamp01(cooldownTimer);

        // Set the slider value based on the timer
        consumableCooldownSlider.value = cooldownTimer;

        // Check if the transition is complete
        if (cooldownTimer >= 1f)
        {
            // Reset the timer to 0 for the next transition
            cooldownTimer = 0f;
            consumableOnCooldown = false;
            consumableCooldownSlider.gameObject.SetActive(false);
        }
    }

    public bool isManaOut()
    {
        return mana <= 0 ? true : false;
    }

    public async Task LoadSprite()
    {
        // Load weapon sprites from Addressables
        String weaponAddress = "Weapon";
        Sprite[] weaponSprites_array;
        List<Sprite> weaponSprites = new List<Sprite>();

        AsyncOperationHandle<Sprite[]> handle_1 = Addressables.LoadAssetAsync<Sprite[]>(weaponAddress);
        await handle_1.Task;

        if (handle_1.Status == AsyncOperationStatus.Succeeded)
        {
            weaponSprites_array = handle_1.Result;
            weaponSprites = new List<Sprite>(weaponSprites_array);
        }
        else
        {
            Debug.LogError("Failed to load sprite with addressable key: " + "Weapon");
        }

        defaultBulletSprite = weaponSprites[192];
    }
}
