using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecognitionScript : MonoBehaviour {

	public Text scanTimeText;

	private float scanTimeLeft;
	private bool isScanned;
	private StandLobby lobbyScript;

	private float showInstructionsTimeLeft;
	private bool showInstructions;

	// Use this for initialization
	void Start () {
		scanTimeLeft = 5.0f;
		isScanned = false;
		showInstructions = false;

		GameObject gameObject = GameObject.Find ("GameObject");
		lobbyScript = gameObject.GetComponent<StandLobby> ();
	}
	
	// Update is called once per frame
	void Update () {
		scanTimeLeft -= Time.deltaTime;

		if (scanTimeLeft < 0 && !isScanned) {
			isScanned = true;
			// TODO: scan here.
			showPerson();
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
				lobbyScript.setActivePanel (lobbyScript.instructionsPanel);
			}
		}
	}

	private void showPerson() {
		showInstructionsPanel ();
	}

	private void showInstructionsPanel() {
		showInstructionsTimeLeft = 2.0f;
		showInstructions = true;
	}
}
