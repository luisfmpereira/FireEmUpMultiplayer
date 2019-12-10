using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeDropManager : MonoBehaviour
{

    public GameObject nukeCollider;
    public float nukeTimerMax = 10f;
    public float nukeTimer = 0;

    void Start()
    {
        nukeTimer = nukeTimerMax;
        nukeCollider = this.transform.GetChild(0).gameObject;
        nukeCollider.SetActive(true);
        nukeCollider.transform.localScale = new Vector3(0.0001f, 0.0001f, 1f);
    }


    public void ActivateNuke()
    {
        if (nukeTimer >= 0)
        {
            nukeCollider.transform.localScale += new Vector3(0.1f, 0.1f, 0f);
            nukeTimer -= Time.deltaTime;
        }
        else
            nukeCollider.SetActive(false);
    }

}
