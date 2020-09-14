using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PosicaoDaBola : MonoBehaviour {

    private Vector3 offSetPonto;
    public static bool textoVermelho;
    public static int numeroBola;
    float porcentagemDis;
    float distancia;
    float angulo;
    
    Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }

    void OnMouseDown()
    {
        //diferença entre a posição do objeto e a do mouse
        offSetPonto = gameObject.transform.position - PosicaoMouseTela.pontoMundo;

        //qual bola sera movimentada                         
        numeroBola = int.Parse(gameObject.name);                                           
        
    }
    void OnMouseDrag()
    {
        //nova posição da bola
        transform.position = PosicaoMouseTela.pontoMundo + offSetPonto;

        //calculo dos angulos e distancia de acordo com o lado em que a bola esta                                          
        if (gameObject.transform.position.x < -0.171f)                                          
        {
            distancia = Mathf.Sqrt(((gameObject.transform.position.x + 0.171f) * (gameObject.transform.position.x + 0.171f)) + ((gameObject.transform.position.y - 0.341f) * (gameObject.transform.position.y - 0.341f)));
            porcentagemDis = (distancia / 1.014f) * 100f;
            angulo = (Mathf.Atan((gameObject.transform.position.y - 0.341f) / (gameObject.transform.position.x + 0.171f)) * Mathf.Rad2Deg - 90f) * -1f;
            Objeto.textoPosicao = porcentagemDis.ToString("f0") + "% e " + angulo.ToString("f0") + "°";
        }
        else
        {
            distancia = Mathf.Sqrt(((gameObject.transform.position.x + 0.171f) * (gameObject.transform.position.x + 0.171f)) + ((gameObject.transform.position.y - 0.341f) * (gameObject.transform.position.y - 0.341f)));
            porcentagemDis = (distancia / 1.014f) * 100f;
            angulo = Mathf.Atan((gameObject.transform.position.y - 0.341f) / (gameObject.transform.position.x + 0.171f)) * Mathf.Rad2Deg + 90f;
            Objeto.textoPosicao = porcentagemDis.ToString("f0") + "% e " + angulo.ToString("f0") + "°";
        }

        //muda cor do render e texto, caso >100 define a bola como fora do alcance (Objeto.teste)
        if (porcentagemDis > 100)
        {
            textoVermelho = true;
            render.material.color = Color.red;
            Objeto.testeAlcance[numeroBola] = 1;
        }
        else
        {
            textoVermelho = false;
            render.material.color = Color.white;
            Objeto.testeAlcance[numeroBola] = 0;
        }
            
    }
}
