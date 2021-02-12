using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class NomeBotao : MonoBehaviour {

    public Text nomeBotaoEnviar;
	public static string nomeEnviar;

    GameObject[] lixo;

	public void ApagarBotao(string tipoBotao)
	{
		nomeEnviar = nomeBotaoEnviar.text;                           //envia nome do jogo que sera apagado
		lixo = GameObject.FindGameObjectsWithTag("Button(Clone)");
		for (int i = 0; i <= lixo.Length - 1; i++)
			Destroy(lixo[i]);

        if (tipoBotao == "SelecionarJogador")
        {
            MenuJogar.nomeJogador = nomeEnviar;
            string[] usuarios = File.ReadAllLines(Diretorios_Salvar.arquivoUsuarios);
            foreach(string nome in usuarios)
            {
                if (MenuJogar.nomeJogador == nome.Split(',')[0] + "," + nome.Split(',')[1] + "," + nome.Split(',')[2])
                {
                    try
                    {
                        MenuJogar.dBracoEsq = nome.Split(',')[3];
                        MenuJogar.dBracoDir = nome.Split(',')[4];
                    }
                    catch
                    {
                        MenuJogar.dBracoEsq = "";
                        MenuJogar.dBracoDir = "";
                    }
                }
                   
            }
        }   
        else if (tipoBotao == "ExcluirJogador")
        {
            string[] usuarios = File.ReadAllLines(Diretorios_Salvar.arquivoUsuarios);
            foreach (string nome in usuarios)
            {
                if (nomeEnviar == nome.Split(',')[0] + "," + nome.Split(',')[1] + "," + nome.Split(',')[2])
                {
                    nomeEnviar = nome;
                }
            }
            MenuJogar.nomeJogador = "";
        }
            
        else if (tipoBotao == "SelecionarJogo")
            Objeto.opcoesDeJogo[6] = nomeEnviar;
				
	}
}