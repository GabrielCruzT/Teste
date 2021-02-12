using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colisao : MonoBehaviour
{
    private string informacao;
    public GameObject bola;
    

    void OnTriggerEnter(Collider colisor)
    {
        Debug.Log("Colisão enter");
        StartCoroutine(TrocarPosicao());
    }
    IEnumerator TrocarPosicao()
    {
        yield return new WaitForSeconds(3);
        if(gameObject.transform.position.x > 0)
            gameObject.transform.position = new Vector3(Random.Range(-25, -15), Random.Range(5, -20), 95f);
        else
            gameObject.transform.position = new Vector3(Random.Range(35, 25), Random.Range(5, -20), 95f);
    }

}
