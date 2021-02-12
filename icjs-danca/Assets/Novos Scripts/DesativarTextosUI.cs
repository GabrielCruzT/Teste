using UnityEngine;
using System.Collections;

public class DesativarTextosUI : MonoBehaviour {

    public GameObject coordenadas;
    //public GameObject camera1;
    //public GameObject camera2;
    //public GameObject[] cena = new GameObject[6];
    bool condicaoCoordenadas = true;
    bool condicaoCamera1 = true;
    bool condicaoCamera2 = true;
    bool f3 = true;
    bool f4 = true;
    bool f5 = true;
    bool f6 = true;
    /*[SerializeField]
    Camera cameraPrincipal;
    [SerializeField]
    Camera c1;
    [SerializeField]
    GameObject telaEsqueleto;*/
    


    void Update()
    {
        //todos as coisas que se desativam com f1
        coordenadas.SetActive(condicaoCoordenadas);
        if (Input.GetKeyDown(KeyCode.F1))
        {
            condicaoCoordenadas = !condicaoCoordenadas;
        }

        //todos as coisas que se desativam com f2
        /*camera1.SetActive(condicaoCamera1);
        camera2.SetActive(condicaoCamera2);
        if (Input.GetKeyDown(KeyCode.F2))
        {
            condicaoCamera1 = !condicaoCamera1;
            condicaoCamera2 = !condicaoCamera2;
        }

        //todos as coisas que se desativam com f3
        cena[0].SetActive(f3);
        cena[2].SetActive(f3);
        cena[4].SetActive(f3);
        cena[5].SetActive(f3);
        if (Input.GetKeyDown(KeyCode.F3))
        {
            f3 = !f3;
        }

        //todos as coisas que se desativam com f4
        cena[1].SetActive(f4);
        cena[3].SetActive(f4);
        if (Input.GetKeyDown(KeyCode.F4))
        {
            f4 = !f4;
        }

        //muda a camera para o esqueleto
        cameraPrincipal.gameObject.SetActive(f5);
        c1.gameObject.SetActive(!f5);
        if (Input.GetKeyDown(KeyCode.F5))
        {
            f5 = !f5;
        }

        //ativa tela da camera do esqueleto
        telaEsqueleto.SetActive(!f6);
        if(Input.GetKeyDown(KeyCode.F6))
        {
            f6 = !f6;
        }*/
            
    }
}
