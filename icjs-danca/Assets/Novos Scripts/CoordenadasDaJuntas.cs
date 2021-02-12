using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CoordenadasDaJuntas : MonoBehaviour
{

    //Coordenadas de cada joint

    float posicaoCabecaX;
    float posicaoCabecaY;
    float posicaoCabecaZ;
    float posicaoTroncoX;
    float posicaoTroncoY;
    float posicaoTroncoZ;
    float posicaoNucaX;
    float posicaoNucaY;
    float posicaoNucaZ;
    float posicaoBasePescocoX;
    float posicaoBasePescocoY;
    float posicaoBasePescocoZ;
    float posicaoCotoveloDirX;
    float posicaoCotoveloDirY;
    float posicaoCotoveloDirZ;
    float posicaoCotoveloEsqX;
    float posicaoCotoveloEsqY;
    float posicaoCotoveloEsqZ;
    float posicaoOmbroDirX;
    float posicaoOmbroDirY;
    float posicaoOmbroDirZ;
    float posicaoOmbroEsqX;
    float posicaoOmbroEsqY;
    float posicaoOmbroEsqZ;
    float posicaoPulsoDirX;
    float posicaoPulsoDirY;
    float posicaoPulsoDirZ;
    float posicaoPulsoEsqX;
    float posicaoPulsoEsqY;
    float posicaoPulsoEsqZ;
    float posicaoMaoDireitaX;
    float posicaoMaoDireitaY;
    float posicaoMaoDireitaZ;
    float posicaoMaoEsquerdaX;
    float posicaoMaoEsquerdaY;
    float posicaoMaoEsquerdaZ;
    float posicaoQuadrilX;
    float posicaoQuadrilY;
    float posicaoQuadrilZ;
    float posicaoQuadrilDireitoX;
    float posicaoQuadrilDireitoY;
    float posicaoQuadrilDireitoZ;
    float posicaoQuadrilEsquerdoX;
    float posicaoQuadrilEsquerdoY;
    float posicaoQuadrilEsquerdoZ;
    float posicaoJoelhoDireitoX;
    float posicaoJoelhoDireitoY;
    float posicaoJoelhoDireitoZ;
    float posicaoJoelhoEsquerdoX;
    float posicaoJoelhoEsquerdoY;
    float posicaoJoelhoEsquerdoZ;
    float posicaoPeDireitoX;
    float posicaoPeDireitoY;
    float posicaoPeDireitoZ;
    float posicaoPeEsquerdoX;
    float posicaoPeEsquerdoY;
    float posicaoPeEsquerdoZ;

    //Vetores para calculo do angulo

    float vetorCotoveloDirOmbroDirX;
    float vetorCotoveloDirOmbroDirY;
    float vetorCotoveloDirOmbroDirZ;
    float vetorPulsoDirCotoveloDirX;
    float vetorPulsoDirCotoveloDirY;
    float vetorPulsoDirCotoveloDirZ;
    float vetorCotoveloEsqOmbroEsqX;
    float vetorCotoveloEsqOmbroEsqY;
    float vetorCotoveloEsqOmbroEsqZ;
    float vetorPulsoEsqCotoveloEsqX;
    float vetorPulsoEsqCotoveloEsqY;
    float vetorPulsoEsqCotoveloEsqZ;
    float vetorOmbroDirOmbroEsqX;
    float vetorOmbroDirOmbroEsqY;
    float vetorOmbroDirOmbroEsqZ;
    float vetorTroncoNucaX;
    float vetorTroncoNucaY;
    float vetorTroncoNucaZ;
    float angulo;
    float cos;

    //Texto para interface

    public Text anguloCotoveloDir;
    public Text anguloOmbroDir;
    public Text anguloCotoveloEsq;
    public Text anguloOmbroEsq;
    public Text cabecaEstado;
    public Text cabecaX;
    public Text cabecaY;
    public Text cabecaZ;
    public Text basePescocoEstado;
    public Text basePescocoX;
    public Text basePescocoY;
    public Text basePescocoZ;
    public Text ombroEsquerdoEstado;
    public Text ombroEsquerdoX;
    public Text ombroEsquerdoY;
    public Text ombroEsquerdoZ;
    public Text cotoveloEsquerdoEstado;
    public Text cotoveloEsquerdoX;
    public Text cotoveloEsquerdoY;
    public Text cotoveloEsquerdoZ;
    public Text maoEsquerdaEstado;
    public Text maoEsquerdaX;
    public Text maoEsquerdaY;
    public Text maoEsquerdaZ;
    public Text ombroDireitoEstado;
    public Text ombroDireitoX;
    public Text ombroDireitoY;
    public Text ombroDireitoZ;
    public Text cotoveloDireitoEstado;
    public Text cotoveloDireitoX;
    public Text cotoveloDireitoY;
    public Text cotoveloDireitoZ;
    public Text maoDireitaEstado;
    public Text maoDireitaX;
    public Text maoDireitaY;
    public Text maoDireitaZ;
    public Text troncoEstado;
    public Text troncoX;
    public Text troncoY;
    public Text troncoZ;
    public Text quadrilEstado;
    public Text quadrilX;
    public Text quadrilY;
    public Text quadrilZ;
    public Text quadrilEsquerdoEstado;
    public Text quadrilEsquerdoX;
    public Text quadrilEsquerdoY;
    public Text quadrilEsquerdoZ;
    public Text joelhoEsquerdoEstado;
    public Text joelhoEsquerdoX;
    public Text joelhoEsquerdoY;
    public Text joelhoEsquerdoZ;
    public Text peEsquerdoEstado;
    public Text peEsquerdoX;
    public Text peEsquerdoY;
    public Text peEsquerdoZ;
    public Text quadrilDireitoEstado;
    public Text quadrilDireitoX;
    public Text quadrilDireitoY;
    public Text quadrilDireitoZ;
    public Text joelhoDireitoEstado;
    public Text joelhoDireitoX;
    public Text joelhoDireitoY;
    public Text joelhoDireitoZ;
    public Text peDireitoEstado;
    public Text peDireitoX;
    public Text peDireitoY;
    public Text peDireitoZ;
    public Text pulsoEsquerdoEstado;
    public Text pulsoEsquerdoX;
    public Text pulsoEsquerdoY;
    public Text pulsoEsquerdoZ;
    public Text pulsoDireitoEstado;
    public Text pulsoDireitoX;
    public Text pulsoDireitoY;
    public Text pulsoDireitoZ;
    public Text nucaEstado;
    public Text nucaX;
    public Text nucaY;
    public Text nucaZ;

    static string arquivoDados;
    int ativoDesativo;
    int contagemInativa;
    int referencia;

    public static Vector3 posicaoPescoso;

    void Start()
    {
        referencia = this.transform.GetSiblingIndex();                  //Pega a referência dos objetos a partir do objeto em que está o script  
    }

    void Update()
    {
        contagemInativa = 0;
        Debug.Log("Referência Atual: " + referencia);
        foreach (var gameObj in FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Joint(Clone)")
            {
                if (gameObj.transform.GetSiblingIndex() == referencia + 0)
                {

                    if (gameObj.activeInHierarchy == true)
                    {
                        cabecaEstado.text = "Ativo";
                        this.cabecaEstado.color = Color.white;
                        Debug.Log("Referencia cabeca:" + referencia);
                        cabecaX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        cabecaY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        cabecaZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoCabecaX = gameObj.transform.position.x;
                        posicaoCabecaY = gameObj.transform.position.y;
                        posicaoCabecaZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        cabecaEstado.text = "Inativo";
                        this.cabecaEstado.color = Color.red;
                        cabecaX.text = "";
                        cabecaY.text = "";
                        cabecaZ.text = "";
                        posicaoCabecaX = 0;
                        posicaoCabecaY = 0;
                        posicaoCabecaZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 1)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        basePescocoEstado.text = "Ativo";
                        this.basePescocoEstado.color = Color.white;
                        basePescocoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        basePescocoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        basePescocoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoBasePescocoX = gameObj.transform.position.x;
                        posicaoBasePescocoY = gameObj.transform.position.y;
                        posicaoBasePescocoZ = gameObj.transform.position.z;

                        posicaoPescoso = gameObj.transform.position;

                    }
                    else
                    {
                        basePescocoEstado.text = "Inativo";
                        this.basePescocoEstado.color = Color.red;
                        basePescocoX.text = "";
                        basePescocoY.text = "";
                        basePescocoZ.text = "";
                        posicaoBasePescocoX = 0;
                        posicaoBasePescocoY = 0;
                        posicaoBasePescocoZ = 0;
                        contagemInativa++;
                    }


                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 2)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        ombroEsquerdoEstado.text = "Ativo";
                        this.ombroEsquerdoEstado.color = Color.white;
                        ombroEsquerdoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        ombroEsquerdoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        ombroEsquerdoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoOmbroEsqX = gameObj.transform.position.x;
                        posicaoOmbroEsqY = gameObj.transform.position.y;
                        posicaoOmbroEsqZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        ombroEsquerdoEstado.text = "Inativo";
                        this.ombroEsquerdoEstado.color = Color.red;
                        ombroEsquerdoX.text = "";
                        ombroEsquerdoY.text = "";
                        ombroEsquerdoZ.text = "";
                        posicaoOmbroEsqX = 0;
                        posicaoOmbroEsqY = 0;
                        posicaoOmbroEsqZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 3)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        cotoveloEsquerdoEstado.text = "Ativo";
                        this.cotoveloEsquerdoEstado.color = Color.white;
                        cotoveloEsquerdoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        cotoveloEsquerdoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        cotoveloEsquerdoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoCotoveloEsqX = gameObj.transform.position.x;
                        posicaoCotoveloEsqY = gameObj.transform.position.y;
                        posicaoCotoveloEsqZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        cotoveloEsquerdoEstado.text = "Inativo";
                        this.cotoveloEsquerdoEstado.color = Color.red;
                        cotoveloEsquerdoX.text = "";
                        cotoveloEsquerdoY.text = "";
                        cotoveloEsquerdoZ.text = "";
                        posicaoCotoveloEsqX = 0;
                        posicaoCotoveloEsqY = 0;
                        posicaoCotoveloEsqZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 4)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        maoEsquerdaEstado.text = "Ativo";
                        this.maoEsquerdaEstado.color = Color.white;
                        maoEsquerdaX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        maoEsquerdaY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        maoEsquerdaZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoMaoEsquerdaX = gameObj.transform.position.x;
                        posicaoMaoEsquerdaY = gameObj.transform.position.y;
                        posicaoMaoEsquerdaZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        maoEsquerdaEstado.text = "Inativo";
                        this.maoEsquerdaEstado.color = Color.red;
                        maoEsquerdaX.text = "";
                        maoEsquerdaY.text = "";
                        maoEsquerdaZ.text = "";
                        posicaoMaoEsquerdaX = 0;
                        posicaoMaoEsquerdaY = 0;
                        posicaoMaoEsquerdaZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 5)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        ombroDireitoEstado.text = "Ativo";
                        this.ombroDireitoEstado.color = Color.white;
                        ombroDireitoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        ombroDireitoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        ombroDireitoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoOmbroDirX = gameObj.transform.position.x;
                        posicaoOmbroDirY = gameObj.transform.position.y;
                        posicaoOmbroDirZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        ombroDireitoEstado.text = "Inativo";
                        this.ombroDireitoEstado.color = Color.red;
                        ombroDireitoX.text = "";
                        ombroDireitoY.text = "";
                        ombroDireitoZ.text = "";
                        posicaoOmbroDirX = 0;
                        posicaoOmbroDirY = 0;
                        posicaoOmbroDirZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 6)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        cotoveloDireitoEstado.text = "Ativo";
                        this.cotoveloDireitoEstado.color = Color.white;
                        cotoveloDireitoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        cotoveloDireitoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        cotoveloDireitoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoCotoveloDirX = gameObj.transform.position.x;
                        posicaoCotoveloDirY = gameObj.transform.position.y;
                        posicaoCotoveloDirZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        cotoveloDireitoEstado.text = "Inativo";
                        this.cotoveloDireitoEstado.color = Color.red;
                        cotoveloDireitoX.text = "";
                        cotoveloDireitoY.text = "";
                        cotoveloDireitoZ.text = "";
                        posicaoCotoveloDirX = 0;
                        posicaoCotoveloDirY = 0;
                        posicaoCotoveloDirZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 7)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        maoDireitaEstado.text = "Ativo";
                        this.maoDireitaEstado.color = Color.white;
                        maoDireitaX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        maoDireitaY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        maoDireitaZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoMaoDireitaX = gameObj.transform.position.x;
                        posicaoMaoDireitaY = gameObj.transform.position.y;
                        posicaoMaoDireitaZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        maoDireitaEstado.text = "Inativo";
                        this.maoDireitaEstado.color = Color.red;
                        maoDireitaX.text = "";
                        maoDireitaY.text = "";
                        maoDireitaZ.text = "";
                        posicaoMaoDireitaX = 0;
                        posicaoMaoDireitaY = 0;
                        posicaoMaoDireitaZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 8)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        troncoEstado.text = "Ativo";
                        this.troncoEstado.color = Color.white;
                        troncoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        troncoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        troncoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoTroncoX = gameObj.transform.position.x;
                        posicaoTroncoY = gameObj.transform.position.y;
                        posicaoTroncoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        troncoEstado.text = "Inativo";
                        this.troncoEstado.color = Color.red;
                        troncoX.text = "";
                        troncoY.text = "";
                        troncoZ.text = "";
                        posicaoTroncoX = 0;
                        posicaoTroncoY = 0;
                        posicaoTroncoZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 9)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        quadrilEstado.text = "Ativo";
                        this.quadrilEstado.color = Color.white;
                        quadrilX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        quadrilY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        quadrilZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoQuadrilX = gameObj.transform.position.x;
                        posicaoQuadrilY = gameObj.transform.position.y;
                        posicaoQuadrilZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        quadrilEstado.text = "Inativo";
                        this.quadrilEstado.color = Color.red;
                        quadrilX.text = "";
                        quadrilY.text = "";
                        quadrilZ.text = "";
                        posicaoQuadrilX = 0;
                        posicaoQuadrilY = 0;
                        posicaoQuadrilZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 10)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        quadrilEsquerdoEstado.text = "Ativo";
                        this.quadrilEsquerdoEstado.color = Color.white;
                        quadrilEsquerdoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        quadrilEsquerdoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        quadrilEsquerdoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoQuadrilEsquerdoX = gameObj.transform.position.x;
                        posicaoQuadrilEsquerdoY = gameObj.transform.position.y;
                        posicaoQuadrilEsquerdoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        quadrilEsquerdoEstado.text = "Inativo";
                        this.quadrilEsquerdoEstado.color = Color.red;
                        quadrilEsquerdoX.text = "";
                        quadrilEsquerdoY.text = "";
                        quadrilEsquerdoZ.text = "";
                        posicaoQuadrilEsquerdoX = 0;
                        posicaoQuadrilEsquerdoY = 0;
                        posicaoQuadrilEsquerdoZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 11)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        joelhoEsquerdoEstado.text = "Ativo";
                        this.joelhoEsquerdoEstado.color = Color.white;
                        joelhoEsquerdoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        joelhoEsquerdoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        joelhoEsquerdoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoJoelhoEsquerdoX = gameObj.transform.position.x;
                        posicaoJoelhoEsquerdoY = gameObj.transform.position.y;
                        posicaoJoelhoEsquerdoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        joelhoEsquerdoEstado.text = "Inativo";
                        this.joelhoEsquerdoEstado.color = Color.red;
                        joelhoEsquerdoX.text = "";
                        joelhoEsquerdoY.text = "";
                        joelhoEsquerdoZ.text = "";
                        posicaoJoelhoEsquerdoX = 0;
                        posicaoJoelhoEsquerdoY = 0;
                        posicaoJoelhoEsquerdoZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 12)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        peEsquerdoEstado.text = "Ativo";
                        this.peEsquerdoEstado.color = Color.white;
                        peEsquerdoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        peEsquerdoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        peEsquerdoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoPeEsquerdoX = gameObj.transform.position.x;
                        posicaoPeEsquerdoY = gameObj.transform.position.y;
                        posicaoPeEsquerdoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        peEsquerdoEstado.text = "Inativo";
                        this.peEsquerdoEstado.color = Color.red;
                        peEsquerdoX.text = "";
                        peEsquerdoY.text = "";
                        peEsquerdoZ.text = "";
                        posicaoPeEsquerdoX = 0;
                        posicaoPeEsquerdoY = 0;
                        posicaoPeEsquerdoZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 13)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        quadrilDireitoEstado.text = "Ativo";
                        this.quadrilDireitoEstado.color = Color.white;
                        quadrilDireitoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        quadrilDireitoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        quadrilDireitoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoQuadrilDireitoX = gameObj.transform.position.x;
                        posicaoQuadrilDireitoY = gameObj.transform.position.y;
                        posicaoQuadrilDireitoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        quadrilDireitoEstado.text = "Inativo";
                        this.quadrilDireitoEstado.color = Color.red;
                        quadrilDireitoX.text = "";
                        quadrilDireitoY.text = "";
                        quadrilDireitoZ.text = "";
                        posicaoQuadrilDireitoX = 0;
                        posicaoQuadrilDireitoY = 0;
                        posicaoQuadrilDireitoZ = 0;
                        contagemInativa++;
                    }

                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 14)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        joelhoDireitoEstado.text = "Ativo";
                        this.joelhoDireitoEstado.color = Color.white;
                        joelhoDireitoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        joelhoDireitoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        joelhoDireitoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoJoelhoDireitoX = gameObj.transform.position.x;
                        posicaoJoelhoDireitoY = gameObj.transform.position.y;
                        posicaoJoelhoDireitoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        joelhoDireitoEstado.text = "Inativo";
                        this.joelhoDireitoEstado.color = Color.red;
                        joelhoDireitoX.text = "";
                        joelhoDireitoY.text = "";
                        joelhoDireitoZ.text = "";
                        posicaoJoelhoDireitoX = 0;
                        posicaoJoelhoDireitoY = 0;
                        posicaoJoelhoDireitoZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 15)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        peDireitoEstado.text = "Ativo";
                        this.peDireitoEstado.color = Color.white;
                        peDireitoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        peDireitoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        peDireitoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoPeDireitoX = gameObj.transform.position.x;
                        posicaoPeDireitoY = gameObj.transform.position.y;
                        posicaoPeDireitoZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        peDireitoEstado.text = "Inativo";
                        this.peDireitoEstado.color = Color.red;
                        peDireitoX.text = "";
                        peDireitoY.text = "";
                        peDireitoZ.text = "";
                        posicaoPeDireitoX = 0;
                        posicaoPeDireitoY = 0;
                        posicaoPeDireitoZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 16)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        pulsoEsquerdoEstado.text = "Ativo";
                        this.pulsoEsquerdoEstado.color = Color.white;
                        pulsoEsquerdoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        pulsoEsquerdoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        pulsoEsquerdoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoPulsoEsqX = gameObj.transform.position.x;
                        posicaoPulsoEsqY = gameObj.transform.position.y;
                        posicaoPulsoEsqZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        pulsoEsquerdoEstado.text = "Inativo";
                        this.pulsoEsquerdoEstado.color = Color.red;
                        pulsoEsquerdoX.text = "";
                        pulsoEsquerdoY.text = "";
                        pulsoEsquerdoZ.text = "";
                        posicaoPulsoEsqX = 0;
                        posicaoPulsoEsqY = 0;
                        posicaoPulsoEsqZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 17)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        pulsoDireitoEstado.text = "Ativo";
                        this.pulsoDireitoEstado.color = Color.white;
                        pulsoDireitoX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        pulsoDireitoY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        pulsoDireitoZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoPulsoDirX = gameObj.transform.position.x;
                        posicaoPulsoDirY = gameObj.transform.position.y;
                        posicaoPulsoDirZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        pulsoDireitoEstado.text = "Inativo";
                        this.pulsoDireitoEstado.color = Color.red;
                        pulsoDireitoX.text = "";
                        pulsoDireitoY.text = "";
                        pulsoDireitoZ.text = "";
                        posicaoPulsoDirX = 0;
                        posicaoPulsoDirY = 0;
                        posicaoPulsoDirZ = 0;
                        contagemInativa++;
                    }
                }

                else if (gameObj.transform.GetSiblingIndex() == referencia + 18)
                {
                    if (gameObj.activeInHierarchy == true)
                    {
                        nucaEstado.text = "Ativo";
                        this.nucaEstado.color = Color.white;
                        nucaX.text = "X" + ": " + gameObj.transform.position.x.ToString("f2");
                        nucaY.text = "Y" + ": " + gameObj.transform.position.y.ToString("f2");
                        nucaZ.text = "Z" + ": " + gameObj.transform.position.z.ToString("f2");
                        posicaoNucaX = gameObj.transform.position.x;
                        posicaoNucaY = gameObj.transform.position.y;
                        posicaoNucaZ = gameObj.transform.position.z;
                    }
                    else
                    {
                        nucaEstado.text = "Inativo";
                        this.nucaEstado.color = Color.red;
                        nucaX.text = "";
                        nucaY.text = "";
                        nucaZ.text = "";
                        posicaoNucaX = 0;
                        posicaoNucaY = 0;
                        posicaoNucaZ = 0;
                        contagemInativa++;
                    }
                }
            }
        }
        if (contagemInativa == 19)
        {
            referencia = referencia + 19;
        }

        //pega distancia entre dois pontos
        vetorCotoveloDirOmbroDirX = posicaoOmbroDirX - posicaoCotoveloDirX;
        vetorCotoveloDirOmbroDirY = posicaoOmbroDirY - posicaoCotoveloDirY;
        vetorCotoveloDirOmbroDirZ = posicaoOmbroDirZ - posicaoCotoveloDirZ;
        vetorPulsoDirCotoveloDirX = posicaoCotoveloDirX - posicaoPulsoDirX;
        vetorPulsoDirCotoveloDirY = posicaoCotoveloDirY - posicaoPulsoDirY;
        vetorPulsoDirCotoveloDirZ = posicaoCotoveloDirZ - posicaoPulsoDirZ;

        vetorCotoveloEsqOmbroEsqX = posicaoOmbroEsqX - posicaoCotoveloEsqX;
        vetorCotoveloEsqOmbroEsqY = posicaoOmbroEsqY - posicaoCotoveloEsqY;
        vetorCotoveloEsqOmbroEsqZ = posicaoOmbroEsqZ - posicaoCotoveloEsqZ;
        vetorPulsoEsqCotoveloEsqX = posicaoCotoveloEsqX - posicaoPulsoEsqX;
        vetorPulsoEsqCotoveloEsqY = posicaoCotoveloEsqY - posicaoPulsoEsqY;
        vetorPulsoEsqCotoveloEsqZ = posicaoCotoveloEsqZ - posicaoPulsoEsqZ;

        vetorOmbroDirOmbroEsqX = posicaoOmbroDirX - posicaoOmbroEsqX;
        vetorOmbroDirOmbroEsqY = posicaoOmbroDirY - posicaoOmbroEsqY;
        vetorOmbroDirOmbroEsqZ = posicaoOmbroDirZ - posicaoOmbroEsqZ;

        vetorTroncoNucaX = posicaoTroncoX - posicaoNucaX;
        vetorTroncoNucaY = posicaoTroncoY - posicaoNucaY;
        vetorTroncoNucaZ = posicaoTroncoZ - posicaoNucaZ;

        CalculoDoAngulo(vetorCotoveloDirOmbroDirX, vetorCotoveloDirOmbroDirY, vetorCotoveloDirOmbroDirZ, vetorPulsoDirCotoveloDirX, vetorPulsoDirCotoveloDirY, vetorPulsoDirCotoveloDirZ); //ACD
        anguloCotoveloDir.text = angulo.ToString("f2");

        if (anguloCotoveloDir.text == "NaN")
        {
            anguloCotoveloDir.text = "500.00";
        }

        CalculoDoAngulo(vetorCotoveloEsqOmbroEsqX, vetorCotoveloEsqOmbroEsqY, vetorCotoveloEsqOmbroEsqZ, vetorPulsoEsqCotoveloEsqX, vetorPulsoEsqCotoveloEsqY, vetorPulsoEsqCotoveloEsqZ); //ACE
        anguloCotoveloEsq.text = angulo.ToString("f2");

        if (anguloCotoveloEsq.text == "NaN")
        {
            anguloCotoveloEsq.text = "500.00";
        }

        CalculoDoAngulo(vetorCotoveloDirOmbroDirX, vetorCotoveloDirOmbroDirY, vetorCotoveloDirOmbroDirZ, vetorTroncoNucaX, vetorTroncoNucaY, vetorTroncoNucaZ); //AOD
        anguloOmbroDir.text = angulo.ToString("f2");

        if (anguloOmbroDir.text == "NaN")
        {
            anguloOmbroDir.text = "500.00";
        }

        CalculoDoAngulo(vetorCotoveloEsqOmbroEsqX, vetorCotoveloEsqOmbroEsqY, vetorCotoveloEsqOmbroEsqZ, vetorTroncoNucaX, vetorTroncoNucaY, vetorTroncoNucaZ); //AOE
        anguloOmbroEsq.text = angulo.ToString("f2");

        if (anguloOmbroEsq.text == "NaN")
        {
            anguloOmbroEsq.text = "500.00";
        }

        //escreve informações da partida no arquivo de dados
        if (Objeto.escrevertxt == true && MenuPrincipal.jogando == true)
        {
            //converte bool para int
            if (Objeto.bolaAtiva)
                ativoDesativo = 1;
            else
                ativoDesativo = 0;

            string final_text = Objeto.tempodeJogo.ToString("f2") + "," +
                                Objeto.pontos.ToString() + "," +
                                Objeto.apareceuBola.ToString() + "," +
                                Objeto.bolaPosicaoAtual.x.ToString() + "," +
                                Objeto.bolaPosicaoAtual.y.ToString() + "," +
                                Objeto.bolaPosicaoAtual.z.ToString() + "," +
                                ativoDesativo.ToString() + "," +
                                anguloCotoveloDir.text + "," +
                                anguloOmbroDir.text + "," +
                                anguloCotoveloEsq.text + "," +
                                anguloOmbroEsq.text + "," +
                                posicaoCabecaX.ToString() + "," +
                                posicaoCabecaY.ToString() + "," +
                                posicaoCabecaZ.ToString() + "," +
                                posicaoNucaX.ToString() + "," +
                                posicaoNucaY.ToString() + "," +
                                posicaoNucaZ.ToString() + "," +
                                posicaoBasePescocoX.ToString() + "," +
                                posicaoBasePescocoY.ToString() + "," +
                                posicaoBasePescocoZ.ToString() + "," +
                                posicaoTroncoX.ToString() + "," +
                                posicaoTroncoY.ToString() + "," +
                                posicaoTroncoZ.ToString() + "," +
                                posicaoCotoveloDirX.ToString() + "," +
                                posicaoCotoveloDirY.ToString() + "," +
                                posicaoCotoveloDirZ.ToString() + "," +
                                posicaoCotoveloEsqX.ToString() + "," +
                                posicaoCotoveloEsqY.ToString() + "," +
                                posicaoCotoveloEsqZ.ToString() + "," +
                                posicaoOmbroDirX.ToString() + "," +
                                posicaoOmbroDirY.ToString() + "," +
                                posicaoOmbroDirZ.ToString() + "," +
                                posicaoOmbroEsqX.ToString() + "," +
                                posicaoOmbroEsqY.ToString() + "," +
                                posicaoOmbroEsqZ.ToString() + "," +
                                posicaoPulsoDirX.ToString() + "," +
                                posicaoPulsoDirY.ToString() + "," +
                                posicaoPulsoDirZ.ToString() + "," +
                                posicaoPulsoEsqX.ToString() + "," +
                                posicaoPulsoEsqY.ToString() + "," +
                                posicaoPulsoEsqZ.ToString() + "," +
                                posicaoMaoDireitaX.ToString() + "," +
                                posicaoMaoDireitaY.ToString() + "," +
                                posicaoMaoDireitaZ.ToString() + "," +
                                posicaoMaoEsquerdaX.ToString() + "," +
                                posicaoMaoEsquerdaY.ToString() + "," +
                                posicaoMaoEsquerdaZ.ToString() + "," +
                                posicaoQuadrilX.ToString() + "," +
                                posicaoQuadrilY.ToString() + "," +
                                posicaoQuadrilZ.ToString() + "," +
                                posicaoQuadrilDireitoX.ToString() + "," +
                                posicaoQuadrilDireitoY.ToString() + "," +
                                posicaoQuadrilDireitoZ.ToString() + "," +
                                posicaoQuadrilEsquerdoX.ToString() + "," +
                                posicaoQuadrilEsquerdoY.ToString() + "," +
                                posicaoQuadrilEsquerdoZ.ToString() + "," +
                                posicaoJoelhoDireitoX.ToString() + "," +
                                posicaoJoelhoDireitoY.ToString() + "," +
                                posicaoJoelhoDireitoZ.ToString() + "," +
                                posicaoJoelhoEsquerdoX.ToString() + "," +
                                posicaoJoelhoEsquerdoY.ToString() + "," +
                                posicaoJoelhoEsquerdoZ.ToString() + "," +
                                posicaoPeDireitoX.ToString() + "," +
                                posicaoPeDireitoY.ToString() + "," +
                                posicaoPeDireitoZ.ToString() + "," +
                                posicaoPeEsquerdoX.ToString() + "," +
                                posicaoPeEsquerdoY.ToString() + "," +
                                posicaoPeEsquerdoZ.ToString() + ",";

            Diretorios_Salvar.AdicionarDados(Diretorios_Salvar.arquivoDados, final_text);
        }
    }

    void CalculoDoAngulo(float vetx1, float vety1, float vetz1, float vetx2, float vety2, float vetz2)
    {

        cos = ((vetx1 * vetx2) + (vety1 * vety2) + (vetz1 * vetz2)) / ((Mathf.Sqrt((vetx1 * vetx1) + (vety1 * vety1) + (vetz1 * vetz1))) * (Mathf.Sqrt((vetx2 * vetx2) + (vety2 * vety2) + (vetz2 * vetz2))));

        if (cos < 0)            //Conversão do cosseno para graus.
        {
            angulo = 57 * Mathf.Acos(cos * -1);
        }
        else
        {
            angulo = 180 - (57 * Mathf.Acos(cos));
        }
    }

}
