using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyRespawnMultiplayer : MonoBehaviour
{
    public float respawnRate = 5f;
    public float respawnCounter;
    public Transform respawnArea;
    public GameObject[] newEnemy;
    public bool horizontalSpawn;

    public int numberOfEnemies = 1;

    PhotonView photonView;

    void Start()
    {
        photonView = gameObject.GetPhotonView();
        respawnArea = this.GetComponent<Transform>();

    }


    void Update()
    {
        respawnCounter -= Time.deltaTime;

        numberOfEnemies = (1 + (int)Time.deltaTime / 50000);

        if (respawnCounter <= 0)
        {
            respawnCounter = respawnRate;
            photonView.RPC("RPCSpawnEnemy", RpcTarget.All, horizontalSpawn, numberOfEnemies);

        }

    }
    [PunRPC]
    void RPCSpawnEnemy(bool horizontal, int amount)
    {

        var selectedEnemy = Random.Range(0, newEnemy.Length);
        if (horizontal)
        {
            for (int i = 0; i < amount; i++)
            {
                if (selectedEnemy == 0)
                    PhotonNetwork.Instantiate("EnemyMultiplayer1", new Vector3(Random.Range(-10, 10), respawnArea.position.y, 0), Quaternion.identity);
                else
                    PhotonNetwork.Instantiate("EnemyMultiplayer2", new Vector3(Random.Range(-10, 10), respawnArea.position.y, 0), Quaternion.identity);
            }
        }
        else
        {
            for (int i = 0; i <= amount; i++)
            {
                if (selectedEnemy == 0)
                    PhotonNetwork.Instantiate("EnemyMultiplayer1", new Vector3(respawnArea.position.x, Random.Range(-10, 10), 0), Quaternion.identity);
                else
                    PhotonNetwork.Instantiate("EnemyMultiplayer2", new Vector3(respawnArea.position.x, Random.Range(-10, 10), 0), Quaternion.identity);
            }

        }
    }

}
