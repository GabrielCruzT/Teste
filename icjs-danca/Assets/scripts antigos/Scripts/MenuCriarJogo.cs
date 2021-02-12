using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuCriarJogo : MonoBehaviour {

	ScrollMenu scrollMenu;
	public static int numeroDeBolas = 8;

	[SerializeField]
	GameObject infoPainel;
	[SerializeField]
	Button criarJogoButton;
	[SerializeField]
	GameObject prefabBotaoExcluirJogo;
	[SerializeField]
	Text prefabBotaoExcluirJogoText;
	[SerializeField]
	InputField campoNumeroDeBolas;
	[SerializeField]
	GameObject scrollMenuExcluirJogo;
	[SerializeField]
	Button excluirJogoButton;

	void Start () {
	
	}

	void Update () {
		
		if (campoNumeroDeBolas.text != "" && int.TryParse(campoNumeroDeBolas.text, out numeroDeBolas) && scrollMenuExcluirJogo.activeInHierarchy == false)
		{
			criarJogoButton.interactable = true;
			infoPainel.SetActive (false);
		}
		else
		{
			criarJogoButton.interactable = false;
			infoPainel.SetActive (true);
		}

		if (Directory.GetFiles(Diretorios_Salvar.PastadeModosdeJogo).Length>1 && scrollMenuExcluirJogo.activeInHierarchy == false) 
			excluirJogoButton.interactable = true;
		else
			excluirJogoButton.interactable = false;
	}
	public void CriarJogo()
	{
		PosicaoDaBola.numeroBola = 0;
		SceneManager.LoadScene ("Principal");
	}

	public void ExcluirJogo()
	{
		scrollMenu = gameObject.AddComponent<ScrollMenu> ();
		scrollMenu.ScrollSelecao (File.ReadAllLines (Diretorios_Salvar.arquivoMododeJogo), prefabBotaoExcluirJogo, prefabBotaoExcluirJogoText);
	}

	public void ExcluirModoDeJogo()
	{
		Diretorios_Salvar.ExcluirDados (File.ReadAllLines (Diretorios_Salvar.arquivoMododeJogo), Diretorios_Salvar.arquivoMododeJogo, NomeBotao.nomeEnviar);
		File.Delete(Diretorios_Salvar.PastadeModosdeJogo + "\\" + NomeBotao.nomeEnviar + ".txt");
        MenuOpcoes.ReescreveOpcoes("");
    }
}
