using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject attack1Smoke;
    private GameObject _player;
    public Animator _animator;
    private float timer = 0;

    bool attac1AnimEnd;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Main Character");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = _player.transform.position;
        float distance = Vector2.Distance(transform.position, playerPos);

        if (distance < 0.5)
        {
            timer += Time.deltaTime;

            if(timer > 2)
            {
                _animator.SetTrigger("Attack");
                timer = 0;
            }
        }

        if(attac1AnimEnd)
        {
            Vector2 pos = transform.position;
            if (_animator.GetFloat("LOOK X") > 0)
            {
                pos.x += 0.25f;
            }
            else
                pos.x -= 1;
            Instantiate(attack1Smoke, pos, Quaternion.identity);
            attac1AnimEnd = false;
        }
    }

    void setAttack1()
    {
        attac1AnimEnd = true;
    }

    void setAttack2()
    {

    }
}
