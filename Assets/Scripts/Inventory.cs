using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<IItem> _items = new List<IItem>(); // Список предметов

    public void AddItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Count++;
            Debug.Log($"Добавлен предмет: {item.Name}, Количество: {item.Count}");
            return;
        }
        _items.Add(item);
        item.Count = 1;
        Debug.Log($"Добавлен предмет: {item.Name}");
    }

    public void RemoveItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Count--;
            if (item.Count <= 0) _items.Remove(item);
            Debug.Log($"Удален предмет: {item.Name}, осталось: {item.Count}");
            return;
        }
        _items.Remove(item);
        Debug.Log($"Удалён предмет: {item.Name}");
    }
    public List<IItem> GetItems() => _items;

    public void UseItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Use();
            item.Count--;
            if(item.Count < 1)
                RemoveItem(item);
        }
    }
}