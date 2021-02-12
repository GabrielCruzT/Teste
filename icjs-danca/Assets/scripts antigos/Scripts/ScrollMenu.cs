using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScrollMenu : MonoBehaviour {

	public void ScrollSelecao(string[] lerPosicoesTexto, GameObject botao, Text botaoNome)
	{
		for (int i = 0; i < lerPosicoesTexto.Length; i++)
		{
            try
            {
                botaoNome.text = lerPosicoesTexto[i].Split(',')[0] + "," + lerPosicoesTexto[i].Split(',')[1] + "," + lerPosicoesTexto[i].Split(',')[2];
            }
            catch
            {
                botaoNome.text = lerPosicoesTexto[i];
            }
			GameObject positionbutton = Instantiate(botao) as GameObject;
			positionbutton.SetActive(true);
			positionbutton.transform.SetParent(botao.transform.parent, false);
		}
	}
}
