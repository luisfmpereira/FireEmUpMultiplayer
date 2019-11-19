using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerHealthMultiplayer : MonoBehaviour
{

    public int maxHealth = 5;
    private int currentHealth;
    public bool isDead;
    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
        UpdateHealth(0);
    }

    private void UpdateHealth(int healthValue)
    {
        currentHealth += healthValue;
        if (currentHealth <= 0)
            isDead = true;

        healthText.text = "HP: " + currentHealth.ToString();
    }

    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (!isDead && hit.gameObject.CompareTag("Enemy"))
        {
            UpdateHealth(-1);
        }

    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!isDead && hit.gameObject.CompareTag("EnemyBullet"))
        {
            UpdateHealth(-1);
        }
    }



}
