using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class Objeto : MonoBehaviour {

    public static float tempodeJogo;
    public static float tempoEscolido;
    public static bool escrevertxt = false;
    public static bool bolaAtiva = false;
    public static bool jogoRodando = false;
    public static string textoPosicao;
    public static string nomeJogo;
    public static int[] testeAlcance = new int[Menu.numerodeBolas];
    public static int pontos = 0;
    public static int pontoEscolido;
    public static int apareceuBola = 0;
    public static Vector3 bolaPosicaoAtual;

    int indiceArrayPosicoes = 0;
    int tempoParaInicio = 3;
    int numberoBolasCriadas = 0;
    bool trocaCompleta = false;
    bool naoSalvar = false;
    float[] posicaoBolaX;
    float[] posicaoBolaY;
    float TEO;
    string ArquivoJogo;
    string NomeArquivoJogo;
    string[] posicaoLida;
    string[] lerTextoPosicoes;
    Vector3[] posicaoBola;
    
    private Som controle_do_som;

    public Text tempoInicio;
    public Text pontuacao;
    public Text tempoLimite;
    public Text pontosLimite;
    public Text porPontos;
    public Text porTempo;
    public Text nomeJogador;
    public Text quantidadeAparecoes;
    public Text[] nomebola;
    public Image imagemInicioTempo;
    public Button criarJogo;
    public Button selecionarJogo;
    public InputField nomeNovoJogo;
    public GameObject prefabBola;
    public GameObject menino;
    public GameObject menina;
    public GameObject interfaceGUI;
    public GameObject tela;
    public GameObject criarJogoCena;
    public GameObject[] bola;

    Animator anim;

    [SerializeField]
    private Text posicaoBotaoText;
    [SerializeField]
    private GameObject posicaoBotao;
    [SerializeField]
    public Text prefabNomeBola;
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private Text jogoAtual;
    [SerializeField]
    private GameObject scrollMenuSelecionarJogo;
    [SerializeField]
    private GameObject infoPainelCriarJogo;

    void Start ()
    {
        //Verifica se vai jogar ou definir as posições da bola
        if (Menu.jogando == false)
        {
            bola = new GameObject[Menu.numerodeBolas];
            posicaoBolaX = new float[Menu.numerodeBolas];
            posicaoBolaY = new float[Menu.numerodeBolas];
            posicaoBola = new Vector3[Menu.numerodeBolas];
            nomebola = new Text[Menu.numerodeBolas];
            interfaceGUI.SetActive(false);
            tela.SetActive(false);
            criarJogoCena.SetActive(true);
            menina.SetActive(false);
            infoPainelCriarJogo.SetActive(false);
        }
        else
        {
            interfaceGUI.SetActive(true);
            tela.SetActive(true);
            criarJogoCena.SetActive(false);
            tempoInicio.enabled = true;

            //alterna entre os personagens
            if (Menu.personagemMaculino == false)
            {
                menina.SetActive(true);
                menino.SetActive(false);
            }
            else
            {
                menina.SetActive(false);
                menino.SetActive(true);
            }

            //define controlador das animações
            anim = GetComponent<Animator>();//animação

            //ativa bola fora da camera
            gameObject.transform.position = new Vector3(10f, 10f, -40f);
            GetComponent<Renderer>().enabled = false;

            nomeJogador.text = Menu.nomeJogador.Split(',')[0];

            //apareceuBola = 0;
            //tempoParaInicio = 3;

            //Quantidade de tempo e pontos de acordo com o modo de jogo
            tempoEscolido = Menu.maxTempo;
            pontoEscolido = Menu.maxPontos;
            TEO = Menu.valorScrollbarTEO;

            //Verifica se existe algum jogo criado
            if (Menu.opcoes[6] != "")
                RetornaJogoSelecionado();

        }

        //efeito sonoro
        controle_do_som = FindObjectOfType(typeof(Som)) as Som;
    }

	void Update ()
    {
        if (Menu.jogando == false)
        {
            if(GameObject.Find("0") != null)        //caso n tenha nehuma bola gerada
            {
                nomebola[PosicaoDaBola.numeroBola].text = "Bola " + ((PosicaoDaBola.numeroBola) + 1).ToString() + ":" + textoPosicao;
                if (PosicaoDaBola.textoVermelho == true)
                    nomebola[PosicaoDaBola.numeroBola].color = Color.red;
                else
                    nomebola[PosicaoDaBola.numeroBola].color = new Color32(34, 218, 141, 255);
            }
                
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Menu");
                
            if (Menu.numerodeBolas > numberoBolasCriadas)
                criarJogo.interactable = false;
            else
                criarJogo.interactable = true;

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (bolaAtiva == true)
                    anim.SetInteger("Condition", 1);
                if (Menu.opcoes[6] != "")       //verifica se algum jogo foi selecionado
                    if ((jogoRodando == false || tempoParaInicio == 0) && scrollMenuSelecionarJogo.activeInHierarchy==false)
                        StartCoroutine(JogarPausar());
            }

            if (Menu.opcoes[6] != "")
            {
                infoPanel.SetActive(false);
                jogoAtual.text = Menu.opcoes[6];
            }  
            else
            {
                infoPanel.SetActive(true);
                jogoAtual.text = "nenhum";
            }
                

            //MODO DE JOGO POR PONTOS
            if (int.Parse(Menu.opcoes[0]) == 1)
            {
                porPontos.text = "Modo de Jogo: Por Pontos";

                if (Menu.maxPontos < apareceuBola)
                {
                    //reseta os valores t e pontos
                    tempoParaInicio = 3;
                    Menu.maxPontos = pontoEscolido;

                    StartCoroutine(JogarPausar());
                    StopCoroutine(Aparece());
                    
                }

                pontosLimite.text = "Max. de Bolas: " + Menu.maxPontos.ToString();
            }

            //MODO DE JOGO POR TEMPO
            if (int.Parse(Menu.opcoes[2]) == 1)
            {
                porTempo.text = "Modo de Jogo: Por tempo";

                if (Menu.maxTempo < 0)
                {
                    //reseta valores de t e tempo
                    tempoParaInicio = 3;
                    Menu.maxTempo = tempoEscolido;

                    StartCoroutine(JogarPausar());
                    StopCoroutine(Aparece());
                   
                }

                tempoLimite.text = "Tempo de Partida: " + Menu.maxTempo.ToString("f2");
            }

            if (trocaCompleta)      //se objeto trocou de posicao
            {
                trocaCompleta = false;

                if (Menu.pontosBool == true && apareceuBola >= Menu.maxPontos)
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
                Menu.maxTempo -= Time.deltaTime;
            }

            //TEMPO ENTRE APARIÇÕES
            if (bolaAtiva == true && tempoParaInicio == 0)
            {
                //contagem de tempo entre objetos
                Menu.valorScrollbarTEO -= Time.deltaTime;

                if (Menu.valorScrollbarTEO <= 0)
                {
                    StartCoroutine(TrocaCor());
                    bolaAtiva = false;
                    Menu.valorScrollbarTEO = TEO;
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab) && jogoRodando == false)
                SceneManager.LoadScene("Estatísticas");

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Menu.jogando = false;
                SceneManager.LoadScene("Menu");
            }

            if (bolaAtiva)
                bolaPosicaoAtual = GetComponent<Transform>().position;
            else
                bolaPosicaoAtual = new Vector3(0f, 0f, 0f);

            pontuacao.text = "Pontos: " + pontos.ToString();
            quantidadeAparecoes.text = "Número de Aparições: " + apareceuBola.ToString();

        }
    }

    //CORROTINA DE TEMPO ENTRE OBJETOS E DESTRUIÇÃO DA BOLA
    IEnumerator TrocaCor()
    {
        anim.SetInteger("Condition", 1);

        GetComponent<Collider>().enabled = false;
        
        //espera tempo entre objetos
        yield return new WaitForSeconds(Menu.valorScrollbarTOA);

        gameObject.transform.position = new Vector3(10f, 10f, -40f);

        trocaCompleta = true;
    }

    //CORROTINA DE MUDANÇA DE POSIÇÃO DA BOLA
    IEnumerator Aparece()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Transform>().position = posicaoBola[indiceArrayPosicoes];      //pega posicao da bola no arquivo de jogo

        indiceArrayPosicoes++;

        anim.SetInteger("Condition", 2);

        bolaAtiva = true;

        apareceuBola++;

        if (indiceArrayPosicoes == Menu.numerodeBolas)
            indiceArrayPosicoes = 0;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        bolaAtiva = false;

        //reseta valor do tempo entre objetos
        Menu.valorScrollbarTEO = TEO;

        //Contabiliza os pontos e ativa o efeito sonoro
        StartCoroutine(TrocaCor());
        pontos++;
        controle_do_som.playSFX(controle_do_som.sfxBola, 0.5f); 
    }

    //INICIA OU PAUSA O JOGO
    IEnumerator JogarPausar()
    {
        if (!jogoRodando)       //jogo rodando
        {
            selecionarJogo.interactable = false;
            jogoRodando = true;
            
            //ESPERA 3 SEGUNDO PARA INICIO
            for (tempoParaInicio = 3; tempoParaInicio > 0; tempoParaInicio--)       //espera 3 segundos
            {
                tempoInicio.text = tempoParaInicio.ToString();
                yield return new WaitForSeconds(1f);
            }

            StartCoroutine(Aparece());

            tempoInicio.enabled = false;
            imagemInicioTempo.enabled = false;      //image background texto tempo

            CoordenadasDaJuntas.CriaArquivodeDados();
        }
        else        //jogo pausado
        {
            selecionarJogo.interactable = true;

            //dados para a cena de estatisticas
            MenuDeEstatisticas.pontosEstatisticas = pontos;
            MenuDeEstatisticas.tempoEstatisticas = tempodeJogo;
            MenuDeEstatisticas.numeroAparicoesEstatisticas = apareceuBola;
            
            //RESTAURA TODAS AS VARIAVEIS PARA VALORES INICIAIS
            gameObject.transform.position = new Vector3(10f, 10f, -40f);
            trocaCompleta = true;
            tempoInicio.enabled = true;
            imagemInicioTempo.enabled = true;
            tempoInicio.text = "Espaço para iniciar";
            jogoRodando = false;
            Menu.maxTempo = tempoEscolido;
            Menu.maxPontos = pontoEscolido;
            Menu.valorScrollbarTEO = TEO;
            apareceuBola = 0;
            pontos = 0;
            tempodeJogo = 0;
            tempoParaInicio = 3;
            indiceArrayPosicoes = 0;
            escrevertxt = false;

            SceneManager.LoadScene("Estatísticas");
        }
    }

    //AS FUNCOES ABIXO SAO PARA A PARTE DE CRIAR JOGO APENAS

    public void SalvarJogo()
    {
        NomeArquivoJogo = nomeNovoJogo.text;        //pega o nome do inputFiled

        for (int i = 0; i < bola.Length; i++)       //pega x e y para salvar a posicao da bola. z n e necessario
        {
            posicaoBolaX[i] = bola[i].transform.position.x;
            posicaoBolaY[i] = bola[i].transform.position.y;
        }

        int g = 0;      //apenas para quando nome de jogos forem iguais

        ArquivoJogo = Menu.PastadeModosdeJogo + "\\" + NomeArquivoJogo + ".txt";
        if (!File.Exists(ArquivoJogo))
            File.WriteAllText(ArquivoJogo, "");
        else
        {
            while (File.Exists(ArquivoJogo))
            {
                g++;
                ArquivoJogo = Menu.PastadeModosdeJogo + "\\" + NomeArquivoJogo + "(" + g + ").txt";
            }
            File.WriteAllText(ArquivoJogo, "");
        }
        using (StreamWriter sw = File.AppendText(ArquivoJogo))
        {
            int i = 0;
            foreach (float line in posicaoBolaX)
            {
                sw.WriteLine(posicaoBolaX[i].ToString());
                sw.WriteLine(posicaoBolaY[i].ToString());
                sw.WriteLine("2.6");
                i++;
            }
        }

        if (g!=0)       //caso tenha algum jogo com nome igual, alterar nome no arquivo modo de jogo tambem
        {
            NomeArquivoJogo = NomeArquivoJogo + "(" + g + ")";
        }
        using (StreamWriter sw = File.AppendText(Menu.arquivoMododeJogo))
        {
            sw.WriteLine(NomeArquivoJogo + "," + bola.Length.ToString());
        }

        CarregarCena();
    }

    public void RetornaJogoSelecionado()
    {
        Menu.opcoes[6] = nomeJogo;

		File.Delete(Menu.arquivoOpcoes);
		using (StreamWriter sw = File.AppendText(Menu.arquivoOpcoes))       //altera arquivo  opcoes
        {
            foreach (string line in Menu.opcoes)
                sw.WriteLine(line);
        }

        posicaoLida = File.ReadAllLines(Menu.arquivoMododeJogo);        //le arquivo modo de jogo para saber quantas posicoes serao diferentes
        for (int i = 0; i < posicaoLida.Length; i++)
        {
            if (posicaoLida[i] == nomeJogo)
            {
                Menu.numerodeBolas = int.Parse(posicaoLida[i].Split(',')[1]);
                posicaoBola = new Vector3[Menu.numerodeBolas];
            }
        }

        AplicarPosicao();
    }

    public void AplicarPosicao()
    {
        int j = 0;
        string[] gamesave = File.ReadAllLines(Menu.PastadeModosdeJogo + "\\" + nomeJogo.Split(',')[0] + ".txt");        //le e aplica posicoes
        for (int i = 0; i < Menu.numerodeBolas * 3; i = i + 3)
        {
            posicaoBola[j] = new Vector3(float.Parse(gamesave[0 + i]), float.Parse(gamesave[1 + i]), float.Parse(gamesave[2 + i]));     //cria array com posicoes para o jogo
            j++;
        }
    }

    public void MenuPosicao()       //cria botoes para seecionar jogo
    {
        lerTextoPosicoes = File.ReadAllLines(Menu.arquivoMododeJogo);
        for (int i = 0; i < lerTextoPosicoes.Length; i++)
        {
            posicaoBotaoText.text = lerTextoPosicoes[i];
            GameObject positionbutton = Instantiate(posicaoBotao) as GameObject;
            positionbutton.SetActive(true);
            positionbutton.transform.SetParent(posicaoBotao.transform.parent, false);
        }
    }

    public void GerarBola()
    {
        PosicaoDaBola.numeroBola = 0;       //começa sempre no indice 0

        if (numberoBolasCriadas < Menu.numerodeBolas)
        {
            //instancia a bola dentro do array
            GameObject ball = Instantiate(prefabBola);
            ball.transform.position = new Vector3(-1.9f, 1.9f, 2.6f);
            bola[numberoBolasCriadas] = ball;

            //instancia text para a informações de distancia e angulo
            Text bolanome = Instantiate(prefabNomeBola);
            bolanome.transform.SetParent(prefabNomeBola.transform.parent, false);
            nomebola[numberoBolasCriadas] = bolanome;
            bolanome.text = "Bola " + (numberoBolasCriadas + 1) +":";

            ball.name = numberoBolasCriadas.ToString();     //muda nome do objeto na hierarquia

            numberoBolasCriadas++;
        }
    }

    public void CarregarCena()
    {
        Menu.jogando = false;
        SceneManager.LoadScene("Menu");
    }

    public void VerificaDistancia()     //apenas quando clica em salvar jogo
    {
        //verica no array se algum objeto ficou fora de alcance
        for (int i = 0; i < numberoBolasCriadas; i++)
        {
            if (testeAlcance[i] == 1)
            {
                naoSalvar = true;
            }
        }
        
        //caso fora de alcance nao salvar
        if (naoSalvar == false)
        {
            infoPainelCriarJogo.SetActive(false);
            SalvarJogo();
        }
        else
        {
            naoSalvar = false;
            infoPainelCriarJogo.SetActive(true);
        }
                  
    }
}