using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTimer : MonoBehaviour
{
    public float apperarTimer;
    public int _damageDeal = 50;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > apperarTimer)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Main Character"))
        {
            collision.gameObject.GetComponent<PlayerControl>().handleBlood(-_damageDeal);
        }
    }
}
