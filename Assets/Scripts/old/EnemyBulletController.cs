using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{


    void Update()
    {

        Destroy(this.gameObject, 5);

    }
}
