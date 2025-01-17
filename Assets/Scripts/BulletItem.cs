using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Item", menuName = "Items/Bullet Item")]
public class BulletItem : Item
{
    [SerializeField] private int damageAmount;

    public override void Use() { }
}