using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pause : MonoBehaviour {

    bool isPause;
    public Text tempoInicio;

    void Start()
    {
        Pause();
    }

    void Pause()
    {
        //Pausa a simulacao do jogo
        if (gameObject.tag == "ObjetoEsquerdo")
        {
            transform.position = new Vector3(0.25f, 1.2f, -1f);
        }
        Time.timeScale = 0;
        tempoInicio.text = "Aperte Espaço para iniciar a Simulação";
    }

    void UnPause()
    {
        //Desativa a pausa da simulacao do jogo
        if (gameObject.tag == "ObjetoEsquerdo")
        {
            transform.position = new Vector3(0.25f, 1.2f, 2.4f);
        }
        Time.timeScale = 1;
        tempoInicio.text = "Aperte Espaço para pausar a Simulação";

    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPause = !isPause;
            if (isPause)
            {
                UnPause();
            }
            else
            {
                Pause();
                
            }
        }   
	}
}
