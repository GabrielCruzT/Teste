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
        if (MenuOpcoes.projetor == true)
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
        if (MenuPrincipal.jogando)        //faz isso apenas na cena principal
        {
            if (MenuOpcoes.projetor == true)      //muda objetos da cena (telas) de acordo com a resolução                          
            {
                telas.transform.position = new Vector3(1.8f, -0.5f, 3.57f);
            }
            else
            {
                telas.transform.position = new Vector3(1.65f, -0.5f, 2.41f);
            }
        }
    }
}
