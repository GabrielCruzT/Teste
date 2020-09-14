using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Menu : MonoBehaviour {

    
    public static float valorScrollbarTOA=2;
    public static float valorScrollbarTEO=2;
    public static float maxTempo;

    public static bool personagemMaculino = false;
    public static bool personagemFeminino = false;
    public static bool apagarNomesMenu = false;
    public static bool projetor;
    public static bool jogando = false;
    public static bool tempoBool = false;
    public static bool pontosBool = false;

    public static string arquivoUsuarios;
	public static string arquivoOpcoes;
    public static string arquivoMododeJogo;
    public static string PastadeDados;
    public static string jogoExcluido;
    public static string PastadeModosdeJogo;
    public static string nomeModoJogo;
    public static string[] opcoes = new string[8];
    public static string nomeJogador;

    public static int maxPontos;
    public static int numerodeBolas = 8;
    
    static string PastadeConfiguracoes;

    string[] lerTexto;
    string[] verificaOpcoes;
    string[] lerPosicoesTexto;

    public InputField campoNome;
    public InputField campoIdade;
    public InputField campoPontos;
    public InputField campoTempo;
    public InputField campoNumerodeBolas;
    public Toggle masculino;
    public Toggle feminino;
    public Toggle tela;
    public Toggle checkboxTempo;
    public Toggle checkboxPontos;
    public Scrollbar tempoObjetoAtivo;
    public Scrollbar tempoEntreObjetos;
    public Text tempoObjetoAtivoTexto;
    public Text tempoEntreObjetosTexto;
    public Text nomeTexto;
    public Text idadeTexto;
    public Text generoTexto;
    public Button criarUsuarioButton;
    public Button deletarUsuarioButton;
    public Button criarJogoButton;
    public Button excluirJogoButton;
    public Button iniciarButton;
    public Button selecionarUsuarioButton;

    [SerializeField]
    private Text textoBotaoScrollExcluirUsuario;
    [SerializeField]
    private GameObject BotaoScrollExcluirUsuario;
    [SerializeField]
    private Text textoBotaoScrollSelecionarUsuario;
    [SerializeField]
    private GameObject BotaoScrollSelecionarUsuario;
    [SerializeField]
    private GameObject PainelSalvar;
    [SerializeField]
    private GameObject OpcoesUI;
    [SerializeField]
    private GameObject MenuUI;
    [SerializeField]
    private GameObject scrollMenuSelecionar;
    [SerializeField]
    private GameObject scrollMenuExcluir;
    [SerializeField]
    private GameObject scrollMenuExcluirJogo;
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private Text posicaoNome;
    [SerializeField]
    private GameObject posicaoBotao;

    public void Start()
    {
		//configurando caminhos inciais
		PastadeDados = Application.persistentDataPath + "\\Dados";
		PastadeConfiguracoes = Application.persistentDataPath + "\\Configuracoes";
        PastadeModosdeJogo = Application.persistentDataPath + "\\ModosdeJogo";

		arquivoUsuarios = PastadeConfiguracoes + "\\Usuarios.txt";
		arquivoOpcoes = PastadeConfiguracoes + "\\Opcoes.txt";
        arquivoMododeJogo = PastadeModosdeJogo + "\\NomedosJogos.txt";

        // Criando pastas iniciais
        if (!Directory.Exists(PastadeDados))
			Directory.CreateDirectory(PastadeDados);

		if (!Directory.Exists(PastadeConfiguracoes))
			Directory.CreateDirectory(PastadeConfiguracoes);

        if(!Directory.Exists(PastadeModosdeJogo))
            Directory.CreateDirectory(PastadeModosdeJogo);

		//resetando vetor de jogador atual
        nomeJogador = "";

        //se nao tiver as opcoes criar arquivo com as opcoes iniciais
        /* linhas do txt
         * 1-modo de jogo por ponto
         * 2-quantidade de pontos
         * 3-modo de jogo por tempo
         * 4-quantidade de tempo
         * 5-tempo entre aparições
         * 6-tempo de objeto ativo
         * 7-nome do jogo selecionado
         * 8-modo com projetor
         */

        if (!File.Exists(arquivoOpcoes))
        {
            AplicarOpcoes();
        }
        else
        {
			opcoes = File.ReadAllLines(arquivoOpcoes);                 //lê arquivo de opções
            if (int.Parse(opcoes[0]) == 0)                             //verifica "linha 1" do txt
            {
                tempoBool = true;
                nomeModoJogo = "Tempo";
                checkboxTempo.isOn = true;
                campoPontos.text = opcoes[1];
                campoTempo.text = opcoes[3];
                maxTempo = float.Parse(opcoes[3]);
            }
            else
            {
                pontosBool = true;
                nomeModoJogo = "Ponto";
                checkboxPontos.isOn = true;
                campoPontos.text = opcoes[1];
                campoTempo.text = opcoes[3];
                maxPontos = int.Parse(opcoes[1]);
            }
            valorScrollbarTOA = float.Parse(opcoes[4]);                           
            valorScrollbarTEO = float.Parse(opcoes[5]);
            Objeto.nomeJogo = opcoes[6];                               
            projetor = bool.Parse(opcoes[7]);  
            tempoObjetoAtivo.value = (valorScrollbarTOA - 1) / 2;
            tempoEntreObjetos.value = (valorScrollbarTEO - 1) / 9;
        }
    }

    private void Update()
    {
        //conversão dos valores definidos no scrollbar
        valorScrollbarTOA = (tempoObjetoAtivo.value * 2) + 1;               
        tempoObjetoAtivoTexto.text = valorScrollbarTOA.ToString();
        valorScrollbarTEO = (tempoEntreObjetos.value * 9) + 1;
        tempoEntreObjetosTexto.text = valorScrollbarTEO.ToString();

        //verifica se foi preenchido todos os dados do usuário
        if (campoNome.text != "" && campoIdade.text != "" && (masculino.isOn || feminino.isOn) && scrollMenuExcluir.activeInHierarchy==false)  
            criarUsuarioButton.interactable = true;
        else
            criarUsuarioButton.interactable = false;

        //verifica se foi selecionado algum usuário  
        if (nomeJogador != "")                                                 
            iniciarButton.interactable = true;
        else
            iniciarButton.interactable = false;

        //verifica se foi definido um numero de bolas para a criação do jogo
        if (campoNumerodeBolas.text != "" && int.TryParse(campoNumerodeBolas.text, out numerodeBolas) && scrollMenuExcluirJogo.activeInHierarchy == false)
        {
            criarJogoButton.interactable = true;
            infoPanel.SetActive(false);
        }
        else
        {
            criarJogoButton.interactable = false;
            infoPanel.SetActive(true);
        }
            

        if (projetor)
            tela.isOn = true;

        //Separa as informações para diferentes objetos textos
        if (nomeJogador != "")
        {
            string[] player_data = nomeJogador.Split(',');
            nomeTexto.text = player_data[0];
            idadeTexto.text = player_data[1];
            generoTexto.text = player_data[2];
        }

        if (File.Exists(arquivoUsuarios) && scrollMenuSelecionar.activeInHierarchy == false && scrollMenuExcluir.activeInHierarchy == false)
        {
            selecionarUsuarioButton.interactable = true;
            deletarUsuarioButton.interactable = true;
        }   
        else
        {
            selecionarUsuarioButton.interactable = false;
            deletarUsuarioButton.interactable = false;
        }

        //funciona apenas quando se deleta usuário, limpando objeto texto com as informações dos usuários selecionados
        if (apagarNomesMenu == true)
        {
            nomeTexto.text = "";
            idadeTexto.text = "";
            generoTexto.text = "";
            apagarNomesMenu = false;
        }

        //verifica se pasta de modos de jogos está vazia
        if (Directory.GetFiles(PastadeModosdeJogo).Length <= 1)
            iniciarButton.interactable = false;

        if (File.Exists(arquivoMododeJogo) && scrollMenuExcluirJogo.activeInHierarchy == false) 
            excluirJogoButton.interactable = true;
        else
            excluirJogoButton.interactable = false;

    }

    //funções que resolvem o problema dos toggles
    public void MudaCheckTempo()
    {
        if (checkboxPontos.isOn == false)
            checkboxTempo.isOn = true;
    }
    public void MudaCheckPontos()
    {
        if (checkboxTempo.isOn == false)
            checkboxPontos.isOn = true;
    }

    public void IniciarJogo()
    {
        string personagem = nomeJogador.Split(',')[2];

        //verifica qual personagem escolher de acordo com o sexo definido na criação do usuário
        if (personagem == "M")                                               
            personagemMaculino = true;
        else
            personagemMaculino = false;
        if (personagem == "F")
            personagemFeminino = true;
        else
            personagemFeminino = false;

        jogando = true;

        SceneManager.LoadScene("Principal");
    }

    public void CriarUsuario()
    {
        if (masculino.isOn)
            SalvarDados.generosalvar = "M";
        else if(feminino.isOn)
            SalvarDados.generosalvar = "F";

        SalvarDados.nomeSalvar = campoNome.text;
        SalvarDados.idadeSalvar = campoIdade.text;
        SalvarDados.AdicionarUsuario();

        campoNome.text = "";
        campoIdade.text = "";

        masculino.isOn = false;
        feminino.isOn = false;
    }

    public void SelecionarUsuario()
    {
        int i = 1;
        //le arquivo de usuarios
		lerTexto = File.ReadAllLines(arquivoUsuarios);
        foreach(string Bname in lerTexto)
        {
            //pula "primeira linha"
            if (i==1)                                                    
            {
                i++;
                continue;
            }
            textoBotaoScrollSelecionarUsuario.text = Bname;
            GameObject button = Instantiate(BotaoScrollSelecionarUsuario) as GameObject;        //cria um botão para cada usuario registrado
            button.SetActive(true);                                                              
            button.transform.SetParent(BotaoScrollSelecionarUsuario.transform.parent, false);   //define posições para que fiquem em ordem no panel de botões 
        }
        i = 1;
    }

    public void DeletarUsuario()
    {
        int i = 1;
        lerTexto = File.ReadAllLines(arquivoUsuarios);                  
        foreach (string Bname in lerTexto)
        {
            if (i == 1)                                                    
            {
                i++;
                continue;
            }
            textoBotaoScrollExcluirUsuario.text = Bname;
            GameObject button = Instantiate(BotaoScrollExcluirUsuario) as GameObject;       
            button.SetActive(true);                                              
            button.transform.SetParent(BotaoScrollExcluirUsuario.transform.parent, false);   
        }
        i = 1;
    }

    public void AplicarOpcoes()
    {
        if (checkboxPontos.isOn)
        {
            pontosBool = true;

            if (!int.TryParse(campoPontos.text, out maxPontos) || int.Parse(campoPontos.text) <= 0)
                maxPontos = 5;
            else
                maxPontos = int.Parse(campoPontos.text);

            nomeModoJogo = "Ponto";
            opcoes[0] = 1.ToString();
            opcoes[1] = maxPontos.ToString();
            opcoes[2] = 0.ToString();
            opcoes[3] = 60.ToString();
            tempoBool = false;
        }
        else if (checkboxTempo.isOn)
        {
            tempoBool = true;

            if (!float.TryParse(campoTempo.text, out maxTempo) || float.Parse(campoTempo.text) <= 0)
                maxTempo = 60f;
            else
                maxTempo = float.Parse(campoTempo.text);

            nomeModoJogo = "Tempo";
            opcoes[0] = 0.ToString();
            opcoes[1] = 5.ToString();
            opcoes[2] = 1.ToString();
            opcoes[3] = maxTempo.ToString();
            pontosBool = false;
        }
        else        //opções de default
        {
            tempoBool = true;
            nomeModoJogo = "Tempo";
            opcoes[0] = 0.ToString();
            opcoes[1] = 5.ToString();
            opcoes[2] = 1.ToString();
            opcoes[3] = 60.ToString();
            pontosBool = false;
        }

        opcoes[4] = valorScrollbarTOA.ToString();
        opcoes[5] = valorScrollbarTEO.ToString();
        opcoes[6] = Objeto.nomeJogo;
        opcoes[7] = projetor.ToString();

        if (!File.Exists(arquivoOpcoes))
        {
            SalvarOpcoes();
            Start();
        }
        else
            VerificaOpcoes();
    }

    void VerificaOpcoes()
    {
        int i = 0;
        foreach (string op in opcoes)
        {
            if (op != verificaOpcoes[i])        //array 'verificaOp' criado quando entra no menu de opções
            {
                PainelSalvar.SetActive(true);   //ativa painel se confirmação
                break;
            }
            i++;
        }
        if (i == opcoes.Length)     //caso os arrays sejam iguais não ativa o painel e já muda para menu principal
            MudarUI();

    }

    public void SalvarOpcoes()      //caso clique na opção sim no savepanel
    {
        File.Delete(arquivoOpcoes);
        using (StreamWriter sw = File.AppendText(arquivoOpcoes))
        {
            foreach (string line in opcoes)
            {
                sw.WriteLine(line);
            }
        }
        opcoes = File.ReadAllLines(arquivoOpcoes);      //atualiza array opcoes

        MudarUI();
    }

    public void LerOpcoes()     //apenas para quando entrar no menu de opções
    {
        if(File.Exists(arquivoOpcoes))
            verificaOpcoes = File.ReadAllLines(arquivoOpcoes);
    }

    public void MudarUI()
    {
        MenuUI.SetActive(true);
        OpcoesUI.SetActive(false);
    }

    public void BotaoVoltar()       //apaga conteudos dos inputfields 
    {
        campoNome.text = "";
        campoIdade.text = "";
    }

    public void FecharJogo()
    {
        Application.Quit();
    }
    
    public void DeletarNome()       //apos excluir usuario retira o nome caso ele tenha sido selecionado
    {
        nomeJogador = "";
    }

    public void CriarJogo()
    {
        jogando = false;
        SceneManager.LoadScene("Principal");
    }

    public void MostrarPasta()        
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    } 

    public void Projetor()
    {
        if (tela.isOn)
            projetor = true;
        else
            projetor = false;
        Resolucao.MudaResolucao();
    }

    public void MenuPosicoes()      //scroll para excluir jogo
    {
        lerPosicoesTexto = File.ReadAllLines(Menu.arquivoMododeJogo);
        for (int i = 0; i < lerPosicoesTexto.Length; i++)
        {
            posicaoNome.text = lerPosicoesTexto[i];
            GameObject positionbutton = Instantiate(posicaoBotao) as GameObject;
            positionbutton.SetActive(true);
            positionbutton.transform.SetParent(posicaoBotao.transform.parent, false);

        }
    }

    public void ExcluirJogo()
    {
        //se o jogo que sera excluido estiver selecionao no arquivo opcoes
        if (opcoes[6] == jogoExcluido)
        {
            opcoes[6] = "";
            File.Delete(arquivoOpcoes);
            using (StreamWriter sw = File.AppendText(arquivoOpcoes))
                foreach (string line in opcoes)
                    sw.WriteLine(line);
        }            

        lerTexto = File.ReadAllLines(arquivoMododeJogo);
        File.Delete(arquivoMododeJogo);
        using (StreamWriter sw = File.AppendText(arquivoMododeJogo))        //reescreve arquivo com o nome dos modos de jogo
        {
            foreach (string line in lerTexto)
            {
                if (line == jogoExcluido)
                    continue;
                else
                    sw.WriteLine(line);
            }
        }
        File.Delete(PastadeModosdeJogo + "\\" + jogoExcluido.Split(',')[0] + ".txt");       //exclui arquivo com posicoes
    }
}
