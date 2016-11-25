using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class FaceRecognition : MonoBehaviour {

	public void Recognize(WebCamTexture camera) {

		this.StartCoroutine(this._Recognize(camera));
	}

	private  IEnumerator _Recognize(WebCamTexture camera) {

		Texture2D snap = new Texture2D(camera.width, camera.height);
     	snap.SetPixels(camera.GetPixels());
		byte[] data = snap.EncodeToPNG();


		string requestUrl = "https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=True&returnFaceLandmarks=False";

		Dictionary<string, string> headers = new Dictionary<string, string>(); 
		headers.Add("Ocp-Apim-Subscription-Key", "a0c4cd4744844acfa4863ce0dc9ad2c9");
		headers.Add("Content-Type", "application/octet-stream");

		WWW www = new WWW(requestUrl, data, headers);
		yield return www;

		if(!string.IsNullOrEmpty(www.error)) {
			Debug.LogWarning(www.error);
		} else {
			string result = www.text;
			Debug.Log(result);
			RecognizedFace[] faces = UserInfo.JsonHelper.getJsonArray<RecognizedFace> (result);
			if (faces.Length > 0) {

				string faceId = faces[0].faceId;

			}
		}
	}

}

[System.Serializable]
class RecognizedFace {
	public string faceId;
}

// face detection example
//[{"faceId":"6a9c46a5-5709-47eb-94c1-9293417dc117","faceRectangle":{"top":243,"left":363,"width":301,"height":301}}