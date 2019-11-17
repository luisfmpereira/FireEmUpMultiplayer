using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class PhotonController : MonoBehaviourPunCallbacks
{
    PhotonView thisPhotonView;

    public GameObject startCanvas;
    public GameObject gameCanvas;
    public GameObject waitingCanvas;


    public Transform[] spawns;

    void Start()
    {
        thisPhotonView = gameObject.GetPhotonView();

        startCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        waitingCanvas.SetActive(true);

        if (PhotonNetwork.PlayerList.Length > 1)
            thisPhotonView.RPC("RPCStartMatch", RpcTarget.AllViaServer);

        base.OnJoinedRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
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
            photonView.RPC("RPCStartSpawns", RpcTarget.All);
    }
    [PunRPC]
    public void RPCStartSpawns()
    {


        PhotonNetwork.Instantiate("EnemyRespawnHorizontal", spawns[0].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnHorizontal", spawns[1].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnVertical", spawns[2].position, Quaternion.identity);
        PhotonNetwork.Instantiate("EnemyRespawnVertical", spawns[3].position, Quaternion.identity);

    }



}
