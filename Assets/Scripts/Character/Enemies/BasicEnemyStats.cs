using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyStats : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Transform target;
    
    public float _speed;
    public Animator _animator;
    [SerializeField] BasicHealthBar healthBar;
    [SerializeField] float _armorStat;
    [SerializeField] float health, maxHealth;
    private GameObject _mainCharacter;
    public GameObject _damageTakenText;
    public bool _canDestroyGameObject = false;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar = gameObject.GetComponentInChildren<BasicHealthBar>();
        healthBar.updateHealthBar(health, maxHealth);

        _mainCharacter = GameObject.FindWithTag("Main Character");
    }

    // Update is called once per frame
    void Update()
    {
        if (_canDestroyGameObject)
        {
            Destroy(gameObject);
        }

        if (health <= 0 && !_canDestroyGameObject)
        {
            Debug.Log("DEAD HIHI");
            _animator.SetTrigger("Dead");

            //Add exp for main character
            _mainCharacter.GetComponent<PlayerControl>().handleExp(15);
        }
    }

    void takeDamage(float damageAmount)
    {
        if (health > 0)
        {
            // formula base on LOL physical armor
            float realDamageAmountTaken = damageAmount * 100 / (100 + _armorStat);

            DamageIdicator idicator = Instantiate(_damageTakenText, transform.position, Quaternion.identity).GetComponent<DamageIdicator>();
            idicator.SetDamageText(realDamageAmountTaken);

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
                takeDamage(StatsManager.instance.playerStats.attack + EquipmentManager.instance.currentWeapon.attackModifier);
            }
            else
            {
                takeDamage(StatsManager.instance.playerStats.attack);
            }
        }
        if (collision.gameObject.tag.Equals("Main Character"))
        {
            _mainCharacter.GetComponent<PlayerControl>().handleBlood(-15);
        }
    }

    public void removeBody()
    {
        _canDestroyGameObject = true;
    }
}
