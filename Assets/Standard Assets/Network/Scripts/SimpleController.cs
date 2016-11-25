using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace UnityStandardAssets.Network
{
    using JetBrains.Annotations;

    using UnityEditor;

    //A very simple networked controller. Just show how to communicate server <-> client
    [RequireComponent(typeof(NetworkTransform))]
    public class SimpleController : NetworkBehaviour
    {

        public bool verticalMove = true;
        public bool horizontalMove = true;

        float moveX = 0;
        float moveY = 0;
        float moveSpeed = 0.2f;

        void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            // input handling for local player only
            float oldMoveX = moveX;
            float oldMoveY = moveY;

            moveX = 0;
            moveY = 0;


            if (horizontalMove)
            {
                moveX = CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
            }

            if (verticalMove)
            {
                moveY = CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical");
            }

            if (moveX != oldMoveX || moveY != oldMoveY)
            {
                CmdMove(moveX, moveY);
            }
        }

        [Command]
        public void CmdMove(float x, float y)
        {
            Move(x, y);
        }

        public void Move(float x, float y)
        {
            moveX = x;
            moveY = y;
        }

        public void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                transform.Translate(moveX * moveSpeed, moveY * moveSpeed, 0);
            }
        }
    }













    public class NetworkMessages
    {
        /// <summary>
        /// The lobby connected.
        /// </summary>
        public const short LobbyConnected = 0;

        /// <summary>
        /// The attack.
        /// </summary>
        public const short Attack = 1;

        /// <summary>
        /// The defend.
        /// </summary>
        public const short Defend = 2;
    }

    public class DefendMessage : NetworkMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefendMessage"/> class.
        /// </summary>
        /// <param name="playerName">
        /// The player name.
        /// </param>
        /// <param name="playerPicture">
        /// The player picture.
        /// </param>
        public DefendMessage(string playerName, string playerPicture, NetworkConnection connection)
        {
            this.Connection = connection;
            this.PlayerName = playerName;
            this.PlayerPicture = playerPicture;
        }

        /// <summary>
        /// Gets or sets the player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the player picture.
        /// </summary>
        public string PlayerPicture { get; set; }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public NetworkConnection Connection { get; set; }
    }

    public class LobbyConnectedMessage : NetworkMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LobbyConnectedMessage"/> class.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        public LobbyConnectedMessage(NetworkConnection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public NetworkConnection Connection { get; private set; }
    }

    /// <summary>
    /// The my network manager.
    /// </summary>
    public class MyNetworkManager : NetworkManager
    {
        /// <summary>
        /// The start.
        /// </summary>
        [UsedImplicitly]
        private void Start()
        {
            NetworkServer.RegisterHandler(
                NetworkMessages.LobbyConnected,
                msg =>
                {
                    var lobbyMsg = msg as LobbyConnectedMessage;
                    foreach (var networkConnection in NetworkServer.connections)
                    {
                        if (lobbyMsg.Connection.connectionId == networkConnection.connectionId)
                        {
                            // TODO: Mark as lobby
                        }
                    }
                });

            NetworkServer.RegisterHandler(
                NetworkMessages.Defend,
                msg =>
                    {
                        var defendMessage = msg as DefendMessage;
                        foreach (var networkConnection in NetworkServer.connections)
                        {
                            if (defendMessage.Connection.connectionId == networkConnection.connectionId)
                            {
                                defendMessage.Connection.isReady = true;
                                // TODO: Handle name and picture
                            }
                        }
                    });
        }
    }
}
