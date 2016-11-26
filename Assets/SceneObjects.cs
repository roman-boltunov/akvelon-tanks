using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneObjects : MonoBehaviour 
{
	public SceneObjects Instance { get; private set; }

	private SceneObjects()
	{

	}

	private void Start()
	{
		Instance = new SceneObjects();
	}

    public GameObject spinner;

    public Text statusInfo;

	[SerializeField]
	public GameObject mainPanel;

    [SerializeField]
    public GameObject connectPanel;

    [SerializeField]
    public GameObject defencePanel;

    [SerializeField]
    public GameObject recognitionPanel;

    [SerializeField]
    public GameObject instructionsPanel;

    [SerializeField]
	public InstructionsScript instructionsScript;

	[SerializeField]
	public GameObject buttonDefence;

	[SerializeField]
	public GameObject buttonAttack;

	[SerializeField]
	public GameObject buttonConnect;

	[SerializeField]
	public InputField inputField;
}
