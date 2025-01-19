using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;  
    
    private Slider _healthSlider;    
    private int _currentHealth;     

    void Start()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        if(!_healthSlider) Debug.LogError("No HealthSlider attached");

        _currentHealth = maxHealth; 
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        _healthSlider.value = _currentHealth; 
    }
    public void TakeHeal(int heal)
    {
        if(_currentHealth == maxHealth) return;
        _currentHealth += heal;
        if (_currentHealth > maxHealth) _currentHealth = maxHealth;
        _healthSlider.value = _currentHealth; 
    }
    

    void Die()
    {
        Debug.Log("Player Died");
        Destroy(gameObject);
    }
    
}
