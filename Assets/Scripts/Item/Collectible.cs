using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
        if (player != null)
        {
            Item itemCopy = Instantiate(item);
            Inventory.instance.AddItem(itemCopy);
            Destroy(gameObject);
        }
    }
}


