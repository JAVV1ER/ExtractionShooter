using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{ 
    [SerializeField] private Item item;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(!_spriteRenderer) Debug.LogError("No spriteRenderer attached");
        _spriteRenderer.sprite = item.Icon;
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