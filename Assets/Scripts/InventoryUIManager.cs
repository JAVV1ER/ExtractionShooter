using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    private bool _isInventoryOpen = false;
    private bool _isItemClicked = true;
    private Inventory _inventory;

    [SerializeField] private GameObject buttonPrefab; 
    [SerializeField] private GameObject useBtnPrefab;
    [SerializeField] private GameObject deleteBtnPrefab;
    

    public void ToggleInventory()
    {
        _inventory = FindObjectOfType<Inventory>();
        if (!_inventory)
        {
            Debug.LogError("Inventory is null");
            gameObject.SetActive(false);
            return;
        }
        _isInventoryOpen = !_isInventoryOpen;
        _isItemClicked = true;
        gameObject.SetActive(_isInventoryOpen);

        //обновление инвентаря в реальном времени
        if (_isInventoryOpen)
        {
            UpdateInventoryUI();
            _inventory.OnItemAdded += OnItemChange;
            _inventory.OnItemRemoved += OnItemChange;
            _inventory.OnItemUsed += OnItemChange;
        }
        else
        {
            _inventory.OnItemAdded -= OnItemChange;
            _inventory.OnItemRemoved -= OnItemChange;
            _inventory.OnItemUsed -= OnItemChange;
        }

    }

    private void OnItemChange(IItem item)
    {
        UpdateInventoryUI();
    }
    private void UpdateInventoryUI()
    {
        // Удаляем старые кнопки
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Получаем список предметов
        var items = _inventory.GetItems();
        if (items == null || items.Count == 0) return;

        // Создаём кнопки для каждого предмета
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            // Создаём кнопку из префаба
            GameObject newButton = Instantiate(buttonPrefab, transform);
            newButton.name = item.Name;

            // Устанавливаем иконку предмета
            Image icon = newButton.GetComponent<Image>();
            icon.sprite = item.Icon;
            
            TMP_Text text = newButton.GetComponentInChildren<TMP_Text>();
            text.text = item.Count > 1 ? item.Count.ToString() : "";
            // Привязываем событие нажатия
            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => ShowItemActions(item, newButton));
            }
        }
    }

    private void ShowItemActions(IItem item, GameObject itemButton)
    {
        Debug.Log($"Кнопка предмета нажата: {item.Name}");
    
        // Удаляем старую панель действий, если есть
        foreach (Transform child in itemButton.transform)
        {
            if (child.name == "ActionPanel")
            {
                Destroy(child.gameObject);
                return;
            }
        }

        //_isItemClicked = !_isItemClicked; 
        //if (_isItemClicked) return;

        // Панель держатель кнопок действий
        GameObject actionPanel = new GameObject("ActionPanel");
        actionPanel.transform.SetParent(itemButton.transform, false); // Привязываем к панели инвентаря

        // Настраиваем Rect
        RectTransform invRect = GetComponent<RectTransform>();
        RectTransform itemBtnRect = itemButton.GetComponent<RectTransform>();
        RectTransform actionPanelRect = actionPanel.AddComponent<RectTransform>();
        actionPanelRect.sizeDelta = new Vector2(itemBtnRect.rect.width, itemBtnRect.rect.height); // Размер панели действий
        actionPanelRect.anchorMin = new Vector2(0.5f, 0.5f); // Привязываем к левому центру
        actionPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        actionPanelRect.pivot = new Vector2(0, 0f); // Точка поворота справа
        actionPanelRect.anchoredPosition = new Vector2(-itemBtnRect.rect.width/2, -itemBtnRect.rect.height); // Смещаем влево от панели

        //Если можно, добавим кнопку использовать
        if (item.IsUsable)
        {
            Button useButton = Instantiate(useBtnPrefab, actionPanel.transform).GetComponent<Button>();
            useButton.onClick.AddListener(() => UseItem(item));
            RectTransform useButtonRect = useButton.GetComponent<RectTransform>();
            useButtonRect.anchoredPosition = new Vector2(0, 100); // Смещение кнопки "Использовать"
        }
        
        Button deleteButton = Instantiate(deleteBtnPrefab, actionPanel.transform).GetComponent<Button>();
        deleteButton.onClick.AddListener(() => DeleteItem(item,itemButton,actionPanel));
        RectTransform deleteButtonRect = deleteButton.GetComponent<RectTransform>();
        deleteButtonRect.anchoredPosition = new Vector2(0, -100); // Смещение кнопки "Удалить"
    }

    private void UseItem(IItem item)
    {
        Debug.Log($"Использован предмет: {item.Name}");
        _inventory.UseItem(item);
        UpdateInventoryUI();
    }

    private void DeleteItem(IItem item, GameObject itemButton, GameObject actionPanel)
    {
        _inventory.RemoveItem(item);
        UpdateInventoryUI();
        
    }
}
