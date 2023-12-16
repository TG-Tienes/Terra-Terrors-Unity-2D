using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectPlayer : MonoBehaviour
{
    public float _moveSpeed = 0.5f;

    private Rigidbody2D _rb2d;
    private Vector2 _moveDirection;
    private Animator _animator;
    private GameObject _mainCharacter;
    private bool _isDetected;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _isDetected = false;

        while (true)
        {
            _mainCharacter = GameObject.FindWithTag("Main Character");

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDetected)
        {
            Vector2 direction = (_mainCharacter.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y);

            _rb2d.rotation = angle;
            _moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("Speed", _moveDirection.magnitude);

        if (_isDetected)
        {
            // move
            _rb2d.velocity = new Vector2(_moveDirection.x, _moveDirection.y) * _moveSpeed;

            // play moving audio
            //playAudioSource(footStepAudio);

            // change animation base on movement
            _animator.SetFloat("LOOK X", _moveDirection.x);
            //_animator.SetFloat("Look Y", _moveDirection.y);
        }
        else // stop moving if not detect player
        {
            _rb2d.velocity = Vector2.zero;
            _moveDirection = Vector2.zero;
        }
    }

    // Detect main character enter detect radius of collider trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Main Character"))
            _isDetected = true;
    }

    // Detect main character get out of detect radius of collider trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Main Character"))
            _isDetected = false;
    }
}
