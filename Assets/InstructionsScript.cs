using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InstructionsScript : MonoBehaviour {

	private float timeLeft = 15.0f;
	private bool gameStarted = false;

	public Text timeLeftLabel;

	public RawImage instructionImage;
	public RawImage tankImage;
	public Image unknownCard;
	public Image recognizedCard;
	public Text nameText;
	public Image faceImage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		if (gameStarted) {
			timeLeftLabel.text = "0";
		} else {
			timeLeftLabel.text = "" + Mathf.Floor (timeLeft);
		}


		if (timeLeft < 0 && !gameStarted) {
			gameStarted = true;

			StandLobby.Instance.SendAttackEvent();
		}
	}

	public void PlayVideo(bool play) {
		StandLobby.Instance.PlayRawImage(instructionImage, play);
		StandLobby.Instance.PlayRawImage(tankImage, play);
	}

	public void setName(string name) {
		if (name != null) {
			unknownCard.gameObject.SetActive (false);
			nameText.text = name;
			var userInfo = this.gameObject.AddComponent<UserInfo> ();
			userInfo.ApplyPersonImage (faceImage, name);
		} else {
			recognizedCard.gameObject.SetActive (false);
			faceImage.gameObject.SetActive (false);
			nameText.gameObject.SetActive (false);
		}
	}
}
