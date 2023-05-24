using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // fieldss
    public float _currentHealth = 100.0f;
    public float _currentMaxHealth = 100.0f;

    // methods
    public void DmgUnit(float dmgAmount){
        if(_currentHealth > 0.0f){
            _currentHealth -= dmgAmount;
        }
        if(_currentHealth < 0.0f){
            _currentHealth = 0.0f;
        }
        Respawn();
    }

    public void DmgUnitOverTime(float dmgAmount, float waitSeconds){
        if(_currentHealth > 0.0f && Math.Round(Time.time, 2) % waitSeconds == 0){
            _currentHealth -= dmgAmount;
        }
        if(_currentHealth < 0.0f){
            _currentHealth = 0.0f;
        }
        Respawn();
    }

    public void HealUnit(float healAmount){
        if(_currentHealth < _currentMaxHealth){
            _currentHealth += healAmount;
        }
        if (_currentHealth > _currentMaxHealth){
            _currentHealth = _currentMaxHealth;
        }
    }

    public void HealUnitOverTime(float healAmount, float waitSeconds){
        if(_currentHealth < _currentMaxHealth && Math.Round(Time.time, 2) % waitSeconds == 0){
            _currentHealth += healAmount;
        }
        if (_currentHealth > _currentMaxHealth){
            _currentHealth = _currentMaxHealth;
        }
    }

    public void Respawn(){
        if(_currentHealth <= 0.0f){
            //transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            Debug.Log("dead");
        }
    }
}
