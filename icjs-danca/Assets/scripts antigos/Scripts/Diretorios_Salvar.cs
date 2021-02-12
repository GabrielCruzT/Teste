using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Diretorios_Salvar {

	public static string arquivoUsuarios;
	public static string arquivoOpcoes;
	public static string arquivoMododeJogo;
    public static string arquivoDados;
    public static string arquivoCalibracao;
    public static string PastadeDados;
	public static string PastadeConfiguracoes;
	public static string PastadeModosdeJogo;
    public static string PastadeCalibragemUsuarios;


    public static void CriarDiretorios()
	{
		PastadeDados = Application.persistentDataPath + "\\Dados";
		PastadeConfiguracoes = Application.persistentDataPath + "\\Configuracoes";
		PastadeModosdeJogo = Application.persistentDataPath + "\\ModosdeJogo";

		arquivoUsuarios = PastadeConfiguracoes + "\\Usuarios.txt";
		arquivoOpcoes = PastadeConfiguracoes + "\\Opcoes.txt";
		arquivoMododeJogo = PastadeModosdeJogo + "\\NomedosJogos.txt";

		// Criando pastas iniciais
		if (!Directory.Exists(PastadeDados))
			Directory.CreateDirectory(PastadeDados);

		if (!Directory.Exists(PastadeConfiguracoes))
			Directory.CreateDirectory(PastadeConfiguracoes);

		if(!Directory.Exists(PastadeModosdeJogo))
			Directory.CreateDirectory(PastadeModosdeJogo);

    }

	public static void SalvarDadosTeste(string[] dados, string local)
	{
		if(File.Exists(local))
			File.Delete (local);
		
		using (StreamWriter sw = File.AppendText(local))
		{
			foreach (string line in dados)
			{
				sw.WriteLine(line);
			}
		}
	}

	public static void AdicionarDados(string local, string dadoAdicionado)
	{
		using (StreamWriter sw = File.AppendText (local)) {
			sw.WriteLine (dadoAdicionado);

		}
	}
	public static void ExcluirDados(string[] dados, string local, string dadoExcluido)
	{
		File.Delete (local);
		using (StreamWriter sw = File.AppendText (local)) {
			foreach (string linha in dados) {
				if (linha == dadoExcluido)
					continue;
				else
					sw.WriteLine (linha);
			}
		}
	}

    public static void CriarArquivoDadosUsuario()
    {
        arquivoDados = Diretorios_Salvar.PastadeDados + "\\" + MenuJogar.nomeJogador.Split(',')[0] + "_" + Objeto.opcoesDeJogo[6] + "_" + Objeto.nomeModoJogo + "_" + System.DateTime.Now.ToString("dd-MM-yyyy_HH.mm.ss") + ".txt";
        if (!File.Exists(arquivoDados))
        {
            File.WriteAllText(arquivoDados, "");
            using (StreamWriter sw = File.CreateText(arquivoDados))
                sw.WriteLine("Tempo,Ponto,Numero de aparicoes,Posicao da Bola.x,Posicao da Bola.y,Posicao da Bola.z,Bola Ativada/Desativada,ACD,ACE,AOE,AOD,Cab.x,Cab.y,Cab.z,Nuc.x,Nuc.y,Nuc.z,BasePes.x,BasePes.y,BasePes.z,Tro.x,Tro.y,Tro.z,Ombdir.x,Ombdir.y,Ombdir.z,Ombesq.x,Ombesq.y,Ombesq.z,Cotdir.x,Cotdir.y,Cotdir.z,Cotesq.x,Cotesq.y,Cotesq.z,Puldir.x,Puldir.y,Puldir.z,Pulesq.x,Pulesq.y,Pulesq.z,Maodir.x,Maodir.y,Maodir.z,Maoesq.x,Maoesq.y,Maoesq.z,Qua.x,Qua.y,Qua.z,Quadir.x,Quadir.y,Quadir.z,Quaesq.x,Quaesq.y,Quaesq.z,Joedir.x,Joedir.y,Joedir.z,Joeesq.x,Joeesq.y,Joeesq.z,Pedir.x,Pedir.y,Pedir.z,Peesq.x,Peesq.y,Peesq.z");     
        }
    }

    public static void SalvarDistanciaUsuario(float mDisEsq, float mDisDir)
    {
        string[] lerTexto = File.ReadAllLines(arquivoUsuarios);
        File.Delete(arquivoUsuarios);

        using (StreamWriter sw = File.AppendText(arquivoUsuarios))
        {
            foreach(string line in lerTexto)
            {
                string linhaTexto = line;
                if (linhaTexto.Split(',')[0] + "," + linhaTexto.Split(',')[1] + "," + linhaTexto.Split(',')[2] == MenuJogar.nomeJogador)
                    linhaTexto = MenuJogar.nomeJogador + "," + mDisEsq + "," + mDisDir;

                sw.WriteLine(linhaTexto);

            }
        }
    }

}
