using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyStats : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Transform target;

    public int expAmount = 15;
    public int coinAmount = 500;

    public float _speed;
    public Animator _animator;
    [SerializeField] BasicHealthBar healthBar;
    [SerializeField] float _armorStat;
    [SerializeField] float health, maxHealth;
    private GameObject _mainCharacter;
    public GameObject _damageTakenText;
    public bool _canDestroyGameObject = false;
    public bool _isBoss;

    bool isDead = false;

    private AudioSource _hitAudio;
    private AudioSource _deadAudio;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar = gameObject.GetComponentInChildren<BasicHealthBar>();
        healthBar.updateHealthBar(health, maxHealth);

        _mainCharacter = GameObject.FindWithTag("Main Character");

        _hitAudio = transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        _deadAudio = transform.GetChild(0).GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   


        if (health <= 0 && !_canDestroyGameObject)
        {
            _animator.SetTrigger("Dead");
            if (!isDead)
            {
                if (_isBoss)
                {
                    QuestManager.instance.RegisterBossKill();
                    QuestLog.CheckQuestObjective(Quest.Objective.Type.killBoss, QuestManager.instance.bossesKilled);
                    Debug.Log("kill boss: " + QuestManager.instance.bossesKilled);
                }
                else
                {
                    QuestManager.instance.RegisterEnemyKill();
                    QuestLog.CheckQuestObjective(Quest.Objective.Type.killEnemy, QuestManager.instance.enemiesKilled);
                    Debug.Log("kill enemy: " + QuestManager.instance.enemiesKilled);

                }
                //Add exp for main character
                PlayerControl.instance.handleExp(expAmount);
                PlayerControl.instance.handleCoin(coinAmount);
                // _mainCharacter.GetComponent<PlayerControl>().handleExp(15);
                // _mainCharacter.GetComponent<PlayerControl>().handleCoin(1500);
                isDead = true;
            }
            Debug.Log("dead");
        }

        if (_canDestroyGameObject)
        {
            Destroy(gameObject);
        }
    }

    void takeDamage(float damageAmount, bool isCrit)
    {
        if (health > 0)
        {
            // formula base on LOL physical armor
            float realDamageAmountTaken = damageAmount * 100 / (100 + _armorStat);

            DamageIdicator idicator = Instantiate(_damageTakenText, transform.position, Quaternion.identity).GetComponent<DamageIdicator>();
            if (isCrit) idicator.SetTextColor();
            idicator.SetDamageText(Mathf.Round(realDamageAmountTaken));

            health -= realDamageAmountTaken;
            healthBar.updateHealthBar(health, maxHealth);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player Projectile"))
        {
            _animator.SetTrigger("Hit");
            if (EquipmentManager.instance.currentWeapon != null)
            {
                // Get player base attack and current weapon attack
                int damageDealt = StatsManager.instance.playerStats.attack + EquipmentManager.instance.currentWeapon.attackModifier;
                // If critical
                bool isCriticalHit = Random.value < EquipmentManager.instance.currentWeapon.criticalChance;
                damageDealt = isCriticalHit ? damageDealt * 2 : damageDealt;
                // Enemy take damage
                takeDamage(damageDealt, isCriticalHit);
            }
            else
            {
                takeDamage(StatsManager.instance.playerStats.attack, false);
            }
        }

        if (collision.gameObject.tag.Equals("Main Character"))
        {
            _mainCharacter.GetComponent<PlayerControl>().handleBlood(-20);
        }
    }

    public void removeBody()
    {
        _canDestroyGameObject = true;
    }

    void playHitSound()
    {
        _hitAudio.Play();
        Debug.Log("HIT SOUND");
    }

    void playDeadSound()
    {
        _deadAudio.Play();
    }
}
