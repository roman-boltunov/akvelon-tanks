using JetBrains.Annotations;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StandLobby : NetworkLobbyManager {//NetworkLobbyManager {

    [SerializeField]
	private GameObject mainPanel;

    [SerializeField]
    private GameObject connectPanel;

    [SerializeField]
    private GameObject defencePanel;

    [SerializeField]
    private GameObject recognitionPanel;

    [SerializeField]
    public GameObject instructionsPanel;

    [SerializeField]
	private InstructionsScript instructionsScript;

	// Use this for initialization
    //http://gamedev.stackexchange.com/questions/102526/why-will-my-server-not-execute-a-command-sent-by-the-client-in-unity-5-1
	[UsedImplicitly]
	private void Start()
    {
		SetupControls ();
		
		SetActivePanel(connectPanel);

		GameObject.Find("ButtonConnect").GetComponent<Button>().onClick.AddListener(() =>
		{
			SetServerInfo("Connecting...");

			this.scriptCRCCheck = false;

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
					SetActivePanel(defencePanel);

            });

			// string serverIP = GameObject.Find("InputField").GetComponent<InputField>().text;

			// Debug.Log("serverIP: " + serverIP);

			// client.Connect (serverIP, 7777);
			client.Connect ("127.0.0.1", 7777);
		});

		/*var faceRecognition = connectPanel.AddComponent<FaceRecognition>();

		GameObject.Find("ButtonRecognize").GetComponent<Button>().onClick.AddListener(() =>
		{
			faceRecognition.Recognize(WebCamCapture.WebCamTexture);
		});*/

	}

	private void SetupControls() {
		GameObject.Find ("ButtonDefence").GetComponent<Button> ().onClick.AddListener (() => {
			SetActivePanel(recognitionPanel);
		});
	}

	private void StartStopMainPageVideo(bool isPlaying) {
		// set/stop main panel video
		PlayRawImage(mainPanel.GetComponent<RawImage>(), isPlaying);
	}

	private void PlayDefencePageVideo(bool play) {
		PlayRawImage(defencePanel.GetComponent<RawImage> (), play);
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
		if(mainPanel == activePanel) {
			mainPanel.SetActive(true);
			StartStopMainPageVideo(true);
		} else {
			StartStopMainPageVideo(false);
			mainPanel.SetActive(false);
		}
			
		connectPanel.SetActive(connectPanel == activePanel);
		recognitionPanel.SetActive (recognitionPanel == activePanel);

		if (instructionsPanel == activePanel) {
			instructionsPanel.SetActive (true);
			instructionsScript.PlayVideo (true);
		} else {
			instructionsScript.PlayVideo (false);
			instructionsPanel.SetActive (false);
		}

		if (defencePanel == activePanel) {
			defencePanel.SetActive (true);
			PlayDefencePageVideo (true);
		} else {
			PlayDefencePageVideo (false);
			defencePanel.SetActive (false);
		}
	}

	private void OnConnectedToServer() {
		SetActivePanel(mainPanel);

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

		GameObject.Find("ButtonAttack").GetComponent<Button>().onClick.AddListener(() =>
		{
				SetActivePanel(recognitionPanel);
		});
	}
	public Text statusInfo;
	public void SetServerInfo(string status)
        {
            statusInfo.text = status;
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
		bool isSent = this.client.Send (MsgType.Highest + 103, new EmptyMsg());
		Debug.Log("StartFame msg sent: " + isSent);
	}

    class EmptyMsg : MessageBase { }
    // private void OnServerConnect(NetworkMessage netMsg)
    // {
    //    // LobbyManager.s_Singleton.CmdAllReadyButton();
	// //    bool sent = client.Send (MsgType.Highest + 101, new TestMsg());
	//    //netMsg.conn
    // }

    // public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	// {
	// 	// var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
	// 	// NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	// }

// 	public virtual void OnClientConnect(NetworkConnection conn) {

		
// 		//ClientScene.AddPlayer (0);
// 		 ClientScene.Ready(conn);

// 		Debug.Log ("OnClientConnect: " + this.numPlayers);
// 		// conn.playerControllers.ForEach((p) => {
// 		// 	//p.unetView.isClient
// 		// 	Debug.Log ("playerControllerId: " + p.playerControllerId);

// 		// });
// //		conn.playerControllers[0].unetView.isClient
// 	}

// 	public virtual void OnStartClient(NetworkClient client) {
		
// 	}

	// public virtual void OnServerConnect(NetworkConnection conn)
	// {
	// 	Debug.Log ("OnServerConnect");
	// }

	//  public virtual void OnClientSceneChanged(NetworkConnection conn) {

	//  }
	
	// Update is called once per frame
	void Update () {
	
	}
}


// public virtual void OnClientConnect(NetworkConnection conn)
//         {
//             if (string.IsNullOrEmpty(m_OnlineScene) || (m_OnlineScene == m_OfflineScene))
//             {
//                 ClientScene.Ready(conn);
//                 if (m_AutoCreatePlayer)
//                 {
//                     ClientScene.AddPlayer(0);
//                 }
//             }
//             else
//             {
//                 // player will be added when on-line scene finishes loading
//             }
//         }
// Code (CSharp):
 
//         public virtual void OnClientSceneChanged(NetworkConnection conn)
//         {
//             // always become ready.
//             ClientScene.Ready(conn);
 
//             if (!m_AutoCreatePlayer)
//             {
//                 return;
//             }
 
//             bool addPlayer = false;
//             if (ClientScene.localPlayers.Count == 0)
//             {
//                 // no players exist
//                 addPlayer = true;
//             }
 
//             bool foundPlayer = false;
//             foreach (var playerController in ClientScene.localPlayers)
//             {
//                 if (playerController.gameObject != null)
//                 {
//                     foundPlayer = true;
//                     break;
//                 }
//             }
//             if (!foundPlayer)
//             {
//                 // there are players, but their game objects have all been deleted
//                 addPlayer = true;
//             }
//             if (addPlayer)
//             {
//                 ClientScene.AddPlayer(0);
//             }
//         }
