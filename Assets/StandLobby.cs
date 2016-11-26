using JetBrains.Annotations;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StandLobby : NetworkLobbyManager 
{
	public static StandLobby Instance {get;set;}

    public GameObject spinner;

    // Defense mode only
	private string opponentName;

	public SceneObjects sceneObjects; 

	// Use this for initialization
    //http://gamedev.stackexchange.com/questions/102526/why-will-my-server-not-execute-a-command-sent-by-the-client-in-unity-5-1
	[UsedImplicitly]
	private void Start()
    {
		Instance = this;
		this.sceneObjects = GameObject.Find("SceneObjects").gameObject.GetComponent<SceneObjects>();

		SceneManager.sceneLoaded += (scene, loadSceneMode) => {
			Debug.Log("Loaded " + scene.name);
			if (scene.name == "StandLobby") {
				this.sceneObjects = GameObject.Find("SceneObjects").gameObject.GetComponent<SceneObjects>();
				this.SetActivePanel(this.sceneObjects.mainPanel);
				this.SetupButtons();
			}
		};
		
		SetActivePanel(this.sceneObjects.connectPanel);
		this.SetupButtons();

		/*var faceRecognition = connectPanel.AddComponent<FaceRecognition>();

		GameObject.Find("ButtonRecognize").GetComponent<Button>().onClick.AddListener(() =>
		{
			faceRecognition.Recognize(WebCamCapture.WebCamTexture);
		});*/

	}

	private void SetupButtons()
	{
		this.sceneObjects.buttonDefence.GetComponent<Button> ().onClick.AddListener (() => {
			SetActivePanel(this.sceneObjects.recognitionPanel);
		});

		this.sceneObjects.buttonAttack.GetComponent<Button>().onClick.AddListener(() =>
		{
				SetActivePanel(this.sceneObjects.recognitionPanel);
		});

		this.sceneObjects.buttonConnect.GetComponent<Button>().onClick.AddListener(() =>
		{
			SetServerInfo("Connecting...");

			this.scriptCRCCheck = false;

			this.ConnectOurClient();
		});
	}

	private void ConnectOurClient()
	{
		client = new NetworkClient ();
			client.RegisterHandler (MsgType.Connect, (NetworkMessage netMsg) => {
				// ping 
				this.client.Send (MsgType.Highest + 100, new EmptyMsg());
				SetServerInfo("Connected");
				OnConnectedToServer();
			});

			// client.RegisterHandler(MsgType.Rpc, OnClientRpc);
			client.RegisterHandler(MsgType.Error, OnError);

			client.RegisterHandler(MsgType.Highest + 104, (NetworkMessage netMsg) => {

					var attackMsg = netMsg.ReadMessage<LobbyManager.PlayerNameMsg>();
					this.opponentName = attackMsg.name;
					SetActivePanel(this.sceneObjects.defencePanel);
            });

			// start in spectrator mode
			client.RegisterHandler(MsgType.Highest + 111, (NetworkMessage netMsg) => {
					this.ServerChangeScene("CompleteMainScene");
            });

			// start in spectrator mode
			client.RegisterHandler(MsgType.Highest + 120, (NetworkMessage netMsg) => {	 
					SceneManager.LoadScene("StandLobby");
            });

			if (string.IsNullOrEmpty(this.serverIP))
			{
				this.serverIP = this.sceneObjects.inputField.text;
			}		

			Debug.Log("serverIP: " + this.serverIP);

			client.Connect (this.serverIP, 7777);
			//client.Connect ("127.0.0.1", 7777);
	}

	private string serverIP;

	private void StartStopMainPageVideo(bool isPlaying) {
		// set/stop main panel video
		PlayRawImage(this.sceneObjects.mainPanel.GetComponent<RawImage>(), isPlaying);
	}

	private void PlayDefencePageVideo(bool play) {
		PlayRawImage(this.sceneObjects.defencePanel.GetComponent<RawImage> (), play);
	}

	public void PlayRawImage(RawImage rawImage, bool play) {
		if (!rawImage) return;

#if !UNITY_ANDROID
        MovieTexture movie = rawImage.texture as MovieTexture;
		if (!movie) return;

		movie.loop = true;
		if (play) {
			movie.Play();
		} else {
			movie.Stop();
		}
#endif
	}

	public void SetActivePanel(GameObject activePanel) {
		// set/stop main panel video
		if(this.sceneObjects.mainPanel == activePanel) {
			this.sceneObjects.mainPanel.SetActive(true);
			StartStopMainPageVideo(true);
		} else {
			StartStopMainPageVideo(false);
			this.sceneObjects.mainPanel.SetActive(false);
		}
			
		this.sceneObjects.connectPanel.SetActive(this.sceneObjects.connectPanel == activePanel);
		this.sceneObjects.recognitionPanel.SetActive (this.sceneObjects.recognitionPanel == activePanel);

		if (this.sceneObjects.instructionsPanel == activePanel) {
			this.sceneObjects.instructionsPanel.SetActive (true);
			this.sceneObjects.instructionsScript.PlayVideo (true);
		} else {
			this.sceneObjects.instructionsScript.PlayVideo (false);
			this.sceneObjects.instructionsPanel.SetActive (false);
		}

		if (this.sceneObjects.defencePanel == activePanel) {
			this.sceneObjects.defencePanel.SetActive (true);
			PlayDefencePageVideo (true);
		} else {
			PlayDefencePageVideo (false);
			this.sceneObjects.defencePanel.SetActive (false);
		}
	}

	private void OnConnectedToServer() {
		SetActivePanel(this.sceneObjects.mainPanel);

		// GameObject.Find("ButtonStartGame").GetComponent<Button>().onClick.AddListener(() =>
		// {
        //     this.CheckReadyToBegin();
		// 	bool isSent = this.client.Send (MsgType.Highest + 101, new EmptyMsg());
		// 	Debug.Log("StartFame msg sent: " + isSent);
		// });

		// GameObject.Find("ButtonStopGame").GetComponent<Button>().onClick.AddListener(() =>
		// {
		// 	bool isSent = this.client.Send (MsgType.Highest + 102, new EmptyMsg());
		// 	Debug.Log("StartFame msg sent: " + isSent);
		// });
	}

	public void SetServerInfo(string status)
        {
            this.sceneObjects.statusInfo.text = status;
        }

	void OnError(NetworkMessage netMsg)
    {
		Debug.Log("!!!!!!!!!!!!!!OnError");
        //var errorMsg = netMsg.ReadMessage<ErrorMessage>();
        //Debug.Log("Error:" + errorMsg.errorCode);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Scene changed to " + sceneName);
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Server ready");
        base.OnServerReady(conn);
    }

	public void SendAttackEvent() {
//		this.CheckReadyToBegin();

		bool isSent = this.client.Send (MsgType.Highest + 103, new EmptyMsg());
		Debug.Log("StartFame msg sent: " + isSent);
	
	}

	
	public void changeStandPlayerNameOnServer(string playerName) {
		bool isSent = this.client.Send (MsgType.Highest + 112, new LobbyManager.PlayerNameMsg(playerName));
		Debug.Log("StartFame msg sent: " + isSent);
	}

    class EmptyMsg : MessageBase { }
    
	void Update () {
	
	}
}

