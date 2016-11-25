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

    [SerializeField]
    private StandLobby lobby;

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

			this.lobby.SendAttackEvent();
		}
	}

	public void PlayVideo(bool play) {
		this.lobby.PlayRawImage(instructionImage, play);
		this.lobby.PlayRawImage(tankImage, play);
	}
}
