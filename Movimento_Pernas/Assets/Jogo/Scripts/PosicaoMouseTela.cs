using UnityEngine;
using System.Collections;

public class PosicaoMouseTela : MonoBehaviour {

    public static Vector3 pontoMundo;
    private Camera cam;                                                             
    Vector3 x, y;
    void Start()
    {
        cam = Camera.main;                                                          
    }
    void OnGUI()                                                                    
    {
        pontoMundo = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        mousePos.x = currentEvent.mousePosition.x;                                              //pega posição x do mouse na tela 
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;                            //pega posição y do mouse na tela e a inverte
        pontoMundo = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 3.6f));         //define um vetor3 com as coordenadas da obtidas e uma distancia definida

    }
}
