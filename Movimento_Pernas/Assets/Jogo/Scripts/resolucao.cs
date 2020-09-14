using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Resolucao : MonoBehaviour {

    public GameObject telas;

    void Start()
    {
        MudaResolucao();
        PosicaoTelas();
    }
    public static void MudaResolucao()
    {
        if (Menu.projetor == true)
        {
            Screen.SetResolution(1024, 768, true);
        }
        else
        {
            Screen.SetResolution(1920, 1080, true);
        }
    }
    void PosicaoTelas()
    {
        if(Menu.jogando)        //faz isso apenas na cena principal
        {
            if (Menu.projetor == true)      //muda objetos da cena (telas) de acordo com a resolução                          
            {
                telas.transform.position = new Vector3(2.27f, -0.28f, 3.57f);
            }
            else
            {
                telas.transform.position = new Vector3(2.12f, -0.28f, 2.41f);
            }
        }
    }
}
