using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;


public class chamada_camera : MonoBehaviour
{
    public Button button_wait;
    public Button button_ok;
    public Button Libras_wait;
    public Button Libras_ok;

    public RawImage Face; 
    private WebCamTexture webcam;
      
    private Texture requisicao;
    
    private int width;
    private int height;

    public string IP; 

    public Text Titulo;
 
    void Start()
    {
        button_wait.interactable = false;
        button_ok.interactable = false;
        Libras_wait.interactable = false;
        Libras_ok.interactable = false;

        button_ok.gameObject.SetActive (false);
        Libras_ok.gameObject.SetActive (false);

        width = 600;
        height = 800;

        requisicao = Face.texture;

        WebCamDevice[] picture_changer;
        
        picture_changer = WebCamTexture.devices;

        webcam = new WebCamTexture (picture_changer[1].name, width, height);
        
        if (webcam == null) {
            Debug.Log("Deu BO na camera irmão");
            return;
        }

        webcam.Play();
        
        ShowFrame ();
    }
       public void ShowFrame (){
        UnityEngine.Object.Destroy(requisicao);
        requisicao = webcam;
        Face.texture = requisicao;
    }

    public void Read_IP(string newIP){
        IP = newIP;
        Debug.Log(IP);
    }

    public void take_photo(){
        //Aqui tiramos fotos
        Texture2D textura = new Texture2D(Face.texture.width, Face.texture.height, TextureFormat.ARGB32, false);
        textura.SetPixels(webcam.GetPixels());
        byte[] redeBytes = rotateTexture(textura, false).EncodeToPNG();

        StartCoroutine(Upload("http://" + IP +"/whois/", redeBytes));
    }

    IEnumerator Upload(string URL, byte[] redeBytes){
        UnityWebRequest www = UnityWebRequest.Put(URL, redeBytes);

        yield return www.SendWebRequest();
        var data = www.downloadHandler.text;

        if (www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
        }else{
            Debug.Log("Upload Complete!");
            Debug.Log(data);
            //Quebro e pega o que queremos, pos 1
            string Object_split = data.Split(": \"")[1];
            string CPF = Object_split.Split("-")[0];
            //Possivel erro, o Python tem que ter uma aspa faltante
            string NAME = Object_split.Split("-")[1].Split("\"")[0];
            if (CPF != "Desconhecido")
            {
                //Parte do botão
                button_ok.gameObject.SetActive (true);
                Libras_ok.gameObject.SetActive (true);
                Libras_wait.gameObject.SetActive (false);
                button_wait.gameObject.SetActive (false);

                Titulo.text = NAME.ToUpper() + " BOA AULA!";
                StartCoroutine (update_sheet("http://" + IP + "/api_presenca/", CPF));    
            }else {
                Task.Delay(3000).Wait();
                take_photo();
            }  
        } 
    }
    IEnumerator update_sheet (string URL, string CPF){
        UnityWebRequest www = UnityWebRequest.Get(URL + "?cpf=" +  CPF + "&data=" + DateTime.Now.ToString("yyyyMMddHmmss"));

        yield return www.SendWebRequest();
        var data = www.downloadHandler.text;

        if (www.result != UnityWebRequest.Result.Success){
            Debug.Log(www.error);
        }else{
            Debug.Log(data);
            Task.Delay(5000).Wait();
            button_ok.gameObject.SetActive (false);
            Libras_ok.gameObject.SetActive (false);
            Libras_wait.gameObject.SetActive (true);
            button_wait.gameObject.SetActive (true);
            take_photo();

            Titulo.text = "ESTUDANTE DESCONHECIDO";
        } 
    }

    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise){
         Color32[] original = originalTexture.GetPixels32();
         Color32[] rotated = new Color32[original.Length];
         int w = originalTexture.width;
         int h = originalTexture.height;
 
         int iRotated, iOriginal;
 
         for (int j = 0; j < h; ++j)
         {
             for (int i = 0; i < w; ++i)
             {
                 iRotated = (i + 1) * h - j - 1;
                 iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                 rotated[iRotated] = original[iOriginal];
             }
         }
 
         Texture2D rotatedTexture = new Texture2D(h, w);
         rotatedTexture.SetPixels32(rotated);
         rotatedTexture.Apply();
         return rotatedTexture;
     }

    void Update()
    {
        float scaleY = webcam.videoVerticallyMirrored ? -1f: 1f;
        Face.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -webcam.videoRotationAngle;
        Face.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        Face.texture = webcam;

    
    }
}