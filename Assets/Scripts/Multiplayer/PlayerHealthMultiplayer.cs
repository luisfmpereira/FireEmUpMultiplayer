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
    public bool otherPlayerDead;

    public GameObject otherDeadCanvas;

    PhotonView photonView;

    private void Start()
    {
        photonView = gameObject.GetPhotonView();
        currentHealth = maxHealth;
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<Text>();
        UpdateHealth(0);
        otherDeadCanvas = GameObject.FindGameObjectWithTag("OtherDeadCanvas").transform.GetChild(0).gameObject;
        otherDeadCanvas.SetActive(false);
    }

    public void PlayerDead()
    {
        photonView.RPC("RPCOtherPlayerDead", RpcTarget.Others);
        isDead = true;
    }

    [PunRPC]
    void RPCOtherPlayerDead()
    {
        otherPlayerDead = true;
    }

    private void Update()
    {
        if (otherPlayerDead)
            otherDeadCanvas.SetActive(true);
        else
            otherDeadCanvas.SetActive(false);
    }

    private void UpdateHealth(int healthValue)
    {
        currentHealth += healthValue;
        if (currentHealth <= 0)
        {
            PlayerDead();
            photonView.RPC("RPCOtherPlayerDead", RpcTarget.Others);
        }
        if (photonView.IsMine)
            healthText.text = "HP: " + currentHealth.ToString();
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
        }
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!isDead && hit.gameObject.CompareTag("EnemyBullet"))
        {
            UpdateHealth(-1);
        }

        if (!isDead && hit.gameObject.CompareTag("Food"))
        {
            UpdateHealth(+1);
            Destroy(hit.gameObject);
        }

    }

}
