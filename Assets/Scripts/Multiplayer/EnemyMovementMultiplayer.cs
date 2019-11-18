using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

public class EnemyMovementMultiplayer : MonoBehaviourPunCallbacks
{
    public int enemyScore = 100;

    //move variables
    public List<GameObject> players = new List<GameObject>();
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


    void Start()
    {
        photonView = gameObject.GetPhotonView();
        enemyTransf = GetComponent<Transform>();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        player = players[Random.Range(0, players.Count)];

        shootingCounter = 0f;

        //playerArray = GameObject.FindGameObjectsWithTag("Player");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        //playerArray = GameObject.FindGameObjectsWithTag("Player");
        //player = playerArray[Random.Range(0, playerArray.Length)];
        players.RemoveRange(0, players.Count);
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        player = players[Random.Range(0, players.Count)];

        base.OnPlayerLeftRoom(otherPlayer);
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
        /*if (!photonView.IsMine)
            return;*/

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
