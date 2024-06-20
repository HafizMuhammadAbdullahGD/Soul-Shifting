using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image _imgHealthBar;
    [SerializeField] private TextMeshProUGUI _txtHealthPercentage;
    [SerializeField] private float _currentHealth;
    public void Damage(float damageRate)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damageRate);
        _imgHealthBar.fillAmount = _currentHealth / 100;
        _txtHealthPercentage.text = _currentHealth + "%";
    }
    public void SetHealth(float newHealth)
    {
        _currentHealth = newHealth;
    }
    public float GetHealth()
    {
        return _currentHealth;
    }
    public bool IsDead()
    {
        return _currentHealth <= 0;
    }
}
