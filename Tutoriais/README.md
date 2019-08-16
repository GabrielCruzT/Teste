# Tutorial para utilizar a camera ORBBEC no unity

[Instalando o driver da camera](#instalando-o-driver-da-camera-no-computador)

[Fazendo download do pacote de assets](#fazendo-download-do-pacote-de-assets)

[Importando o pacote de assets](#importando-o-pacote-de-assets-para-unity)

[Adicionando o Controle da Câmera](#adicionando-o-controle-da-câmera)



## Instalando o driver da camera no computador

Para o reconhecimento da Câmera ORBBEC ASTRA no computador, é necessário instalar drives para o seu funcionamento. Para isso, deve-se acessar o site dos desenvolvedores da ORBBEC [desenvolvedores da ORBBEC](https://orbbec3d.com/develop/) e realizar o download dos drives compatíveis com a câmera e o sistema operacional do computador. **(Figura 1)** 

<p align="center">
<img src="/img/driver_camera.png" width="500">
 <br>
 Figura 1.
</p>





## Fazendo download do pacote de assets

No mesmo site citado anteriormente, está disponível o pacote de assets necessarios para o funcinamento da camera na plataforma Unity. **(Figura 2)** (OBS: Necessário instalar a versao 5.3.6 da [Unity](https://unity3d.com/pt/get-unity/download/archive))

<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62720364-42709c80-b9e0-11e9-91be-ec1d56a658e0.PNG"width="500">
 <br>
 Figura 2. 
</p>
 
 

## Importando o pacote de assets para Unity


Depois de baixado e instalado o drive da câmera, importe do pacote de assets para Unity. **(Figuras 3 a 5)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62721786-5c5fae80-b9e3-11e9-9bd1-ea806db81326.png"width="500">
 <br>
 Figura 3.
</p>
 


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62721812-6ed9e800-b9e3-11e9-8cbb-98693531ee9c.PNG"width="500">
 <br>
 Figura 4.
</p>




<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62721814-71d4d880-b9e3-11e9-8689-94a6a83d18fb.png"width="500">
 <br>
 Figura 5.
</p>
 




## Como obter a imagem da Câmera 

Feito todo o processo de instalação e depois de aberto o unity, crie um novo objeto na aba “Main Camera”. **(Figura 6)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62721972-cd9f6180-b9e3-11e9-8025-08e225fe46fa.png"width="500">
 <br>
 Figura 6.
</p>
 


Na aba do objeto criado, clique na opção “Add Component” e logo após na opção “Mesh”. **(Figura 7)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722048-f1fb3e00-b9e3-11e9-9e59-ab49aaa7ff30.png"width="500">
 <br>
 Figura 7.
</p>
 

Selecione o “Mesh Filter”, em seguida clique no círculo que irá aparecer do lado direito da tela e depois selecione a opção “Quad” para que se crie a caixa onde aparecerá a imagem da câmera como mostram as seguintes.  **(Figuras 8 a 10)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722192-47374f80-b9e4-11e9-9932-93c94a95ba96.png"width="500">
 <br>
 Figura 8.
</p>
 


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722281-7948b180-b9e4-11e9-9d2d-53ed1fb4656c.png"width="500">
 <br>
 Figura 9.
</p>
 

<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722317-8d8cae80-b9e4-11e9-9802-5c1a4d81f466.png"width="500">
 <br>
 Figura 10.
</p>
 


Para renderizar a imagem da câmera adicione outro componente chamado “Mesh Renderer” no mesmo objeto criado anteriormente. **(Figura 11)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722395-b44ae500-b9e4-11e9-963b-a670b757907f.png"width="500">
 <br>
 Figura 11.
</p>
 

Após adicionar o Mesh Renderer, na aba “Cast Shadows” coloque em “Off” juntamente com a aba “Reflection Probes”, desmarque as caixas “Receive Shadows” e “Use Light Probes”, clique na opção “Materials” e depois no círculo na parte direita da tela e selecione a “Unlit Texture” como está apresentado nas seguintes. **(Figuras 12 a 14)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722465-d2184a00-b9e4-11e9-8e17-4ba3d2398bfa.png"width="500">
 <br>
 Figura 12.
</p>
 


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722523-fc6a0780-b9e4-11e9-9b6d-e7c0a6ce3977.png"width="500">
 <br>
 Figura 13.
</p>
                                                                                                                         


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722525-ff64f800-b9e4-11e9-9d22-0d61be3523ea.png"width="500">
 <br>
 Figura 14.
</p>
 


Selecione a pasta scripts dentro do pacote de assets que foi adicionado no início do tutorial e escolha o script “Color Texture Renderer”. **(Figura 15)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722622-43f09380-b9e5-11e9-927e-e9ea947b90da.png"width="500">
 <br>
 Figura 15.
</p>
 


## Adicionando o Controle da Câmera

Crie um novo objeto fora da pasta “Main Camera” e adicione um script chamado “Astra Controller”. **(Figuras 16 e 17)**
 

<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/63188108-f1435700-c036-11e9-9b86-b60238b7d1a6.PNG"width="500">
 <br>
 Figura 16.
</p>
 


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62722977-fc1e3c00-b9e5-11e9-8068-44be60a5d993.PNG"width="500">
 <br>
 Figura 17.
</p>
                                                                                                                         


No objeto criado e com o script selecione a opção “New Color Frame Event” e clique no “+”, em seguida escolha o objeto criado no início do tutorial na aba “Main Camera”. **(Figuras 18 e 19)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62723052-27089000-b9e6-11e9-8b66-2c62a88280cf.png"width="500">
 <br>
 Figura 18.
</p>
 


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62723316-cf1e5900-b9e6-11e9-8362-7bc566052760.png"width="500">
 <br>
 Figura 19.
</p>
 


Clique na aba “No function”, selecione a opção “Color texture renderer” e por fim na opção “On new frame”. **(Figura 20)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62723325-d2b1e000-b9e6-11e9-98c1-fe55cfd6e88e.png"width="500">
 <br>
 Figura 20.
</p>
 


Após, selecione a aba “Runtime Only” e marque a opção “Editor and Runtime”. **(Figura 21)**


<p align="center">
<img src="https://user-images.githubusercontent.com/53840235/62723504-42c06600-b9e7-11e9-873b-b39935c19379.png"width="500">
 <br>
 Figura 21.
</p>
 
 
[Voltar para o inicio](#tutorial-para-utilizar-a-camera-orbbec-no-unity)
