# Tutorial para utilizar a câmera ORBBEC no unity

[Instalando o driver da Câmera](#instalando-o-driver-da-câmera-no-computador)

[Fazendo download do pacote de assets](#fazendo-download-do-pacote-de-assets)

[Importando o pacote de assets](#importando-o-pacote-de-assets-para-unity)

[Adicionando o Controle da Câmera](#adicionando-o-controle-da-câmera)

[Como obter a imagem RAW da Câmera](#como-obter-a-imagem-raw-da-câmera) 




## Instalando o driver da câmera no computador

Para o reconhecimento da Câmera ORBBEC ASTRA no computador, é necessário instalar drives para o seu funcionamento. Para isso, deve-se acessar o site dos [desenvolvedores da ORBBEC](https://orbbec3d.com/develop/) e realizar o download dos drives compatíveis com a câmera e o sistema operacional do computador, conforme mostrado na Figura 1. 

<p align="center">
<img src="img/driver_camera.png" width="500">
 <br>
 Figura 1. Download dos drivers na pagina do desenvolvedor da Orbecc.
</p>





## Fazendo download do pacote de assets

No mesmo site citado anteriormente, está disponível o pacote de assets necessarios para o funcinamento da camera na plataforma Unity, de acordo com a Figura 2. (OBS: Necessário instalar a versao 5.3.6 da [Unity](https://unity3d.com/pt/get-unity/download/archive))

<p align="center">
<img src="img/assets_unity.png"width="500">
 <br>
 Figura 2. 
</p>
 
 

## Importando o pacote de assets para Unity


Depois de baixado e instalado o driver da câmera, importe do pacote de assets para Unity, como mostram as figuras de 3 a 5.


<p align="center">
<img src="img/importar_assets1.png"width="500">
 <br>
 Figura 3.
</p>
 


<p align="center">
<img src="img/importar_assets2.png"width="500">
 <br>
 Figura 4.
</p>




<p align="center">
<img src="img/importar_assets3.png"width="500">
 <br>
 Figura 5.
</p>
 
 



## Adicionando o Controlador da Câmera

O primeiro passo é adicionar o Controlador da Câmera. Isso é feito adicionando o script **"Astra Controller"** à um novo objeto. Para isso, crie um novo objeto clicando com o botão direito do mouse na **Janela de Hierarquia** e escolhendo a opção *"Create Empty"*. Uma vez criado o objeto, selecione o script, na pasta de scripts do pacote de assets, chamado **“Astra Controller”** e arraste para o objeto criado, consoante com as Figuras 6 e 7. O script também pode ser adicionado por meio do botão *"Add Component"* do objeto recém criado.
 

<p align="center">
<img src="img/criando_objeto_para_controller.png"width="500">
 <br>
 Figura 6. Adicionando um novo objeto vazio.
</p>
 


<p align="center">
<img src="img/adicionando_script_astracontroler.png"width="500">
 <br>
 Figura 7. Selecionando o script <b>"Astra Controller"</b> para ser arrastado ao novo objeto recém criado.
</p>
                                                                                                                         


No objeto criado e com o script selecione a opção “New Color Frame Event” e clique no “+”, em seguida escolha o objeto criado no início do tutorial na aba “Main Camera”, tal qual as figuras 18 e 19.


<p align="center">
<img src="img/configurando_newcolorframe1.png"width="500">
 <br>
 Figura 18.
</p>
 


<p align="center">
<img src="img/configurando_newcolorframe2.png"width="500">
 <br>
 Figura 19.
</p>
 


Clique na aba “No function”, selecione a opção “Color texture renderer” e em seguida a opção “On new frame”, segundo a figura 20.


<p align="center">
<img src="img/configurando_newcolorframe3.png"width="500">
 <br>
 Figura 20.
</p>
 


Após, selecione a aba “Runtime Only” e marque a opção “Editor and Runtime”, como apresenta a figura 21.


<p align="center">
<img src="img/configurando_newcolorframe4.png"width="500">
 <br>
 Figura 21.
</p>## Como obter a imagem RAW da Câmera 

Feito todo o processo de instalação e depois de aberto o unity, crie um novo objeto na aba “Main Camera”, conforme a figura 6.


<p align="center">
<img src="img/criando_objeto_maincamera.png"width="500">
 <br>
 Figura 6.
</p>
 


Na aba do objeto criado, clique na opção “Add Component” e logo após na opção “Mesh”,segundo a figura 7.


<p align="center">
<img src="img/adicionando_componente_meshfilter1.png"width="500">
 <br>
 Figura 7.
</p>
 

Selecione o “Mesh Filter”, em seguida clique no círculo que irá aparecer do lado direito da tela e depois selecione a opção “Quad” para que se crie a caixa onde aparecerá a imagem da câmera como mostram as seguintes, como apresentado nas figuras de 8 a 10.


<p align="center">
<img src="img/adicionando_componente_meshfilter2.png"width="500">
 <br>
 Figura 8.
</p>
 


<p align="center">
<img src="img/selecionar_tipo_mesh1.png"width="500">
 <br>
 Figura 9.
</p>
 

<p align="center">
<img src="img/selecionar_tipo_mesh2.png"width="500">
 <br>
 Figura 10.
</p>
 


Para renderizar a imagem da câmera adicione outro componente chamado “Mesh Renderer” no mesmo objeto criado anteriormente, conforme mostra a Figura 11.


<p align="center">
<img src="img/adicionando_componente_meshrenderer.png"width="500">
 <br>
 Figura 11.
</p>
 

Após adicionar o Mesh Renderer, na aba “Cast Shadows” coloque em “Off” juntamente com a aba “Reflection Probes”, desmarque as caixas “Receive Shadows” e “Use Light Probes”, clique na opção “Materials” e depois no círculo na parte direita da tela e selecione a “Unlit Texture” como está apresentado nas seguintes, de acordo com as figuras de 12 a 14.


<p align="center">
<img src="img/configurando_meshrenderer1.png"width="500">
 <br>
 Figura 12.
</p>
 


<p align="center">
<img src="img/configurando_meshrenderer2.png"width="500">
 <br>
 Figura 13.
</p>
                                                                                                                         


<p align="center">
<img src="img/configurando_meshrenderer3.png"width="500">
 <br>
 Figura 14.
</p>
 


Selecione a pasta scripts dentro do pacote de assets que foi adicionado no início do tutorial e escolha o script “Color Texture Renderer”, em conformidade com a figura 15.


<p align="center">
<img src="img/adicionando_script_colortexturerenderer.png"width="500">
 <br>
 Figura 15.
</p>



[Voltar para o inicio](#tutorial-para-utilizar-a-camera-orbbec-no-unity)
