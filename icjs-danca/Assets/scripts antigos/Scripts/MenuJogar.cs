using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuJogar : MonoBehaviour {

	ScrollMenu scrollmMenu;
	public static string nomeJogador;
	public static bool apagarNomesMenu;
    public static string dBracoEsq;
    public static string dBracoDir;

    [SerializeField]
	Text nomeTexto;
	[SerializeField]
	Text idadeTexto;
	[SerializeField]
	Text generoTexto;
	[SerializeField]
	GameObject prefabBotaoSelecionarUsuario;
	[SerializeField]
	Button iniciarButton;
	[SerializeField]
	GameObject scrollMenuSelecionar; 
	[SerializeField]
	Button selecionarUsuarioButton;
	[SerializeField]
	Text prefabBotaoText;
    [SerializeField]
    Button calibrar;
    [SerializeField]
    GameObject painelAvisoCalibrado;

	void Update () {

		if (nomeJogador != "")
        {
            if(painelAvisoCalibrado.activeInHierarchy == false)
            {
                iniciarButton.interactable = true;
                calibrar.interactable = true;
            }
        }                                          
		else
        {
            Debug.Log("Caiu");
            calibrar.interactable = false;
            dBracoDir = "";
            dBracoEsq = "";
            iniciarButton.interactable = false;
        }
			

		if (nomeJogador != "") {
			string[] DadosJogador = nomeJogador.Split (',');
			nomeTexto.text = DadosJogador [0];
			idadeTexto.text = DadosJogador [1];
			generoTexto.text = DadosJogador [2];
		}
		else 
		{
			nomeTexto.text = "";
			idadeTexto.text = "";
			generoTexto.text = "";
		}

		if (File.Exists(Diretorios_Salvar.arquivoUsuarios) && scrollMenuSelecionar.activeInHierarchy == false)
		{
			if(File.ReadAllLines(Diretorios_Salvar.arquivoUsuarios).Length>0)
				selecionarUsuarioButton.interactable = true;
			else
				selecionarUsuarioButton.interactable = false;
		}

		if (Directory.GetFiles(Diretorios_Salvar.PastadeModosdeJogo).Length <= 1)
			iniciarButton.interactable = false;

		if (apagarNomesMenu == true)
		{
			nomeTexto.text = "";
			idadeTexto.text = "";
			generoTexto.text = "";
			apagarNomesMenu = false;
		}

        if (dBracoEsq == "" || dBracoDir == "")
            iniciarButton.interactable = false;
        else
            iniciarButton.interactable = true;

	}

	public void SelecionarUsuario()
	{
		scrollmMenu = gameObject.AddComponent<ScrollMenu> ();

		scrollmMenu.ScrollSelecao (File.ReadAllLines(Diretorios_Salvar.arquivoUsuarios), prefabBotaoSelecionarUsuario, prefabBotaoText);
	}

	public void IniciarJogo()
	{
        MenuPrincipal.jogando = true;
		SceneManager.LoadScene ("Principal");
	}

    public void CalibrarJogo()
    {
        calibrar.interactable = false;
        Debug.Log(dBracoDir);
        Debug.Log(nomeJogador);
        if (dBracoEsq == "" || dBracoDir == "")
            CenaCalibracao();
        else
            painelAvisoCalibrado.SetActive(true);
            
    }

    public void CenaCalibracao()
    {
        calibrar.interactable = true;
        painelAvisoCalibrado.SetActive(false);
        SceneManager.LoadScene("Calibração");
    }
}