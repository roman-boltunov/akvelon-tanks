using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class UserInfo: MonoBehaviour  {

	public class JsonHelper
	{
		public static T[] getJsonArray<T>(string json)
		{
			string newJson = "{ \"array\": " + json + "}";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
			return wrapper.array;
		}
	
		[System.Serializable]
		private class Wrapper<T>
		{
			public T[] array;
		}
	}

	private IEnumerator ApplyPersonCoroutine(Image ava, string name)
	{
		WWW wwwAll = new WWW("http://prism.akvelon.net/api/employees/all");
		yield return wwwAll;

		string id = FindPersonInJon(wwwAll.text, name);

		if (id != null) {
			WWW www = new WWW("http://prism.akvelon.net/api/system/getphoto/" + id);
			yield return www;
        	ava.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));	
		}		
	}

	public void  ApplyPersonImage(Image ava, string name) {
		this.StartCoroutine(this.ApplyPersonCoroutine(ava, name));
	}

	private string FindPersonInJon(string jsonString, string name)
     {
        
		PrismPerson[] objects = JsonHelper.getJsonArray<PrismPerson> (jsonString);

		name = name.ToLower();

		foreach(PrismPerson obj in objects) {
			if (string.Equals(name, (obj.FirstName + " " + obj.LastName).ToLower()) || 
				string.Equals(name, (obj.LastName + " " + obj.FirstName).ToLower()) || 
				string.Equals(name, obj.Login)) {
				
				return obj.Id.ToString();
			}
		}
		
		return null;
 
     }

	[System.Serializable]
	 class PrismPerson {
		 public int Id;
		public string LastName;
		public string FirstName;

		public string Login;
	 }

}

// {"ActiveDirectorySynchronizationTimestamp":"2016-11-24T06:34:30",
// "Dislocation":"2nd floor: Mobile + Axapta room",
// "FirstName":"Сергей",
// "Id":36,"InBuilding":true,
// "LastName":"Гребнов",
// "Login":"sg","Mail":"sergey.grebnov@akvelon.com","Notes":"","NotificationCheck":false,"IsNeedTurnOn":false,"MacAddress":"","SecurityKey":true,"Skype":"SGrebnov","Telephone":"+79206722463","CarLicensePlate":"н027ео"}
