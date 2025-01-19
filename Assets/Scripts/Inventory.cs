using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<IItem> _items = new List<IItem>(); // Список предметов
    public event Action<IItem> OnItemAdded;
    public event Action<IItem> OnItemUsed;
    public event Action<IItem> OnItemRemoved;
    
    public void AddItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Count++;
            Debug.Log($"Добавлен предмет: {item.Name}, Количество: {item.Count}");
            OnItemAdded?.Invoke(item);
            return;
        }
        _items.Add(item);
        item.Count = 1;
        OnItemAdded?.Invoke(item);
        Debug.Log($"Добавлен предмет: {item.Name}");
        // Пробуем найти BulletItem
        
    }

    public void RemoveItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Count--;
            if (item.Count <= 0) _items.Remove(item);
            OnItemRemoved?.Invoke(item);
            Debug.Log($"Удален предмет: {item.Name}, осталось: {item.Count}");
        }
    }
    public List<IItem> GetItems() => _items;

    public bool TryGetItem<T>( out T result) 
        where T : class, IItem
    {
        foreach (var item in _items)
        {
            if (item is T typedItem)
            {
                result = typedItem;
                return true;
            }
        }

        result = null;
        return false;
    }

    public void UseItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Use();
            item.Count--;
            OnItemUsed?.Invoke(item);
            if(item.Count < 1)
                RemoveItem(item);
        }
    }
}