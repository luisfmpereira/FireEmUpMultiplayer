using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyMovementMultiplayer : MonoBehaviour
{
    public int enemyScore = 100;

    //move variables
    public GameObject[] playerArray;
    public GameObject player;
    private Transform enemyTransf;
    public float moveSpeed = 1f;

    //shooting variables
    public float cooldown = 2f;
    private float shootingCounter;
    public Rigidbody2D bulletPrefab;
    public bool allowShooting;

    //health and drops
    public int enemyHealth = 1;
    public GameObject[] foodPrefabs;
    public float dropChance;
    PhotonView photonView;


    void Awake()
    {
        photonView = gameObject.GetPhotonView();
        enemyTransf = GetComponent<Transform>();
        playerArray = GameObject.FindGameObjectsWithTag("Player");

        shootingCounter = 0f;

        player = playerArray[Random.Range(0, playerArray.Length)];
    }


    void Update()
    {
        var direction = (player.transform.position - enemyTransf.position);

        MoveEnemy(direction);


        if (allowShooting)
        {
            shootingCounter += Time.deltaTime;

            //ShootEnemy();
            photonView.RPC("RPCShootEnemy", RpcTarget.All, direction);
        }

        if (enemyHealth <= 0)
            photonView.RPC("RPCKillEnemy", RpcTarget.All);
    }

    void MoveEnemy(Vector3 direction)
    {
        enemyTransf.position = Vector2.MoveTowards(enemyTransf.position, player.transform.position, moveSpeed * Time.deltaTime);

        //rotate enemy sprite towards player
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        enemyTransf.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    [PunRPC]
    void RPCShootEnemy(Vector3 direction)
    {
        if (!photonView.IsMine)
            return;

        if (shootingCounter >= cooldown)
        {
            shootingCounter = 0;
            /*
            var bullet = Instantiate(bulletPrefab, enemyTransf.position, enemyTransf.rotation) as Rigidbody2D;
            bullet.AddForce(300 * new Vector2(direction.x, direction.y).normalized);
            */
            PhotonNetwork.Instantiate("EnemyBullet", this.transform.position, this.transform.rotation).gameObject.
            GetComponent<Rigidbody2D>().AddForce(300 * new Vector2(direction.x, direction.y).normalized);
        }
    }

    [PunRPC]
    void RPCKillEnemy()
    {
        if (!photonView.IsMine)
            return;
            
        float drop = Random.Range(0, 10);
        /*
        var foodOfChoice = Random.Range(0, foodPrefabs.Length);

         if (drop <= dropChance)
         {
             GameObject food = Instantiate(foodPrefabs[foodOfChoice], this.transform.position, Quaternion.identity);
             Destroy(food.gameObject, 5);
         }
        */

        PhotonNetwork.Destroy(this.gameObject);

    }


    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("PlayerBullet"))
            enemyHealth--;
    }
}
