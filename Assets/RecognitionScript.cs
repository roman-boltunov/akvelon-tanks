using UnityEngine;
using System.Collections;
using Assets.Scripts;
using JetBrains.Annotations;
using UnityEngine.UI;

public class RecognitionScript : MonoBehaviour {

	public Text scanTimeText;
	public Image cardImage;
	public Image cardRecognizedImage;
	public Image cardUnknownImage;
	public Image faceImage;
	public Text nameText;

    private float scanTimeLeft;
	private bool isScanned;

	private string recognizedName;

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

		faceImage.gameObject.SetActive (false);
		nameText.gameObject.SetActive (false);

		cardRecognizedImage.gameObject.SetActive (false);
		cardUnknownImage.gameObject.SetActive (false);

        this.faceRecognition = this.gameObject.AddComponent<FaceRecognition>();
	}
	
	// Update is called once per frame
	[UsedImplicitly]
	private void Update() {
		scanTimeLeft -= Time.deltaTime;
        
		if (scanTimeLeft < 0 && !isScanned) {
			isScanned = true;

            // StandLobby.Instance.spinner.SetActive(true);
            faceRecognition.Recognize(WebCamCapture.WebCamTexture, name =>
            {
                // StandLobby.Instance.spinner.SetActive(false);
                this.ShowPerson(name);
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
				StandLobby.Instance.SetActivePanel (StandLobby.Instance.sceneObjects.instructionsPanel);
				StandLobby.Instance.sceneObjects.instructionsPanel.GetComponent<InstructionsScript> ().setName (recognizedName);
			}
		}
	}

	private void ShowPerson(string name) {
		cardImage.gameObject.SetActive (false);
		if (name != null) {
			cardRecognizedImage.gameObject.SetActive (true);
			var userInfo = this.gameObject.AddComponent<UserInfo> ();
			userInfo.ApplyPersonImage (faceImage, name);
			faceImage.gameObject.SetActive (true);
			nameText.text = name;
			nameText.gameObject.SetActive (true);
			
			 StandLobby.Instance.changeStandPlayerNameOnServer(name);

		} else {
			cardUnknownImage.gameObject.SetActive (true);
		}

		recognizedName = name;
		ShowInstructionsPanel();
	}

	private void ShowInstructionsPanel() {
		showInstructionsTimeLeft = 5.0f;
		showInstructions = true;
	}
}
