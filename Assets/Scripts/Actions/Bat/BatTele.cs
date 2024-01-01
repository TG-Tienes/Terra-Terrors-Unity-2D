using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTele : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _player;
    public Animator _animator;
    private float timer = 0;

    bool _canTeleport;
    bool _canAttack;
    AudioSource _teleAudio;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Main Character");
        _teleAudio = transform.GetChild(0).GetChild(5).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = _player.transform.position;
        float distance = Vector2.Distance(transform.position, playerPos);

        if (distance < 5)
        {
            timer += Time.deltaTime;

            if (timer > 5)
            {
                _animator.SetTrigger("Teleport");
                timer = 0;
            }
        }

        if (_canTeleport)
        {
            Vector2 tempPos = playerPos;
            tempPos.x += 0.2f;
            _animator.SetFloat("LOOK X", -playerPos.x);
            gameObject.transform.position = tempPos;
            _animator.SetTrigger("Appear");

            _canTeleport = false;
        }

        if (_canAttack)
        {
            _animator.SetTrigger("Attack");
            _canAttack = false;
        }
    }

    void teleportToPlayer()
    {
        _canTeleport = true;
        _teleAudio.Play();

    }

    void attackPlayer()
    {
        _canAttack = true;
    }
}
