using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerHealthMultiplayer : MonoBehaviour
{

    public int maxHealth = 5;
    private int currentHealth;
    public bool isDead;
    public Text healthText;
    public bool otherPlayerDead;
    public GameObject otherDeadCanvas;
    PhotonView photonView;
    PhotonController photonController;
    PlayerMovementMultiplayer playerMovementMultiplayer;

    private void Start()
    {
        photonView = gameObject.GetPhotonView();
        photonController = GameObject.FindGameObjectWithTag("PhotonController").GetComponent<PhotonController>();
        currentHealth = maxHealth;
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
        UpdateHealth(0);
        otherDeadCanvas = GameObject.FindGameObjectWithTag("OtherDeadCanvas").transform.GetChild(0).gameObject;
        otherDeadCanvas.SetActive(false);
        playerMovementMultiplayer = this.GetComponent<PlayerMovementMultiplayer>();

    }



    private void Update()
    {
        if (!otherPlayerDead)
            otherDeadCanvas.SetActive(false);


        if (currentHealth <= 0)
        {
            PlayerDead();
            if (!photonView.IsMine)
                otherDeadCanvas.SetActive(true);
        }
    }

    private void UpdateHealth(int healthValue)
    {
        currentHealth += healthValue;

        if (photonView.IsMine)
            healthText.text = "HP: " + currentHealth.ToString();

        if (currentHealth <= 0)
        {
            photonController.playersDead++;
        }
    }

    public void PlayerDead()
    {
        isDead = true;
    }


    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (!isDead && hit.gameObject.CompareTag("Enemy"))
        {
            UpdateHealth(-1);
        }

        if (isDead && hit.gameObject.CompareTag("Player"))
        {
            UpdateHealth(10);
            isDead = false;
            photonController.playersDead--;
        }
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!isDead && hit.gameObject.CompareTag("EnemyBullet"))
        {
            UpdateHealth(-1);
            Destroy(hit.gameObject);
        }

        if (!isDead && hit.gameObject.CompareTag("Food"))
        {
            UpdateHealth(+1);
            Destroy(hit.gameObject);
        }

        if (!isDead && hit.gameObject.CompareTag("NewWeapon"))
        {
            playerMovementMultiplayer.StartShotgun();
            Destroy(hit.gameObject);
        }
    }




}
