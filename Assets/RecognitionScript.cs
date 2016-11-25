using UnityEngine;
using System.Collections;
using Assets.Scripts;
using JetBrains.Annotations;
using UnityEngine.UI;

public class RecognitionScript : MonoBehaviour {

	public Text scanTimeText;

    private float scanTimeLeft;
	private bool isScanned;

    [SerializeField]
	private StandLobby lobbyScript;

	private float showInstructionsTimeLeft;
	private bool showInstructions;
    private FaceRecognition faceRecognition;

    // Use this for initialization
	[UsedImplicitly]
	private void Start()
    {
		scanTimeLeft = 5.0f;
		isScanned = false;
		showInstructions = false;

        this.faceRecognition = this.gameObject.AddComponent<FaceRecognition>();
	}
	
	// Update is called once per frame
	[UsedImplicitly]
	private void Update() {
		scanTimeLeft -= Time.deltaTime;
        
		if (scanTimeLeft < 0 && !isScanned) {
			isScanned = true;

            faceRecognition.Recognize(WebCamCapture.WebCamTexture, name =>
            {
                this.ShowPerson();
            });
		}

		if (isScanned) {
			scanTimeText.text = "0";
		} else {
			scanTimeText.text = "" + Mathf.Floor (scanTimeLeft);
		}

		if (showInstructions) {
			showInstructionsTimeLeft -= Time.deltaTime;
			if (showInstructionsTimeLeft < 0) {
				showInstructions = false;
				lobbyScript.SetActivePanel (lobbyScript.instructionsPanel);
			}
		}
	}

	private void ShowPerson() {
		ShowInstructionsPanel();
	}

	private void ShowInstructionsPanel() {
		showInstructionsTimeLeft = 2.0f;
		showInstructions = true;
	}
}
