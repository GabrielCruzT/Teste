using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class InterfaceJogo : MonoBehaviour {

    public GameObject menino;
	public GameObject menina;

	[SerializeField]
	private GameObject interfaceGUI;
	[SerializeField]
	private GameObject tela;
	[SerializeField]
	private GameObject criarJogoCena;
	[SerializeField]
	private Text tempoInicio;
	[SerializeField]
	private Text nomeJogador;
	[SerializeField]
	private GameObject infoPanel;
	[SerializeField]
	private Text jogoAtual;

	public Image caixaTempoParaInicio;
	public Button selecionarJogo;

	public Text pontuacao;
	public Text tempoLimite;
	public Text pontosLimite;
	public Text porPontos;
	public Text porTempo;
	public Text quantidadeAparecoes;

	ScrollMenu scrollMenu;

	[SerializeField]
	private GameObject prefabPosicaoBotao;
	[SerializeField]
	private Text prefabPosicaoBotaoText;


	// Use this for initialization
	void Start () {
		//Verifica se vai jogar ou definir as posições da bola

		if (MenuPrincipal.jogando == false)
		{
			interfaceGUI.SetActive(false);
			tela.SetActive(false);
			criarJogoCena.SetActive(true);
			menina.SetActive(false);
		}
		else
		{
			interfaceGUI.SetActive(true);
			tela.SetActive(true);
			criarJogoCena.SetActive(false);
			tempoInicio.enabled = true;
			nomeJogador.text = MenuJogar.nomeJogador.Split(',')[0];

			if (MenuJogar.nomeJogador.Split(',')[2]=="F")
			{
				menina.SetActive(true);
				menino.SetActive (false);
			}
			else
			{
				menina.SetActive(false);
				menino.SetActive(true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (MenuPrincipal.jogando == true)
        {
            if (Objeto.opcoesDeJogo[6] != "")
            {
                infoPanel.SetActive(false);
                jogoAtual.text = Objeto.opcoesDeJogo[6];
            }
            else
            {
                infoPanel.SetActive(true);
                jogoAtual.text = "nenhum";
            }

            if (int.Parse(Objeto.opcoesDeJogo[0]) == 1)
            {
                porPontos.text = "Modo de Jogo: Por Pontos";
                pontosLimite.text = "Max. de Bolas: " + Objeto.pontosMaximo.ToString();
            }
            else if (int.Parse(Objeto.opcoesDeJogo[2]) == 1)
            {
                porTempo.text = "Modo de Jogo: Por tempo";
                tempoLimite.text = "Tempo de Partida: " + Objeto.tempoMaximo.ToString("f2");
            }

            if (Objeto.tempoParaInicio == 3 && Objeto.jogoRodando == false)
            {
                selecionarJogo.interactable = true;
                caixaTempoParaInicio.enabled = true;
                tempoInicio.enabled = true;
                tempoInicio.text = "Espaço para iniciar";
            }
            else if (Objeto.tempoParaInicio == 0)
            {
                caixaTempoParaInicio.enabled = false;
                tempoInicio.enabled = false;
            }
            else
            {
                selecionarJogo.interactable = false;
                tempoInicio.text = Objeto.tempoParaInicio.ToString();
            }

            if (Input.GetKeyDown(KeyCode.Tab) && Objeto.jogoRodando == false)
            {
                SceneManager.LoadScene("Estatísticas");
                MenuPrincipal.jogando = false;
            }
                

            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Menu");

            pontuacao.text = "Pontos: " + Objeto.pontos.ToString();
            quantidadeAparecoes.text = "Número de Aparições: " + Objeto.apareceuBola.ToString();
        }
	}

	public void ScrollMenuPosicao()       //cria botoes para seecionar jogo
	{
		scrollMenu = gameObject.AddComponent<ScrollMenu> ();
		scrollMenu.ScrollSelecao (File.ReadAllLines (Diretorios_Salvar.arquivoMododeJogo), prefabPosicaoBotao, prefabPosicaoBotaoText);
	}
}
