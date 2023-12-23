using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private GameObject _player;
    public Animator _animator;
    private float timer;
    bool _canFire;
    public string _animationName;
    public float _shootDistance;
    public float _spaceDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        _canFire = false;
        _player = GameObject.FindGameObjectWithTag("Main Character");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);

        if(distance < _shootDistance && distance >= _spaceDistance)
        {
            timer += Time.deltaTime;

            if(timer > 2) {
                timer = 0;
                _animator.SetTrigger(_animationName);
            }
        }


        if (_canFire)
        {
            shoot();
            _canFire = false;
        }
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    void enableFire()
    {
        _canFire = true;
    }

    void disableFire()
    {
        _canFire = true;
    }
}
