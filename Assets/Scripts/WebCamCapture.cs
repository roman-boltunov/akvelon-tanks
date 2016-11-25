using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebCamCapture : MonoBehaviour 
 {
     public RawImage rawimage;
     void Start () 
     {
		 WebCamCapture.WebCamTexture = new WebCamTexture();

         rawimage.texture = WebCamCapture.WebCamTexture;
         rawimage.material.mainTexture = WebCamCapture.WebCamTexture;
         WebCamCapture.WebCamTexture.Play();

     }

	 public static WebCamTexture WebCamTexture {get; set;}
 }