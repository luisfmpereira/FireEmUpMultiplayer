using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NukeDropManager : MonoBehaviour
{
    public bool nukeActivated;
    public float waitingTime = 5f;

    public void ActivateNuke()
    {
        if (nukeActivated)
            return;
        nukeActivated = true;
    }

    public void Update()
    {
        if (!nukeActivated)
            waitingTime -= Time.deltaTime;
        if (waitingTime <= 0)
            Destroy(this.gameObject);
        if (nukeActivated)
        {
            PhotonNetwork.Instantiate("NukeCollider", this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


}
