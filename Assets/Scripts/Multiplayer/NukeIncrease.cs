using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeIncrease : MonoBehaviour
{
    void Update()
    {
        StartCoroutine("ExplodeNuke");
    }

    IEnumerator ExplodeNuke()
    {
        this.gameObject.transform.localScale += new Vector3(0.5f, 0.5f, 0f);
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

}
