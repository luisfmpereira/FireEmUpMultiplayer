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
    public float dropChance = 0.3f;
    PhotonView photonView;

    public GameObject target;
    public Vector3 direction;
    PhotonController photonController;


    void Start()
    {
        photonView = gameObject.GetPhotonView();
        enemyTransf = GetComponent<Transform>();
        shootingCounter = 0f;
        target = FindClosestPlayer();
        photonController = GameObject.FindGameObjectWithTag("PhotonController").GetComponent<PhotonController>();

    }

    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && !go.gameObject.GetComponent<PlayerHealthMultiplayer>().isDead)
            {
                closest = go;
                distance = curDistance;
                direction = go.transform.position - enemyTransf.position;
            }
        }
        return closest;
    }


    void Update()
    {
        target = FindClosestPlayer();

        MoveEnemy(target, direction);


        if (allowShooting)
        {
            shootingCounter += Time.deltaTime;
            ShootEnemy(target, direction);
            //photonView.RPC("RPCShootEnemy", RpcTarget.All, direction);
        }

        if (enemyHealth <= 0)
            photonView.RPC("RPCKillEnemy", RpcTarget.All);
    }


    void MoveEnemy(GameObject player, Vector3 dir)
    {
        enemyTransf.position = Vector2.MoveTowards(enemyTransf.position, player.transform.position, moveSpeed * Time.deltaTime);

        //rotate enemy sprite towards player
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        enemyTransf.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    [PunRPC]
    void RPCShootEnemy(GameObject player, Vector3 dir)
    {
        if (shootingCounter >= cooldown)
        {
            shootingCounter = 0;
            PhotonNetwork.Instantiate("EnemyBullet", this.transform.position, this.transform.rotation).gameObject.
            GetComponent<Rigidbody2D>().AddForce(300 * new Vector2(dir.x, dir.y).normalized);
        }
    }

    void ShootEnemy(GameObject player, Vector3 dir)
    {
        if (shootingCounter >= cooldown)
        {
            shootingCounter = 0;
            Instantiate(bulletPrefab, this.transform.position, this.transform.rotation).gameObject.
            GetComponent<Rigidbody2D>().AddForce(300 * new Vector2(dir.x, dir.y).normalized);
        }
    }


    [PunRPC]
    void RPCKillEnemy()
    {
        if (!photonView.IsMine)
            return;

        //photonView.RPC("RPCDropFood", RpcTarget.All);
        DropFood();
        photonController.UpdateScore();
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void DropFood()
    {
        float drop = Random.Range(0, 1);

        if (drop <= dropChance)
            Instantiate(foodPrefabs[0], this.transform.position, Quaternion.identity);
        //PhotonNetwork.Instantiate("Health1Pack", this.transform.position, Quaternion.identity);

    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("PlayerBullet"))
            enemyHealth--;
    }


}
