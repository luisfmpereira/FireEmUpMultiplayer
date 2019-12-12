using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovementMultiplayer : MonoBehaviour
{
    public Transform playerTransf;
    public Rigidbody2D playerRB;
    public Animator playerAnim;

    PhotonView photonView;

    //move and look
    public float moveSpeed = 5f;
    public Camera cam;
    private Vector3 mousePos;
    private float angle;

    //shooting
    public Transform muzzlePosition;
    public float fireCooldown = 0.5f;
    private float fireTimer = 0;
    public bool isDead;
    public bool isShotgun;
    public float shotgunTimer;

    PlayerHealthMultiplayer playerHealthMultiplayer;

    void OnEnable()
    {
        playerHealthMultiplayer = this.GetComponent<PlayerHealthMultiplayer>();
        photonView = gameObject.GetPhotonView();
        playerTransf = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    void Update()
    {
        isDead = playerHealthMultiplayer.isDead;
        if (!photonView.IsMine || isDead)
            return;

        fireTimer -= Time.deltaTime;
        photonView.RPC("RPCMovePlayer", RpcTarget.All);
        photonView.RPC("RPCPlayerShoot", RpcTarget.All);

        if (isShotgun)
            shotgunTimer -= Time.deltaTime;

        if (shotgunTimer <= 0)
            isShotgun = false;
    }

    [PunRPC]
    void RPCMovePlayer()
    {
        if (!photonView.IsMine)
            return;
        //move player according to axis inputs
        playerTransf.position = new Vector2(playerTransf.position.x + Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, playerTransf.position.y + Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        var dir = mousePos - playerTransf.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //rotate player according to mouse position
        playerTransf.rotation = Quaternion.Euler(0f, 0f, angle);

    }

    //referencia de angulo para facilitar o código
    float shotgunAngle = 30 * Mathf.Deg2Rad;

    [PunRPC]
    void RPCPlayerShoot()
    {
        //metodo de tiro

        if (!photonView.IsMine)
            return;

        var dir = mousePos - playerTransf.position;

        if (Input.GetButton("Fire1") && fireTimer <= 0)
        {

            fireTimer = fireCooldown;
            //se a shotgun foi ativada, usa este tipo de tiro
            if (isShotgun)
            {
                //3 tiros, 2 deles com angulos e um reto
                PhotonNetwork.Instantiate("PlayerBullet", muzzlePosition.position, playerTransf.transform.rotation).
                gameObject.GetComponent<Rigidbody2D>().AddForce(700 * new Vector2(dir.x * Mathf.Sin(shotgunAngle), dir.y * Mathf.Cos(shotgunAngle)).normalized);
                PhotonNetwork.Instantiate("PlayerBullet", muzzlePosition.position, playerTransf.transform.rotation).
                gameObject.GetComponent<Rigidbody2D>().AddForce(700 * new Vector2(dir.x * Mathf.Cos(-shotgunAngle), dir.y * Mathf.Sin(shotgunAngle)).normalized);
                PhotonNetwork.Instantiate("PlayerBullet", muzzlePosition.position, playerTransf.transform.rotation).
                gameObject.GetComponent<Rigidbody2D>().AddForce(700 * new Vector2(dir.x, dir.y).normalized);

            }
            //se não é shotgun, usa esse tiro
            else
            {
                PhotonNetwork.Instantiate("PlayerBullet", muzzlePosition.position, playerTransf.transform.rotation).
                gameObject.GetComponent<Rigidbody2D>().AddForce(700 * new Vector2(dir.x, dir.y).normalized);
            }

        }


    }

    //no outro script, esse metodo é chamado quando começa a shotgun
    public void StartShotgun()
    {
        isShotgun = true;
        shotgunTimer = 10f;
    }

}
