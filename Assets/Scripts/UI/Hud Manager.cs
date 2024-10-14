using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    uint savedMaxHealth = 0;
    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text coinCounterText;
    public void OnHealthInitialized(uint maxHealth, uint currentHealth)
    {
        savedMaxHealth = maxHealth;
        healthBar.value = currentHealth / maxHealth;
    }
    public void OnHealthDamage(DamageData damageData, uint oldHealth, uint newHealth)
    {
        healthBar.value = (float)newHealth / (float)savedMaxHealth;
    }
    public void OnMaxHealthUpdated(uint newMaxHealth)
    {
        uint currentHealth = (uint)Mathf.RoundToInt(savedMaxHealth * healthBar.value);
        savedMaxHealth = newMaxHealth;
        healthBar.value = Mathf.Clamp(currentHealth ,0, savedMaxHealth) / savedMaxHealth;
    }
    public void OnHealthHeal(HealData healData, uint oldHealth, uint newHealth)
    {
        healthBar.value = (float)newHealth / (float)savedMaxHealth;
    }
    public void OnCoinsUpdated(uint newCointCount)
    {
        coinCounterText.text = newCointCount.ToString();
    }

    //////////////////////////////////////////////////////////////////////

    void Awake()
    {
        FindObjectOfType<RunManager>().GetComponent<RunManager>().OnCoinCountUpdated.AddListener(OnCoinsUpdated);
    }
}
