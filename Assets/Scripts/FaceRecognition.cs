using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Assets.Scripts;

public class FaceRecognition : MonoBehaviour {

	public void Recognize(WebCamTexture camera, Action<string> callback) {

		this.StartCoroutine(this._Recognize(camera, callback));
	}

	private  IEnumerator _Recognize(WebCamTexture camera, Action<string> callback) {

		Texture2D snap = new Texture2D(camera.width, camera.height);
     	snap.SetPixels(camera.GetPixels());
		byte[] data = snap.EncodeToPNG();

        WebCamCapture.WebCamTexture.Stop();

        string requestUrl = "https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=True&returnFaceLandmarks=False";

		Dictionary<string, string> headers = new Dictionary<string, string>(); 
		headers.Add("Ocp-Apim-Subscription-Key", "a0c4cd4744844acfa4863ce0dc9ad2c9");
		headers.Add("Content-Type", "application/octet-stream");

		WWW www = new WWW(requestUrl, data, headers);
		yield return www;

		if(!string.IsNullOrEmpty(www.error)) {
			Debug.LogWarning(www.error);
		    callback(null);
			yield break;
		} else {
			string result = www.text;
			Debug.Log(result);
			RecognizedFace[] faces = UserInfo.JsonHelper.getJsonArray<RecognizedFace> (result);
			if (faces.Length > 0) {

				string faceId = faces [0].faceId;
			
				var jsonString = "{'personGroupId':'66549acf-e321-4417-8498-91cc9e0ce819','faceIds':['" + faceId + "'],'maxNumOfCandidatesReturned':1}";
 
				var encoding = new System.Text.UTF8Encoding ();
				var postHeader = new Dictionary<string, string> ();
   
				postHeader.Add ("Content-Type", "application/json");
				postHeader.Add ("Content-Length", jsonString.Length.ToString ());
				postHeader.Add ("Ocp-Apim-Subscription-Key", "a0c4cd4744844acfa4863ce0dc9ad2c9");
 
				var request = new WWW ("https://api.projectoxford.ai/face/v1.0/identify", encoding.GetBytes (jsonString), postHeader);
				yield return request;

				if (!string.IsNullOrEmpty (www.error)) {
					Debug.LogWarning (request.error);
					callback (null);
				} else {
					string json = request.text;
					Debug.Log (json);

					RecognitionResult[] res = UserInfo.JsonHelper.getJsonArray<RecognitionResult> (request.text);

					if (res.Length< 1 || res[0].candidates.Length<0) {
						callback (null);
						yield break;
					}


					String url =
						"https://api.projectoxford.ai/face/v1.0/persongroups/66549acf-e321-4417-8498-91cc9e0ce819/persons/" +
						res [0].candidates [0].personId;

					Dictionary<string, string> headers3 = new Dictionary<string, string> ();
					headers3.Add ("Ocp-Apim-Subscription-Key", "a0c4cd4744844acfa4863ce0dc9ad2c9");

					WWW finalRequest = new WWW (url, null, headers3);
					yield return finalRequest;

					if (!string.IsNullOrEmpty (finalRequest.error)) {
						Debug.LogWarning (finalRequest.error);
						callback (null);
					} else {
						Debug.Log (finalRequest.text);

						ObjectWithName person = JsonUtility.FromJson<ObjectWithName> (finalRequest.text);

						// !!!!!!!!!!!!!!!!!!!
						Debug.Log (person.name);
						callback (person.name);
					}
				}

			} else {
				callback (null);
			}
		}
	}

}

[System.Serializable]
class RecognizedFace {
	public string faceId;
}

[System.Serializable]
class RecognitionResult {
	public RecognitionCandidatate[] candidates;
}

[System.Serializable]
class RecognitionCandidatate {
	public string personId;
}

[System.Serializable]
class ObjectWithName {
	public string name;
}
