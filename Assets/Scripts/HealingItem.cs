using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Items/Healing Item")]
public class HealingItem : Item
{
    [SerializeField] private int healAmount;

    public override void Use()
    {
        Debug.Log($"Использован {Name}. Восстановлено {healAmount} здоровья.");
        
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeHeal(healAmount); // Восстанавливаем здоровье
        }
    }
}