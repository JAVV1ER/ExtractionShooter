using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int _currentHealth;
    
    private Slider _healthSlider;
    
    void Start()
    {
        _healthSlider = gameObject.GetComponentInChildren<Slider>();
        if(_healthSlider == null)
            Debug.LogError("HEALTH SLIDER IS NULL");
        _currentHealth = maxHealth;
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth; 
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        SetHealth(_currentHealth);
        //Debug.Log(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
    void SetHealth(int currentHealth) => _healthSlider.value = currentHealth; 
}
