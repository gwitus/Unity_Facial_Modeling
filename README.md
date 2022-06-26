# Unity_2D_Facial.Core

   O presente repositório contém um aplicativo mobile modelado em Unity (APK disponibilizado), cuja incentivo é realizar a chamada de uma sala de aula utilizando reconhecimento Facial.
    
    - A primeira função é pegar uma imagem picture através do Raw Image no Unity, cada frame que é capturado e se não for utilizado é destruido em uma função C#;
    - A segunda função é pegar essa imagem e enviar para um serviço feito em Python, que utilizando OPENCV irá realizar o reconhecimento facial de qualquer rosto que se oponha a camera;
    - A terceira função é caso o rosto seja confirmado, enviar essa imagem para outro serviço python feito em Flask via requisição WEB que ao associar o rosto a alguém prencha a data e hora em uma planilha online (Google Sheets) com a presença do estudante;

#Algumas observações:
  
  - Foi utilizado POO;
  - Todos as funções foram feitas utilizando as bibliotecas do Unity, por exemplo, ao invés de usar um HttpClient foi utilizado o UnityWebRequest;
  - O app foi modelado em Unity, utilizando a switch 2D core;

  - O Apk disponibilizado pode conter erros de responsividade e compatibilidade, e os mesmos sofrerão update em versionamentos futuros;
