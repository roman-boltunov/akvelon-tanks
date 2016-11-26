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

    [SerializeField]
    private StandLobby lobby;

	// Use this for initialization
	void Start () {
		unknownCard.gameObject.SetActive (false);
		recognizedCard.gameObject.SetActive (false);
		nameText.gameObject.SetActive (false);
		faceImage.gameObject.SetActive (false);
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

			this.lobby.SendAttackEvent();
		}
	}

	public void PlayVideo(bool play) {
		this.lobby.PlayRawImage(instructionImage, play);
		this.lobby.PlayRawImage(tankImage, play);
	}

	public void setName(string name) {
		if (name != null) {
			recognizedCard.gameObject.SetActive (true);
			var userInfo = this.gameObject.AddComponent<UserInfo> ();
			userInfo.ApplyPersonImage (faceImage, name);
			faceImage.gameObject.SetActive (true);
			nameText.text = name;
			nameText.gameObject.SetActive (true);
		} else {
			unknownCard.gameObject.SetActive (true);
		}
	}
}
