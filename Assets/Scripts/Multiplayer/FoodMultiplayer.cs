using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FoodMultiplayer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Wait5secs());
    }

    IEnumerator Wait5secs()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
