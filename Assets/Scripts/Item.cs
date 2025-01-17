using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public abstract class Item : ScriptableObject, IItem
{
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool isUsable;
    private int _count;
    public string Name => itemName;
    public string Description => description;
    public bool IsUsable => isUsable;
    public int Count
    {
        get => _count;
        set => _count = value;
    }

    public Sprite Icon => icon;

    public abstract void Use();
}