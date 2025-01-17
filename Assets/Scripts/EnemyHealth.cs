using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int _currentHealth;
    [SerializeField] private List<DeathDrop> deathDrop;
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
        //Instantiate(deathDrop[Random.Range(0, deathDrop.Count)].gameObject, transform.position, Quaternion.identity);
        
        for (int i = 0; i < deathDrop.Count; i++)
            for (int j = 0; j < deathDrop[i].count; j++)
                Instantiate(deathDrop[i].gameObject, transform.position, Quaternion.identity);
        
        
        Destroy(gameObject);
    }
    void SetHealth(int currentHealth) => _healthSlider.value = currentHealth;

    [Serializable]
    public struct DeathDrop
    {
        public GameObject gameObject;
        public int count;
    }
}
