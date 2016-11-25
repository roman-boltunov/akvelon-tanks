using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class FaceRecognition : MonoBehaviour {

	private static byte[] Color32ArrayToByteArray(Color32[] colors)
	{
		if (colors == null || colors.Length == 0)
			return null;

		int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
		int length = lengthOfColor32 * colors.Length;
		byte[] bytes = new byte[length];

		GCHandle handle = default(GCHandle);
		try
		{
			handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject();
			Marshal.Copy(ptr, bytes, 0, length);
		}
		finally
		{
			if (handle != default(GCHandle))
				handle.Free();
		}

		return bytes;
	}

	public void Recognize(WebCamTexture camera) {

		this.StartCoroutine(this._Recognize(camera));
	}

	private  IEnumerator _Recognize(WebCamTexture camera) {
		byte[] data = FaceRecognition.Color32ArrayToByteArray(camera.GetPixels32());

		string requestUrl = "https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=True&returnFaceLandmarks=False";

		Dictionary<string, string> headers = new Dictionary<string, string>(); 
		headers.Add("Ocp-Apim-Subscription-Key", "a0c4cd4744844acfa4863ce0dc9ad2c9");

		WWW www = new WWW(requestUrl, data, headers);
		yield return www;

		string result = www.text;


// Add a custom header to the request.
// In this case a basic authentication to access a password protected resource.
// headers["Authorization"] = "Basic " + System.Convert.ToBase64String(
// 	System.Text.Encoding.ASCII.GetBytes("username:password"));

// Post a request to an URL with our custom headers


		// 	HttpResponseMessage response = await _httpClient.SendAsync(request);

			// if (requestBody is Stream) // Buffered Stream
            //     {
            //         request.Content = new StreamContent(requestBody as Stream);
            //         request.Content.Headers.ContentType = new MediaTypeHeaderValue(StreamContentTypeHeader);
            //     }
            //     else
            //     {
            //         request.Content = new StringContent(JsonConvert.SerializeObject(requestBody, s_settings), Encoding.UTF8, JsonContentTypeHeader);
            //     }
	}

	// /// <summary>
	// /// Detects an image asynchronously.
	// /// </summary>
	// /// <param name="imageStream">The image stream.</param>
	// /// <param name="returnFaceId">If set to <c>true</c> [return face ID].</param>
	// /// <param name="returnFaceLandmarks">If set to <c>true</c> [return face landmarks].</param>
	// /// <param name="returnFaceAttributes">Face attributes need to be returned.</param> 
	// /// <returns>The detected faces.</returns>
	// public async Task<Microsoft.ProjectOxford.Face.Contract.Face[]> DetectAsync(Stream imageStream, bool returnFaceId = true, bool returnFaceLandmarks = false, IEnumerable<FaceAttributeType> returnFaceAttributes = null)
	// {
	// 	if (returnFaceAttributes != null)
	// 	{
	// 		var requestUrl = string.Format(
	// 			"{0}/{1}?returnFaceId={2}&returnFaceLandmarks={3}&returnFaceAttributes={4}",
	// 			ServiceHost,
	// 			DetectQuery,
	// 			returnFaceId,
	// 			returnFaceLandmarks,
	// 			GetAttributeString(returnFaceAttributes));

	// 		return await this.SendRequestAsync<Stream, Microsoft.ProjectOxford.Face.Contract.Face[]>(HttpMethod.Post, requestUrl, imageStream);
	// 	}
	// 	else
	// 	{
	// 		var requestUrl = string.Format(
	// 			"{0}/{1}?returnFaceId={2}&returnFaceLandmarks={3}",
	// 			ServiceHost,
	// 			DetectQuery,
	// 			returnFaceId,
	// 			returnFaceLandmarks);

	// 		return await this.SendRequestAsync<Stream, Microsoft.ProjectOxford.Face.Contract.Face[]>(HttpMethod.Post, requestUrl, imageStream);
	// 	}
	// }	
}

// face detection example
//[{"faceId":"6a9c46a5-5709-47eb-94c1-9293417dc117","faceRectangle":{"top":243,"left":363,"width":301,"height":301}}