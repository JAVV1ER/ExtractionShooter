using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  
    
    private Slider _healthSlider;    
    private int _currentHealth;     

    void Start()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        _currentHealth = maxHealth; 
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthSlider.value = _currentHealth; 

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
        Destroy(gameObject);
    }
    
}
