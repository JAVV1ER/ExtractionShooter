using UnityEngine;

public interface IItem
{
    string Name { get; }         
    string Description { get; }
    int Count { get; set; }
    bool IsUsable { get; }
    Sprite Icon { get; }         
    void Use();                  
}