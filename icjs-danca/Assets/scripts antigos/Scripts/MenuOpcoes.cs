using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuOpcoes : MonoBehaviour{

	public Toggle checkboxPontos;
	public Toggle checkboxTempo;
	public Toggle checkboxProjetor;
	public InputField campoPontos;
	public InputField campoTempo;
	public Scrollbar tempoObjetoAtivo;
	public Scrollbar tempoEntreObjetos;
	public Text tempoObjetoAtivoTexto;
	public Text tempoEntreObjetosTexto;
	private Diretorios_Salvar diretorioSalvar = new Diretorios_Salvar();
	public static float valorScrollbarTOA=2;
	public static float valorScrollbarTEO=2;
    public static bool projetor;
    public static string[] opcoesIniciais = new string[8];
    private int maxPontos;
	private float maxTempo;
	private string[] verificaOp;
    public static string[] opcoesSelecionadas = new string[8];

    [SerializeField]
	private GameObject PainelSalvar;

	public void Start()
	{
		opcoesSelecionadas = File.ReadAllLines (Diretorios_Salvar.arquivoOpcoes);
		verificaOp = File.ReadAllLines (Diretorios_Salvar.arquivoOpcoes);

		if (int.Parse(opcoesSelecionadas[0]) == 0)                             
		{
			checkboxTempo.isOn = true;
			campoPontos.text = opcoesSelecionadas[1];
			campoTempo.text = opcoesSelecionadas[3];
			maxTempo = float.Parse(opcoesSelecionadas[3]);
		}
		else
		{
			checkboxPontos.isOn = true;
			campoPontos.text = opcoesSelecionadas[1];
			campoTempo.text = opcoesSelecionadas[3];
			maxPontos = int.Parse(opcoesSelecionadas[1]);
		}
		valorScrollbarTOA = float.Parse(opcoesSelecionadas[4]);                           
		valorScrollbarTEO = float.Parse(opcoesSelecionadas[5]);
		Objeto.nomeJogo = opcoesSelecionadas[6];                               
		checkboxProjetor.isOn = bool.Parse(opcoesSelecionadas[7]);  
		tempoObjetoAtivo.value = (valorScrollbarTOA - 1) / 2;
		tempoEntreObjetos.value = (valorScrollbarTEO - 1) / 9;
        Debug.Log(projetor);
	}

	public void Update()
	{
		valorScrollbarTOA = (tempoObjetoAtivo.value * 2) + 1;               
		tempoObjetoAtivoTexto.text = valorScrollbarTOA.ToString();
		valorScrollbarTEO = (tempoEntreObjetos.value * 9) + 1;
		tempoEntreObjetosTexto.text = valorScrollbarTEO.ToString();
		
	}

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

	public void AplicarOpcoes()
	{
		if (checkboxPontos.isOn)
		{
			if (!int.TryParse(campoPontos.text, out maxPontos) || int.Parse(campoPontos.text) <= 0)
				maxPontos = 5;
			else
				maxPontos = int.Parse(campoPontos.text);

			opcoesSelecionadas[0] = 1.ToString();
			opcoesSelecionadas[1] = maxPontos.ToString();
			opcoesSelecionadas[2] = 0.ToString();
			opcoesSelecionadas[3] = 60.ToString();
		}
		else if (checkboxTempo.isOn)
		{
			if (!float.TryParse(campoTempo.text, out maxTempo) || float.Parse(campoTempo.text) <= 0)
				maxTempo = 60f;
			else
				maxTempo = float.Parse(campoTempo.text);

			opcoesSelecionadas[0] = 0.ToString();
			opcoesSelecionadas[1] = 5.ToString();
			opcoesSelecionadas[2] = 1.ToString();
			opcoesSelecionadas[3] = maxTempo.ToString();
		}

		opcoesSelecionadas[4] = valorScrollbarTOA.ToString();
		opcoesSelecionadas[5] = valorScrollbarTEO.ToString();
		opcoesSelecionadas[6] = Objeto.nomeJogo;

		if (checkboxProjetor.isOn)
			projetor = true;
		else
			projetor = false;

		opcoesSelecionadas[7] = projetor.ToString();

		VerificaOpcoes ();
	}

	public static string[] OpcoesIniciais()
	{
		opcoesIniciais [0] = "0";
		opcoesIniciais [1] = "0";
		opcoesIniciais [2] = 1.ToString ();
		opcoesIniciais [3] = 60.ToString ();
		opcoesIniciais [4] = "2";
		opcoesIniciais [5] = "2";
		opcoesIniciais [6] = "";
		opcoesIniciais [7] = "false";

		return opcoesIniciais;
	}

	public void VerificaOpcoes()
	{
		int i=0;
		foreach (string op in opcoesSelecionadas)
		{
			if (op != verificaOp[i])        //array 'verificaOp' criado quando entra no menu de opções
			{
				PainelSalvar.SetActive(true);   //ativa painel se confirmação
				break;
			}
			i++;
		}
	}

	public void PainelSim()
	{
		Diretorios_Salvar.SalvarDadosTeste (opcoesSelecionadas, Diretorios_Salvar.arquivoOpcoes);
        Resolucao.MudaResolucao();
        Start();
    }

    public static void ReescreveOpcoes(string novoJogo)
    {
        string[] novasOpcoes = File.ReadAllLines(Diretorios_Salvar.arquivoOpcoes);
        File.Delete(Diretorios_Salvar.arquivoOpcoes);
        novasOpcoes[6] = novoJogo;
        using (StreamWriter sw = File.AppendText(Diretorios_Salvar.arquivoOpcoes))       //altera arquivo  opcoes
        {
            foreach (string line in novasOpcoes)
                sw.WriteLine(line);
        }
    }

}
