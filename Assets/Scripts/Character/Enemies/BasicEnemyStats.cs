using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyStats : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Transform target;
    float timer;

    public float _speed;
    public Animator _animator;
    [SerializeField] BasicHealthBar healthBar;
    [SerializeField] float _armorStat;
    [SerializeField] float health, maxHealth;
    [SerializeField] float _showDeadBodyTime;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar =  gameObject.GetComponentInChildren<BasicHealthBar>();
        healthBar.updateHealthBar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(health < 0 && timer > _showDeadBodyTime)
        {
            Destroy(gameObject);
        }
    }

    void takeDamage(float damageAmount)
    {
        // formula base on LOL physical armor
        float realDamageAmountTaken = damageAmount * 100 / (100 + _armorStat);

        health -= realDamageAmountTaken;
        healthBar.updateHealthBar(health, maxHealth);

        _animator.SetTrigger("Hit");

        if(health < 0)
        {
            _animator.SetTrigger("Dead");
            timer = 0;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player Projectile"))
        {
            takeDamage(15);
        }
    }
}