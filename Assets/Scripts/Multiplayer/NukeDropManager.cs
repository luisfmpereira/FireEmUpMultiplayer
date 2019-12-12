using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeDropManager : MonoBehaviour
{
    //objeto do collider que mata os inimigos
    public GameObject nukeCollider;
    //booleana que indica que o nuke esta ativo
    public bool nukeActivated;
    //tempo máximo que a fruta espera o jogador pegar
    public float waitingTime = 5f;


    //este metodo e' chamado pelo jogador quando toca na fruta de nuke
    public void ActivateNuke()
    {
        //se o nuke ja foi ativado, ignora a ação
        if (nukeActivated)
            return;

        //liga o objeto
        nukeCollider.SetActive(true);
        //ativa o processo do nuke
        nukeActivated = true;
    }

    public void Update()
    {
        //se o nuke não foi ativado, o tempo para desligar é contado
        if (!nukeActivated)
            waitingTime -= Time.deltaTime;
        //se o tempo chega a zero, este objeto é destruido
        if (waitingTime <= 0)
            Destroy(this.gameObject);

        //quando o nuke é ativado, ele ativa a corrotina
        if (nukeActivated)
            StartCoroutine("ExplodeNuke");

    }

    IEnumerator ExplodeNuke()
    {
        //a corrotina aumenta o tamanho do collider, espera 10 segundos e destroi o objeto no final
        nukeCollider.transform.localScale += new Vector3(0.5f, 0.5f, 0f);
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);

    }

}
