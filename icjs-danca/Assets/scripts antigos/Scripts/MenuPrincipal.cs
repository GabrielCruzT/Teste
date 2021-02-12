using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuPrincipal : MonoBehaviour {

	private MenuOpcoes menuOpcoes;
	private Diretorios_Salvar diretorioSalvar;
	private MenuGerenciarUsuarios menuGerenciarUsuarios;
	public static bool jogando;

	public void Start()
	{
		jogando = false;
		MenuJogar.nomeJogador = "";
		Diretorios_Salvar.CriarDiretorios ();
		diretorioSalvar = new Diretorios_Salvar();
		if (!File.Exists (Diretorios_Salvar.arquivoOpcoes))
			Diretorios_Salvar.SalvarDadosTeste (MenuOpcoes.OpcoesIniciais	(), Diretorios_Salvar.arquivoOpcoes);
        else
        {
            MenuOpcoes.opcoesSelecionadas = File.ReadAllLines(Diretorios_Salvar.arquivoOpcoes);
            MenuOpcoes.projetor = bool.Parse(MenuOpcoes.opcoesSelecionadas[7]);
        }
            
    }

	public void AbrirPasta()
	{
		System.Diagnostics.Process.Start (Application.persistentDataPath);
	}
		
	public void SairdoJogo()
	{
		Application.Quit ();
	}
}
