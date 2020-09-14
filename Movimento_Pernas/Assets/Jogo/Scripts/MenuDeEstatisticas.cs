using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuDeEstatisticas : MonoBehaviour {

    public Text Pontos;
    public Text Acertos;
    public Text Tempo;
    public Text Jogador;
    public static int pontosEstatisticas;
    public static int numeroAparicoesEstatisticas;
    public static float tempoEstatisticas;
    

    private void Start()
    {
        Menu.jogando = false;
        if (Objeto.jogoRodando == false)
        {
            Jogador.text = "JOGADOR: " + Menu.nomeJogador.Split(',')[0];
            Acertos.text = "ACERTOS: " + (((float)pontosEstatisticas / (float)numeroAparicoesEstatisticas) *100).ToString("f2") + "%"; //calcula a porcentagem de acertos
            Pontos.text = "PONTOS: " + pontosEstatisticas.ToString();
            Tempo.text = "TEMPO DE JOGO: " + tempoEstatisticas.ToString("f0") + " segundos";
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Principal");
            Menu.jogando = true;
        }      
    }
}
