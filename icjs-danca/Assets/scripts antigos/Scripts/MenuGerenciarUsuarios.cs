using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuGerenciarUsuarios : MonoBehaviour {

	string jogador;
	string nomeSalvar;
	string idadeSalvar;
	string generosalvar;
	Diretorios_Salvar diretorioSalvar = new Diretorios_Salvar();
	ScrollMenu scrollMenu;

	[SerializeField]
	Toggle masculino;
	[SerializeField]
	Toggle feminino;
	[SerializeField]
	InputField campoNome;
	[SerializeField]
	InputField campoIdade;
	[SerializeField]
	Button criarUsuarioButton;
	[SerializeField]
	GameObject scrollMenuExcluir;
	[SerializeField]
	GameObject prefabBotaoExcluirUsuario;
	[SerializeField]
	Text prefabBotaoExcluirUsuarioTexto;
	[SerializeField]
	Button deletarUsuarioButton;

	public void Update()
	{
		if (campoNome.text != "" && campoIdade.text != "" && (masculino.isOn || feminino.isOn) && scrollMenuExcluir.activeInHierarchy==false)  
			criarUsuarioButton.interactable = true;
		else
			criarUsuarioButton.interactable = false;

		/*if (File.Exists(Diretorios_Salvar.arquivoUsuarios) &&  scrollMenuExcluir.activeInHierarchy == false)
		{
			deletarUsuarioButton.interactable = true;
		}   
		else
		{
			deletarUsuarioButton.interactable = false;
		}*/
	}

	public void Criarjogador()
	{
		if (masculino.isOn)
			generosalvar = "M";
		else if(feminino.isOn)
			generosalvar = "F";

		nomeSalvar = campoNome.text;
		idadeSalvar = campoIdade.text; 

		jogador = nomeSalvar + "," + idadeSalvar + "," + generosalvar;

		campoNome.text = "";
		campoIdade.text = "";

		masculino.isOn = false;
		feminino.isOn = false;

		Diretorios_Salvar.AdicionarDados (Diretorios_Salvar.arquivoUsuarios, jogador);
	}

	public void ExcluirJogador()
	{
		scrollMenu = gameObject.AddComponent<ScrollMenu> ();
		scrollMenu.ScrollSelecao (File.ReadAllLines (Diretorios_Salvar.arquivoUsuarios), prefabBotaoExcluirUsuario, prefabBotaoExcluirUsuarioTexto);
	}

	public void ExcluirNomeJogador()
	{
		Diretorios_Salvar.ExcluirDados (File.ReadAllLines (Diretorios_Salvar.arquivoUsuarios), Diretorios_Salvar.arquivoUsuarios, NomeBotao.nomeEnviar);
	}

	public void BotaoVoltar()       //apaga conteudos dos inputfields 
	{
		campoNome.text = "";
		campoIdade.text = "";
	}
}
