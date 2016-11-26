using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DefenceScript : MonoBehaviour {

	public Text attackedText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setAttackerName(string attackedBy) {
		attackedText.text = attackedBy + " is attacking you!";
	}
		
}
