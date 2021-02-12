using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class Objeto : MonoBehaviour {

	public static int pontos=0;
	public static int apareceuBola=0;
    public static int pontosMaximo;
    public static int numberoBolasCriadas = 0;
    public static int tempoParaInicio = 3;
    public static float tempodeJogo;
	public static float tempoMaximo;
	public static float TEO;
    public static float TOA;
    public static bool jogoRodando = false;
    public static bool bolaAtiva = false;
    public static bool escrevertxt = false;
    public static string nomeJogo;
    public static string nomeModoJogo;
    public static string[] opcoesDeJogo;
    public static Vector3 bolaPosicaoAtual;

    private int indiceArrayPosicoes = 0;
    private bool trocaCompleta = false;
    private bool naoSalvar = false;
    private float[] posicaoBolaX;
    private float[] posicaoBolaY;
    private string ArquivoJogo;
    private string NomeArquivoJogo;
    private string[] posicaoLida;
    private string[] lerTextoPosicoes;
    private Vector3[] posicaoBola;
    private Animator anim;
    private EfeitoSonoroBola controle_do_som;

    [SerializeField]
    private Image imagemInicioTempo;
    [SerializeField]
    private InputField nomeNovoJogo;
    [SerializeField]
    private GameObject prefabBola;
    [SerializeField]
    private GameObject interfaceGUI;
    [SerializeField]
    private GameObject tela;
    [SerializeField]
    private GameObject criarJogoCena;
    [SerializeField]
    private GameObject[] bola;
    [SerializeField]
    private Text posicaoBotaoText;
    [SerializeField]
    private GameObject posicaoBotao;
    [SerializeField]
    private Text prefabNomeBola;
    [SerializeField]
    private GameObject scrollMenuSelecionarJogo;
    [SerializeField]
    private GameObject infoPainelCriarJogo;
	[SerializeField]
	private Button botaoGerarBola;

    [SerializeField]
    GameObject bola2;

    void Start ()
    {
		opcoesDeJogo = File.ReadAllLines (Diretorios_Salvar.arquivoOpcoes);

        //Verifica se vai jogar ou definir as posições da bola
		if (MenuPrincipal.jogando == false)
        {
            interfaceGUI.SetActive(false);
            tela.SetActive(false);
            criarJogoCena.SetActive(true);
            infoPainelCriarJogo.SetActive(false);
        }
        else
        {
            interfaceGUI.SetActive(true);
            tela.SetActive(true);
            criarJogoCena.SetActive(false);

            //define controlador das animações
            anim = GetComponent<Animator>();//animação

            //ativa bola fora da camera
            gameObject.transform.position = new Vector3(10f, 10f, -40f);
            GetComponent<Renderer>().enabled = false;

            //Quantidade de tempo e pontos de acordo com o modo de jogo
			pontosMaximo = int.Parse (opcoesDeJogo [1]);
			tempoMaximo = float.Parse (opcoesDeJogo [3]);
            TOA = float.Parse(opcoesDeJogo[4]);
            TEO = float.Parse (opcoesDeJogo [5]);

            //Verifica se existe algum jogo criado
            if (opcoesDeJogo[6] != "")
                RetornaJogoSelecionado();


        }

        //efeito sonoro
        controle_do_som = FindObjectOfType(typeof(EfeitoSonoroBola)) as EfeitoSonoroBola;
    }

	void Update ()
    {
		if (MenuPrincipal.jogando == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (bolaAtiva == true)
                    anim.SetInteger("Condition", 1);
				if (opcoesDeJogo[6] != "")       //verifica se algum jogo foi selecionado
					if ((jogoRodando == false || tempoParaInicio == 0) && scrollMenuSelecionarJogo.activeInHierarchy==false)
                        StartCoroutine(JogarPausar());
            }
                

            //MODO DE JOGO POR PONTOS
			if (int.Parse(opcoesDeJogo[0]) == 1)
            {

				if (pontosMaximo < apareceuBola)
                {
                    //reseta o valor de t
					tempoParaInicio = 3;

                    StartCoroutine(JogarPausar());
                    StopCoroutine(Aparece());
                    
                }
            }

            //MODO DE JOGO POR TEMPO
			if (int.Parse(opcoesDeJogo[2]) == 1)
			{
				if (tempoMaximo < 0)
                {
                    //reseta valores de t e tempo
					tempoParaInicio = 3;
					tempoMaximo = float.Parse(opcoesDeJogo[3]);

                    StartCoroutine(JogarPausar());
                    StopCoroutine(Aparece());
                   
                }
            }

            if (trocaCompleta)      //se objeto trocou de posicao
            {
                trocaCompleta = false;

				if (int.Parse(opcoesDeJogo[0])== 1 && apareceuBola >= pontosMaximo)
                {
					tempoParaInicio = 3;        //para caso o jogo seja pausado
                    StartCoroutine(JogarPausar());
                }

                //não inicia a corrotina aparece caso jogo esteja pausado
				if (tempoParaInicio != 3)
                    StartCoroutine(Aparece());
            }

            //QUANDO JOGO INICIA
			if (tempoParaInicio == 0)
            {
                //inicia gravação de dados no txt
				escrevertxt = true;

				tempodeJogo += Time.deltaTime;
				tempoMaximo -= Time.deltaTime;
            }

            //TEMPO ENTRE APARIÇÕES
			if (bolaAtiva == true && tempoParaInicio == 0)
            {
                //contagem de tempo entre objetos
				TEO -= Time.deltaTime;

				if (TEO <= 0)
                {
                    StartCoroutine(TrocaCor());
                    bolaAtiva = false;
					TEO = float.Parse(opcoesDeJogo[5]);
                }
            }

            if (bolaAtiva)
                bolaPosicaoAtual = bola2.transform.position;
            else
                bolaPosicaoAtual = new Vector3(0f, 0f, 0f);

        }
    }

    //CORROTINA DE TEMPO ENTRE OBJETOS E DESTRUIÇÃO DA BOLA
    IEnumerator TrocaCor()
    {
        anim.SetInteger("Condition", 1);

        GetComponent<Collider>().enabled = false;
        
        //espera tempo entre objetos
        yield return new WaitForSeconds(TOA);

        gameObject.transform.position = new Vector3(10f, 10f, -40f);
        bola2.transform.position = new Vector3(10f, 10f, -50f);

        trocaCompleta = true;
    }

    //CORROTINA DE MUDANÇA DE POSIÇÃO DA BOLA
    IEnumerator Aparece()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Transform>().position = posicaoBola[indiceArrayPosicoes];      //pega posicao da bola no arquivo de jogo

        if(gameObject.transform.position.x <= 0f)
            bola2.transform.position = CoordenadasDaJuntas.posicaoPescoso + new Vector3(posicaoBola[indiceArrayPosicoes].x * float.Parse(MenuJogar.dBracoEsq), posicaoBola[indiceArrayPosicoes].y * float.Parse(MenuJogar.dBracoEsq), 0);
        else
            bola2.transform.position = CoordenadasDaJuntas.posicaoPescoso + new Vector3(posicaoBola[indiceArrayPosicoes].x * float.Parse(MenuJogar.dBracoDir), posicaoBola[indiceArrayPosicoes].y * float.Parse(MenuJogar.dBracoDir), 0);

        indiceArrayPosicoes++;

        anim.SetInteger("Condition", 2);

        bolaAtiva = true;

		apareceuBola++;

		if (indiceArrayPosicoes == numeroDeBolasEmJogo)
            indiceArrayPosicoes = 0;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        bolaAtiva = false;

        //reseta valor do tempo entre objetos
        TEO = float.Parse(opcoesDeJogo[5]);

        StartCoroutine(TrocaCor());
		pontos++;
        controle_do_som.playSFX(controle_do_som.sfxBola, 0.5f);
    }

    //INICIA OU PAUSA O JOGO
    IEnumerator JogarPausar()
    {
		if (!jogoRodando)       //jogo rodando
        {
            jogoRodando = true;
            
            //ESPERA 3 SEGUNDO PARA INICIO
            for (tempoParaInicio = 3; tempoParaInicio > 0; tempoParaInicio--)       //espera 3 segundos
                yield return new WaitForSeconds(1f);

            StartCoroutine(Aparece());
            imagemInicioTempo.enabled = false;      //image background texto tempo

            Diretorios_Salvar.CriarArquivoDadosUsuario();
        }
        else        //jogo pausado
        {
			//dados para a cena de estatisticas
			MenuDeEstatisticas.pontosEstatisticas = Objeto.pontos;
			MenuDeEstatisticas.tempoEstatisticas = Objeto.tempodeJogo;
			MenuDeEstatisticas.numeroAparicoesEstatisticas = Objeto.apareceuBola;

			//RESTAURA TODAS AS VARIAVEIS PARA VALORES INICIAIS
			jogoRodando = false;
			pontosMaximo = int.Parse (opcoesDeJogo [1]);
			tempoMaximo = float.Parse (opcoesDeJogo [3]);
			TEO = float.Parse (opcoesDeJogo [5]);			
			apareceuBola = 0;
			pontos = 0;
			tempodeJogo = 0;
			tempoParaInicio = 3;
			escrevertxt = false;
			indiceArrayPosicoes = 0;

			SceneManager.LoadScene("Estatísticas");
        }
    }

    int numeroDeBolasEmJogo;

    public void RetornaJogoSelecionado()
    {
        MenuOpcoes.ReescreveOpcoes(opcoesDeJogo[6]);

		posicaoLida = File.ReadAllLines(Diretorios_Salvar.arquivoMododeJogo);        //le arquivo modo de jogo para saber quantas posicoes serao diferentes
        for (int i = 0; i < posicaoLida.Length; i++)
        {
            if (posicaoLida[i] == opcoesDeJogo[6])
            {
                numeroDeBolasEmJogo = int.Parse(posicaoLida[i].Split(',')[1]);
				posicaoBola = new Vector3[numeroDeBolasEmJogo];
            }
        }

        AplicarPosicao();
    }

    public void AplicarPosicao()
    {
        int j = 0;
		string[] gamesave = File.ReadAllLines(Diretorios_Salvar.PastadeModosdeJogo + "\\" + opcoesDeJogo[6] + ".txt");        //le e aplica posicoes
		for (int i = 0; i < numeroDeBolasEmJogo * 3; i = i + 3)
        {
            posicaoBola[j] = new Vector3(float.Parse(gamesave[0 + i]), float.Parse(gamesave[1 + i]), float.Parse(gamesave[2 + i]));     //cria array com posicoes para o jogo
            j++;
        }
    }
}