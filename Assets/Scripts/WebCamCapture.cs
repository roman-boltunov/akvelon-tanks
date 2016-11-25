using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WebCamCapture : MonoBehaviour 
    {
        [SerializeField]
        private RawImage rawimage;

        public static WebCamTexture WebCamTexture { get; set; }

        [UsedImplicitly]
        private void Start()
        {
            WebCamCapture.WebCamTexture = new WebCamTexture();

            this.rawimage.texture = WebCamCapture.WebCamTexture;
            this.rawimage.material.mainTexture = WebCamCapture.WebCamTexture;
            WebCamCapture.WebCamTexture.Play();
        }
    }
}