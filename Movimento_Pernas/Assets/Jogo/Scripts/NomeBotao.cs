using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NomeBotao : MonoBehaviour {

    public Text nomeBotaoEnviar;
    public GameObject posicaoMenu;

    GameObject[] lixo;

    //Envia o nome para os botões que são gerados no scroll
    public void EnviarNome()
    {
        Menu.nomeJogador = nomeBotaoEnviar.text;
        lixo = GameObject.FindGameObjectsWithTag("Button(Clone)");          //seleciona todos os botões 
        for (int i = 0; i <= lixo.Length - 1; i++)
            Destroy(lixo[i]);
    }

    //Deleta os botões do scroll para que n se repitam quando desejar selecionar novamente usuario
    public void ApagarBotao()
    {
        Menu.jogoExcluido = nomeBotaoEnviar.text;                           //envia nome do jogo que sera apagado
        lixo = GameObject.FindGameObjectsWithTag("Button(Clone)");
        for (int i = 0; i <= lixo.Length - 1; i++)
            Destroy(lixo[i]);
    }

    //Função identica a anterior, porém utilizada na cena principal e tb desliga o scroll
    public void ApagarBotaoPosicao()
    {
        Objeto.nomeJogo = nomeBotaoEnviar.text;                             //envia o nome do jogo escolhido
        lixo = GameObject.FindGameObjectsWithTag("Button(Clone)");
        for (int i = 0; i <= lixo.Length - 1; i++)
            Destroy(lixo[i]);
        posicaoMenu.SetActive(false);                                       //desativa o menu scroll da cena principal
    }
}