using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonController : MonoBehaviourPunCallbacks
{
    PhotonView photonView;

    public GameObject startCanvas;
    public GameObject gameCanvas;
    public GameObject waitingCanvas;
    public GameObject otherDeadCanvas;
    public Transform[] spawns;

    public int playersDead;



    void Start()
    {
        photonView = gameObject.GetPhotonView();

        startCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        waitingCanvas.SetActive(true);

        if (PhotonNetwork.PlayerList.Length > 1)
            photonView.RPC("RPCStartMatch", RpcTarget.AllViaServer);

        base.OnJoinedRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        waitingCanvas.SetActive(true);
        base.OnPlayerLeftRoom(otherPlayer);
    }

    [PunRPC]
    public void RPCStartMatch()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        waitingCanvas.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
            StartSpawns();
        //photonView.RPC("RPCStartSpawns", RpcTarget.All);
    }

    public void StartSpawns()
    {
        PhotonNetwork.Instantiate("EnemyRespawnHorizontal", spawns[0].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnHorizontal", spawns[1].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnVertical", spawns[2].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnVertical", spawns[3].position, Quaternion.identity);
    }

    /*[PunRPC]
    public void RPCStartSpawns()
    {
        PhotonNetwork.Instantiate("EnemyRespawnHorizontal", spawns[0].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnHorizontal", spawns[1].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnVertical", spawns[2].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnVertical", spawns[3].position, Quaternion.identity);
    }*/

    private void Update()
    {
        if (playersDead >= 2)
            photonView.RPC("RPCRestartGame", RpcTarget.AllViaServer);
    }


    [PunRPC]
    public void RPCRestartGame()
    {
        Debug.Log("All dead");
        SceneManager.LoadScene(0);
        PhotonNetwork.Disconnect();
    }
}
