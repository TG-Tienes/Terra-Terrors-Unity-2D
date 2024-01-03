using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire = true;
    private float timer;
    public float fireRate;
    private GameObject _mainCharacter;
    bool _isOpeningMenu = false;

    private AudioSource _shootingAudio;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _mainCharacter = GameObject.FindWithTag("Main Character");

        _shootingAudio = _mainCharacter.transform.GetChild(2).GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _isOpeningMenu = PauseMenuManager._isOpeningMenu;
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (!_isOpeningMenu)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (!canFire)
            {
                timer += Time.deltaTime;
                if (EquipmentManager.instance.currentWeapon != null)
                {
                    if (timer > EquipmentManager.instance.currentWeapon.fireRate)
                    {
                        canFire = true;
                        timer = 0;
                    }
                }
                else if (timer > fireRate)
                {
                    canFire = true;
                    timer = 0;
                }
            }

            if (Input.GetMouseButton(0) && canFire && !_isOpeningMenu && !_mainCharacter.GetComponent<PlayerControl>().isManaOut())
            {
                _shootingAudio.Play();
                canFire = false;
                Instantiate(bullet, bulletTransform.position, bulletTransform.rotation);

                _mainCharacter.GetComponent<PlayerControl>().handleMana(-1);
            }
        }
    }
}
