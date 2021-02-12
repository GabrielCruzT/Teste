using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PosicaoDaBola : MonoBehaviour {

    private Vector3 offSetPonto;
    float distancia;
    float porcentagemDis;
    float angulo;
    public static int numeroBola;
    public static bool textoVermelho = true;
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
		numeroBola = int.Parse (gameObject.name);
    }
    void OnMouseDrag()
    {
        //nova posição da bola
        transform.position = PosicaoMouseTela.pontoMundo + offSetPonto;

        //calculo dos angulos e distancia de acordo com o lado em que a bola esta                                          
        if (gameObject.transform.position.x < 0)                                          
        {
            distancia = Mathf.Sqrt(((gameObject.transform.position.x) * (gameObject.transform.position.x)) + ((gameObject.transform.position.y) * (gameObject.transform.position.y)));
            porcentagemDis = (distancia) * 100f;
            angulo = (Mathf.Atan((gameObject.transform.position.y) / (gameObject.transform.position.x)) * Mathf.Rad2Deg - 90f) * -1f;
			CriarJogo.textoPosicao = porcentagemDis.ToString("f0") + "% e " + angulo.ToString("f0") + "°";
        }
        else
        {
            distancia = Mathf.Sqrt(((gameObject.transform.position.x) * (gameObject.transform.position.x)) + ((gameObject.transform.position.y) * (gameObject.transform.position.y)));
            porcentagemDis = (distancia) * 100f;
            angulo = Mathf.Atan((gameObject.transform.position.y) / (gameObject.transform.position.x)) * Mathf.Rad2Deg + 90f;
			CriarJogo.textoPosicao = porcentagemDis.ToString("f0") + "% e " + angulo.ToString("f0") + "°";
        }

        //muda cor do render e texto, caso >100 define a bola como fora do alcance (Objeto.teste)
        if (porcentagemDis > 100)
        {
            textoVermelho = true;
            render.material.color = Color.red;
            CriarJogo.testeAlcance[numeroBola] = 1;
        }
        else
        {
            textoVermelho = false;
            render.material.color = Color.white;
            CriarJogo.testeAlcance[numeroBola] = 0;
        }
            
    }
}
