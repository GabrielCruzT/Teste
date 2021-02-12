using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class Calibracao : MonoBehaviour {

    int referencia;
    Vector3 posicaoPescoco;
    Vector3 posicaoPulsoDireito;
    Vector3 posicaoPulsoEsquerdo;
    float[] distanciaDireita = new float[5];
    float[] distanciaEsquerda = new float[5];
    float mDisEsq;
    float mDisDir;
    bool DisValida;

    private Animator anim;

    [SerializeField]
    Button botaoCalibrar;
    [SerializeField]
    GameObject persCalibrado;
    [SerializeField]
    AudioSource contadorMp3;
    [SerializeField]
    Text contagem;

    void Start () {
        referencia = this.transform.GetSiblingIndex();
        persCalibrado.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }

    public void IniciarCalibracao()
    {
        StartCoroutine(Cronometro());
    }

    void CalibrarDistanciaBraco(int i)
    {
        foreach (var gameObj in FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Joint(Clone)")
            {
                if (gameObj.transform.GetSiblingIndex() == referencia + 1)
                {
                    if (gameObj.activeInHierarchy == true)
                        posicaoPescoco = gameObj.transform.position;
                }
                else if (gameObj.transform.GetSiblingIndex() == referencia + 4)
                {
                    if (gameObj.activeInHierarchy == true)
                        posicaoPulsoEsquerdo = gameObj.transform.position;
                }
                else if (gameObj.transform.GetSiblingIndex() == referencia + 7)
                {
                    if (gameObj.activeInHierarchy == true)
                        posicaoPulsoDireito = gameObj.transform.position;
                }

                if (gameObj.transform.GetSiblingIndex() >= referencia + 19)
                    referencia = referencia + 19;
            }
        }
        distanciaEsquerda[i] = CalcularDistancia(posicaoPescoco, posicaoPulsoEsquerdo);
        distanciaDireita[i] = CalcularDistancia(posicaoPescoco, posicaoPulsoDireito);
    }
    float CalcularDistancia(Vector3 posi1, Vector3 posi2)
    {
        float distancia;
        distancia = Mathf.Sqrt(((posi2.x - posi1.x) * (posi2.x - posi1.x)) + ((posi2.y - posi1.y) * (posi2.y - posi1.y)));
        return distancia;
    }

    IEnumerator Cronometro()
    {
        persCalibrado.SetActive(false);
        botaoCalibrar.interactable = false;
        contadorMp3.Play();
        contagem.text = 5.ToString();
        for (int i=0;i<5;i++)
        {
            contagem.text = (5-i).ToString();
            CalibrarDistanciaBraco(i);
            yield return new WaitForSeconds(1f);
        }
        contagem.text = 5.ToString();
        if (VerificaCalibracao(distanciaEsquerda) && VerificaCalibracao(distanciaDireita))
        {
            mDisEsq = PegaMediaDistancia(distanciaEsquerda);
            mDisDir = PegaMediaDistancia(distanciaDireita);
            SalvarDistanciaUsuario(mDisEsq, mDisDir);
        }
        persCalibrado.SetActive(true);
        botaoCalibrar.interactable = true;

            

    }

    bool VerificaCalibracao(float[] distancia)
    {
        float somaDistancia = 0;
        float mediaDistancia = 0;
        for (int i=1;i<distancia.Length;i++)
        {
                somaDistancia = somaDistancia + distancia[i];
        }
        mediaDistancia = somaDistancia / 4;

        for (int i = 0; i < distancia.Length; i++)
        {
            if (distancia[i] < mediaDistancia * 0.95 || distancia[i] > mediaDistancia * 1.05)
                return false;
        }
        return true;
    }

    float PegaMediaDistancia(float[] distancia)
    {
        float somaDistancia = 0;
        float mediaDistancia = 0;
        for (int i = 1; i < distancia.Length; i++)
        {
                somaDistancia = somaDistancia + distancia[i];
        }
        mediaDistancia = somaDistancia / 4;
        return mediaDistancia;
    }

    void SalvarDistanciaUsuario(float disDir, float disEsq)
    {
        Diretorios_Salvar.SalvarDistanciaUsuario(mDisEsq,mDisDir);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
