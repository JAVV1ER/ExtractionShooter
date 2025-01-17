using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{ 
    public Item item;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.Icon;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(item);
                Destroy(gameObject); // Уничтожаем предмет на сцене
            }
        }
    }
}