using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb2d;
    public float force;
    public float _bulletRot;
    public bool _useRot;
    public float _flightPos = 0;
    public int _damageDeal = 15;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Main Character");

        Vector3 direction = player.transform.position - transform.position;
        direction.y += _flightPos;
        rb2d.velocity = new Vector2 (direction.x, direction.y).normalized * force;
        
        float rot = 0;
        if (_useRot)
            rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + _bulletRot);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Main Character"))
        {
            collision.gameObject.GetComponent<PlayerControl>().handleBlood(-_damageDeal);

            Destroy(gameObject);
        }
    }
}
