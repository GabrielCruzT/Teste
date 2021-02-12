using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class CriarJogo : MonoBehaviour {

    public static string textoPosicao;
    public static int[] testeAlcance= new int[MenuCriarJogo.numeroDeBolas];

    private float[] posicaoBolaX;
    private float[] posicaoBolaY;
    private int numberoBolasCriadas = 0;
    private string ArquivoJogo;
    private string NomeArquivoJogo;
    private string[] posicaoLida;
    private string[] lerTextoPosicoes;
    private bool naoSalvar = false;

    [SerializeField]
    private Text[] nomebola;
    [SerializeField]
    private Button criarJogo;
    [SerializeField]
    private InputField nomeNovoJogo;
    [SerializeField]
    private GameObject prefabBola;
    [SerializeField]
    private GameObject[] bola;
    [SerializeField]
    private Text prefabNomeBola;
    [SerializeField]
    private GameObject infoPainelCriarJogo;
    [SerializeField]
    private Button botaoGerarBola;

    public Text[] textoPainelBolas;

    void Start()
    {
        bola = new GameObject[MenuCriarJogo.numeroDeBolas];
        posicaoBolaX = new float[MenuCriarJogo.numeroDeBolas];
        posicaoBolaY = new float[MenuCriarJogo.numeroDeBolas];
        nomebola = new Text[MenuCriarJogo.numeroDeBolas];
    }
    void Update()
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

		if (MenuCriarJogo.numeroDeBolas > numberoBolasCriadas)
			criarJogo.interactable = false;
		else
			criarJogo.interactable = true;
	}

	public void SalvarJogo()
	{
		NomeArquivoJogo = nomeNovoJogo.text;        //pega o nome do inputFiled

		for (int i = 0; i < bola.Length; i++)       //pega x e y para salvar a posicao da bola. z n e necessario
		{
			posicaoBolaX[i] = bola[i].transform.position.x;
			posicaoBolaY[i] = bola[i].transform.position.y;
		}

		int g = 0;      //apenas para quando nome de jogos forem iguais

		ArquivoJogo = Diretorios_Salvar.PastadeModosdeJogo + "\\" + NomeArquivoJogo + "," + bola.Length + ".txt";
		if (!File.Exists(ArquivoJogo))
			File.WriteAllText(ArquivoJogo, "");
		else
		{
			while (File.Exists(ArquivoJogo))
			{
				g++;
				ArquivoJogo = Diretorios_Salvar.PastadeModosdeJogo + "\\" + NomeArquivoJogo + "," + bola.Length + "(" + g + ").txt";
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
				sw.WriteLine("95");
				i++;
			}
		}

		if (g!=0)       //caso tenha algum jogo com nome igual, alterar nome no arquivo modo de jogo tambem
		{
			NomeArquivoJogo = NomeArquivoJogo + "(" + g + ")";
		}
		using (StreamWriter sw = File.AppendText(Diretorios_Salvar.arquivoMododeJogo))
		{
			sw.WriteLine(NomeArquivoJogo + "," + bola.Length.ToString());
		}

		CarregarCena();
	}
		
	public void CarregarCena()
	{
	    MenuPrincipal.jogando = false;
		SceneManager.LoadScene("Menu");
	}

	public void GerarBola()
	{
		//começa sempre no indice 0

		if (numberoBolasCriadas < MenuCriarJogo.numeroDeBolas) {
			//instancia a bola dentro do array
			GameObject ball = Instantiate (prefabBola);
			ball.transform.position = new Vector3 (4f, 28f, 95f);
			bola [numberoBolasCriadas] = ball;
            ball.GetComponent<Renderer>().material.color= Color.red;

            //instancia text para a informações de distancia e angulo
            Text bolanome = Instantiate (prefabNomeBola);
			bolanome.transform.SetParent (prefabNomeBola.transform.parent, false);
			nomebola [numberoBolasCriadas] = bolanome;
			bolanome.text = "Bola " + (numberoBolasCriadas + 1) + ": ";

			ball.name = numberoBolasCriadas.ToString ();     //muda nome do objeto na hierarquia
            testeAlcance[numberoBolasCriadas] = 1;
			numberoBolasCriadas++;
			if (numberoBolasCriadas >= MenuCriarJogo.numeroDeBolas)
				botaoGerarBola.interactable = false;
		} 
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
