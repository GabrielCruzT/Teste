using UnityEngine;
using System.Collections;

public class DesativarTextosUI : MonoBehaviour {

    public GameObject coordenadas;
    public GameObject imagemDaCamera;
    public GameObject mascaraDeImagemDaCamera;
    public GameObject[] cena = new GameObject[6];
    bool condicaoCoordenadas = true;
    bool condicaoimagemDaCamera = true;
    bool condicaomascaraDeImagemDaCamera = true;
    bool f3 = true;
    bool f4 = true;


    void Update()
    {
        //Desativar as coordenadas com f1
        coordenadas.SetActive(condicaoCoordenadas);
        if (Input.GetKeyDown(KeyCode.F1))
        {
            condicaoCoordenadas = !condicaoCoordenadas;
        }

        //Desativar as cameras com f2
        imagemDaCamera.SetActive(condicaoimagemDaCamera);
        mascaraDeImagemDaCamera.SetActive(condicaomascaraDeImagemDaCamera);
        if (Input.GetKeyDown(KeyCode.F2))
        {
            condicaoimagemDaCamera = !condicaoimagemDaCamera;
            condicaomascaraDeImagemDaCamera = !condicaomascaraDeImagemDaCamera;
        }

        //Desativar o modo de jogo e outras barras superiores com f3
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
    }
}
