using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacteristics : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealthValue;

    public HealthChangedEvent OnHealthChanged;

    private void Awake()
    {
        if (_maxHealth > 0)
        {
            _currentHealthValue = _maxHealth;
            OnHealthChanged.Invoke(_currentHealthValue.ToString());
        }
        else
            Debug.LogError("Inncorrect maximum health value");
    }

    public void TakeDamage(float damageValue)
    {
        if (damageValue > 0)
        {
            if (_currentHealthValue - damageValue > 0)
            {
                _currentHealthValue -= damageValue;
                OnHealthChanged.Invoke(_currentHealthValue.ToString());
            }
            else
            {
                _currentHealthValue = 0;
                OnHealthChanged.Invoke(_currentHealthValue.ToString());
                //Destroy player
            }
            Debug.Log("current health = " + _currentHealthValue);
        }
    }

    public void TakeHeal(float healValue)
    {
        if (healValue > 0)
        {
            if (healValue + _currentHealthValue <= _maxHealth)
            {
                _currentHealthValue += healValue;
                OnHealthChanged.Invoke(_currentHealthValue.ToString());
            }
            else
            {
                _currentHealthValue = _maxHealth;
                OnHealthChanged.Invoke(_currentHealthValue.ToString());
            } 
            Debug.Log("current health = " + _currentHealthValue);
        } 
    }
}

[System.Serializable]
public class HealthChangedEvent : UnityEvent<string> { }
