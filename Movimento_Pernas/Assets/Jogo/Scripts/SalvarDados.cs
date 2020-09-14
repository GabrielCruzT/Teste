using System.IO;
using System;
using UnityEngine;

public class SalvarDados : MonoBehaviour{

    public static string nomeSalvar;
    public static string idadeSalvar;
    public static string generosalvar;
    public static string[] lerTexto;  
    public static bool usuarioInvalido = false;
    public static int numeroUsuariosRegistrado;

    public static void AdicionarUsuario()
    {

		string registroParaGravar = nomeSalvar + "," + idadeSalvar + "," + generosalvar;

        if (nomeSalvar != "" && idadeSalvar != "" && generosalvar != "")
        {
            numeroUsuariosRegistrado = 0;

			//caso exista o arquivo de usuários... leia ele
			if (File.Exists (Menu.arquivoUsuarios))
            {
                lerTexto = File.ReadAllLines (Menu.arquivoUsuarios);
				foreach (string line in lerTexto)
                {
					if (line != "")
                        numeroUsuariosRegistrado++;

					//caso o registro que esteja sendo adicionado já exista, marque como invalido para não inserir
					if (line == nomeSalvar + "," + idadeSalvar + "," + generosalvar)
                        usuarioInvalido = true;
				}
				if (usuarioInvalido == false) 
				{
                    //insere num arquivo novo tudo que ja tinha e mais o novo registro.
                    lerTexto[0] = "Numero de Usuarios " + numeroUsuariosRegistrado.ToString ();
					File.Delete (Menu.arquivoUsuarios);
					using (StreamWriter sw = File.AppendText (Menu.arquivoUsuarios)) 
					{
						foreach (string line in lerTexto) 
						{
							if (line != "")
								sw.WriteLine (line);
						}
						sw.WriteLine (registroParaGravar);
					}
				}
			} 
			else //se o arquivo não existe ele grava
			{
				using (StreamWriter sw = File.AppendText (Menu.arquivoUsuarios)) 
				{
					sw.WriteLine ("Numero de Usuarios 1");
					sw.WriteLine (registroParaGravar);
				}
			}
        }
    }
    public void ApagarUsuario()
    {
        //le o arquivo todo
        lerTexto = File.ReadAllLines(Menu.arquivoUsuarios);
        lerTexto[0] = "Numero de Usuarios " + (lerTexto.Length - 2).ToString();

		//apaga o arquivo
		File.Delete(Menu.arquivoUsuarios);

		//reescreve o arquivo novamente.
		using (StreamWriter sw = File.AppendText(Menu.arquivoUsuarios))
            foreach (string line in lerTexto)
            {
				//ignora a linha selecionada (do usuário para ser apagado)
				if (line == Menu.nomeJogador)
                    continue;
                else
                    sw.WriteLine(line);
            }
        Menu.apagarNomesMenu = true;
    }
}
