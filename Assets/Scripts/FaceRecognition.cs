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
			
				var jsonString = "{'personGroupId':'66549acf-e321-4417-8498-91cc9e0ce819','faceIds':['"+faceId+"'],'maxNumOfCandidatesReturned':1}";
 
				var encoding = new System.Text.UTF8Encoding();
				var postHeader = new Dictionary<string, string>();
   
				postHeader.Add("Content-Type", "application/json");
				postHeader.Add("Content-Length", jsonString.Length.ToString());
				postHeader.Add("Ocp-Apim-Subscription-Key", "a0c4cd4744844acfa4863ce0dc9ad2c9");
 
				var request = new WWW("https://api.projectoxford.ai/face/v1.0/identify", encoding.GetBytes(jsonString), postHeader);
				yield return request;

				string err = request.error;
				

				 //personGroupId = "66549acf-e321-4417-8498-91cc9e0ce819",
					// faceIds = [faceId],
                    // maxNumOfCandidatesReturned = 1

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


//"[{\"faceId\":\"f97538d4-b941-4b7b-a444-be082b94eef6\",\"candidates\":[{\"personId\":\"2ed7bb0b-0d26-4617-88b4…"