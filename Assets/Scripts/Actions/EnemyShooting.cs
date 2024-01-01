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
    public bool _isMultipleShooting = false;
    public int _multipleShootingProjectileCount = 1;
    public bool _useAudio = true;

    private AudioSource _shootSound;

    // Start is called before the first frame update
    void Start()
    {
        _canFire = false;
        _player = GameObject.FindGameObjectWithTag("Main Character");
        _shootSound = this.transform.GetChild(0).GetChild(3).GetComponent<AudioSource>();
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
            if (_isMultipleShooting)
            {
                bullet.GetComponent<EnemyBulletScript>()._flightPos = 1;
                shoot();

                bullet.GetComponent<EnemyBulletScript>()._flightPos = 0;
                shoot();

                bullet.GetComponent<EnemyBulletScript>()._flightPos = -1;
                shoot();
            }
            else
                shoot();
            _canFire = false;
        }
    }

    void shoot()
    {
        if(_useAudio)
            _shootSound.Play();

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
