# Tutorial para utilizar a câmera ORBBEC no unity

[Instalando o driver da câmera](#instalando-o-driver-da-câmera-no-computador)

[Fazendo download do pacote de assets](#fazendo-download-do-pacote-de-assets)

[Importando o pacote de assets](#importando-o-pacote-de-assets-para-unity)

[Adicionando o Controlador da câmera](#adicionando-o-controlador-da-câmera)

[Como obter a imagem RAW da câmera](#como-obter-a-imagem-raw-da-câmera)

[Como obter a visualização do mapa de profundidade da imagem](#como-obter-a-visualização-do-mapa-de-profundidade-da-imagem)




## Instalando o driver da câmera no computador

Para o reconhecimento e funcionamento da Câmera ORBBEC ASTRA no computador, é necessário instalar drives da câmera. Para isso, deve-se acessar o site dos [desenvolvedores da ORBBEC](https://orbbec3d.com/develop/) e realizar o download dos drives compatíveis com a câmera e o sistema operacional do computador, conforme mostrado na Figura 1.

<p align="center">
<img src="img/driver_camera.png" width="500">
 <br>
 Figura 1. Download dos drivers na pagina do desenvolvedor da Orbecc.
</p>



## Fazendo download do pacote de assets

No mesmo site citado anteriormente, está disponível o pacote de assets necessarios para o funcinamento da câmera na plataforma Unity,de acordo com a Figura 2. (OBS: Necessário instalar a versao 5.3.6 da [Unity](https://unity3d.com/pt/get-unity/download/archive))

<p align="center">
<img src="img/assets_unity.png"width="500">
 <br>
 Figura 2. 
</p>
 
 

## Importando o pacote de assets para Unity


Depois de baixado e instalado o driver da câmera, importe do pacote de assets para Unity, como mostram as Figuras de 3 a 5.


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
<img src="img/Import_asset3.png"width="500">
 <br>
 Figura 5.
</p>
 
 



## Adicionando o Controlador da Câmera

O primeiro passo é adicionar o Controlador da Câmera. Isso é feito adicionando o script **"Astra Controller"** à um novo objeto. Para isso, crie um novo objeto clicando com o botão direito do mouse na **Janela de Hierarquia** e escolhendo a opção *"Create Empty"*. Uma vez criado o objeto, renomeie o objeto para o nome desejado e selecione o script, na pasta de scripts do pacote de assets, chamado **“Astra Controller”** e arraste para o objeto criado, consoante com as Figuras 6 a 8. \*O script também pode ser adicionado por meio do botão *"Add Component"* do objeto recém criado.
 

<p align="center">
<img src="img/criando_objeto_para_controller.png"width="500">
 <br>
 Figura 6. Adicionando um novo objeto vazio.
</p>
 


<p align="center">
<img src="img/adicionando_script_astracontroler.png"width="500">
 <br>
 Figura 7. Selecionando o script <b>"Astra Controller"</b> para ser arrastado ao novo objeto recém criado com o nome desejado.
</p>


<p align="center">
<img src="img/imagem_do_script_de_controle_da_camera.png"width="500">
 <br>
 Figura 8.
</p>
                                                                                                                         
- Após adicionar o *script* **"Astra Controller"**, são apresentadas 7 opções de controles distintos como mostra a Figura 8:
- Depth Frame: É utilizado para visualização do mapa de profundidade da imagem;
- Color Frame: É utilizado para a visualização da imagem colorida (Raw) da câmera;
- NV21 Color Frame: É utilizado para a visualização da imagem colorida da câmera, no formato NV21 (Só pode ser usado quando se utilza a câmera na plataforma Android);
- Body Frame **\***: É utilizado para visualizar o esqueleto criado a partir da imagem da câmera;
- Masked Color Frame **\***: É utilizado para visualizar a máscara do objeto;
- Colorized Body Frame **\***: É utilizado para visualizar a máscara colorida do objeto ;
- Body Mask **\***: É utilizado para visualizar o teste de colisão do personagem;

**\*** *Estes modos de visualização só funcionam a partir do momento que a câmera detecta um corpo na imagem.*

## Como obter a imagem RAW da Câmera

A imagem RAW é obtida configurando o color frame do controlador, e adicionando um objeto que será a imagem da camera na tela.

Crie um novo objeto na aba **“Main Camera”** da Janela de Hierarquia e o renomeie. Tal objeto, mostrará a imagem da câmera na tela, conforme a Figura 9.


<p align="center">
<img src="img/criando_objeto_maincamera.png"width="500">
 <br>
 Figura 9.
</p>
 


Selecione o objeto criado, clique na opção **“Add Component”** e logo após na opção **“Mesh ... Mesh Filter”**,segundo a Figura 10.

Descobrir na net pra que serve esse mesh no caso o mesh filter para falar pq vc ta fazendo isso?

<p align="center">
<img src="img/adicionando_componente_meshfilter1.png"width="500">
 <br>
 Figura 10.
</p>
 

Selecione o componente **“Mesh Filter”**, em seguida clique no círculo que irá aparecer do lado direito da tela e depois selecione a opção **“Quad”** para que se crie a caixa onde aparecerá a imagem da câmera como mostram as seguintes, como apresentado nas figuras de 11 a 13.


<p align="center">
<img src="img/adicionando_componente_meshfilter2.png"width="500">
 <br>
 Figura 11.
</p>
 


<p align="center">
<img src="img/selecionar_tipo_mesh1.png"width="500">
 <br>
 Figura 12.
</p>
 

<p align="center">
<img src="img/selecionar_tipo_mesh2.png"width="500">
 <br>
 Figura 13.
</p>
 


Para acrescentar textura no objeto que irá transmitir a imagem da câmera adicione outro componente chamado **“Mesh Renderer”** no mesmo objeto, conforme mostra a Figura 14.

Explicar pq vc ta escolhendo essa textura? 

<p align="center">
<img src="img/adicionando_componente_meshrenderer.png"width="500">
 <br>
 Figura 14.
</p>
 
E porque dessas configurações abaixo aí

Após adicionar o **Mesh Renderer**, na aba **“Cast Shadows”** coloque em **“Off”** juntamente com a aba **“Reflection Probes”**, desmarque as caixas **“Receive Shadows”** e **“Use Light Probes”**, clique na opção **“Materials”** para escolher o tipo de textura a ser usada e depois no círculo na parte direita da tela e selecione a **“Unlit Texture”**, conforme as figuras de 15 a 17.


<p align="center">
<img src="img/configurando_meshrenderer1.png"width="500">
 <br>
 Figura 15.
</p>
 


<p align="center">
<img src="img/configurando_meshrenderer2.png"width="500">
 <br>
 Figura 16.
</p>
                                                                                                                         


<p align="center">
<img src="img/configurando_meshrenderer3.png"width="500">
 <br>
 Figura 17.
</p>


Depois de criar e configurar o objeto, é necessário reposicioná-lo na frente da **Main Camera**, para que ela possa "pegar" a imagem a ser transmitida. Para isso selecione a opção **"Scene"**(Seta preta) para no modo de configuração da cena, logo após selecione a opção de reposicionamento (Seta Azul) e escolha o lugar desejdo para mostrar a imagem da camera utilizando os vetores (Seta Amarela), de acordo com a Figura 18.


<p align="center">
<img src="img/imagem_do_script_de_controle_da_camera.png"width="500">
 <br>
 Figura 18.
</p>
 


Selecione a pasta **scripts** dentro do pacote de **assets** que foi adicionado no [início do tutorial](#tutorial-para-utilizar-a-câmera-orbbec-no-unity), escolha o script **“Color Texture Renderer”** e o adicione no objeto que fornece a imagem da câmera, em conformidade com a Figura 19.


<p align="center">
<img src="img/adicionando_script_colortexturerenderer.png"width="500">
 <br>
 Figura 19.
</p>


Após esse processo, selecione o objeto de controle da câmera, criado no [início do tutorial](#tutorial-para-utilizar-a-câmera-orbbec-no-unity), com o nome desejado, selecione a opção **“New Color Frame Event”** e clique no **“+”**. Em seguida, escolha o objeto criado anteriormente na aba **“Main Camera”**, tal qual as Figuras 20 e 21.


<p align="center">
<img src="img/configurando_newcolorframe1.png"width="500">
 <br>
 Figura 20.
</p>
 


<p align="center">
<img src="img/configurando_newcolorframe2.png"width="500">
 <br>
 Figura 21.
</p>
 


Escolha a aba **“No function”**, selecione a opção **“Color texture renderer”** e em seguida a opção **“On new frame”**, segundo a Figura 22.

Porque???

<p align="center">
<img src="img/configurando_newcolorframe3.png"width="500">
 <br>
 Figura 22.
</p>
 


Após, selecione a aba **“Runtime Only”** e marque a opção **“Editor and Runtime”**, como apresenta a Figura 23.


<p align="center">
<img src="img/configurando_newcolorframe4.png"width="500">
 <br>
 Figura 23.
</p>

Feito isso está configurada a imagem raw da camera. Uma vez que dê o play na cena, o programa funcionará e mostrará a imagem:

Foto do funcionamento aqui.



## Como obter a visualização do mapa de profundidade da imagem

Para obter o mapa de profundidade da câmera, primeiramente é necessário criar um novo objeto vazio dentro da aba **"Main Camera"** na **Janela de hierarquia**, como mostra a Figura 24. 
\*O objeto criado também deverá ser reposicionado na frente da "Main Camera" como foi realizado na obtenção da imagem da câmera.


<p align="center">
<img src="img/adicionando_objeto_de_profundidade.png"width="500">
 <br>
 Figura 24.
</p>



Em seguida, adicione os componentes **"Mesh Filter e Mesh Renderer"** e os configure como foi ensinado anteriormente no tópico [Como obter a imagem RAW da Câmera](#como-obter-a-imagem-raw-da-câmera).

Depois de concluir os passos acima, adicione o script **"Depth Texture Renderer"** no mesmo objeto criado anteriormente de acordo com a Figura 25.


<p align="center">
<img src="img/adicionando_script_depht_texture_renderer.png"width="500">
 <br>
 Figura 25.
</p>


Selecione o objeto de **"Controle da Camera"** para configurar o objeto que irá transmitir o mapa de profundidade, em seguida clique no **"+"** logo abaixo da opção **"New Depth Frame Event"** para adicionar o objeto desejado como é observado na Figura 26.


<p align="center">
<img src="img/adicionando_componente_camera_de_profundidade_no_controle_da_camera.png"width="500">
 <br>
 Figura 26.
</p>


Posteriormente, escolha o objeto que irá transmitir o mapa de profundidade, e o configure do mesmo modo que foi feito nos passos de [Como obter a imagem RAW da Câmera](#como-obter-a-imagem-raw-da-câmera) de acordo com a Figuras de 27 a 30. 


<p align="center">
<img src="img/configurando_controle_da_camera_para_profundidade.png"width="500">
 <br>
 Figura 27.
</p>


<p align="center">
<img src="img/configurando_controle_da_camera_para_profundidade1.png"width="500">
 <br>
 Figura 28.
</p>


<p align="center">
<img src="img/configurando_controle_da_camera_para_profundidade2.png"width="500">
 <br>
 Figura 29.
</p>


<p align="center">
<img src="img/configurando_controle_da_camera_para_profundidade3.png"width="500">
 <br>
 Figura 30.
</p>




[Voltar para o inicio](#tutorial-para-utilizar-a-câmera-orbbec-no-unity)
