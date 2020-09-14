using UnityEngine;
using System.Collections;

public class Controle_do_carrinho : MonoBehaviour
{
    private CharacterController controle;
    private float velocidade = 20.0f;
    

    // Use this for initialization
    void Start ()
    {
        controle = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
        controle.Move(Vector3.forward * velocidade * Time.deltaTime);
	}
}
