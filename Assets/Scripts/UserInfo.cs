using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInfo: MonoBehaviour  {

	private IEnumerator ApplyPersonCoroutine(Image ava, string name)
	{
		WWW www = new WWW("http://prism.akvelon.net/api/system/getphoto/36");
		yield return www;
        ava.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}

	public void  ApplyPersonImage(Image ava, string name) {
		this.StartCoroutine(this.ApplyPersonCoroutine(ava, name));
	}

}
