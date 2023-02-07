using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeapGame
{
    public class Room : NetworkRoomManager
    {
        private IList<Player> addedPlayers = new SyncList<Player>();
        private bool showStartButton = true;

        public override void OnGUI()
        {
            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                if (addedPlayers.Count() != maxConnections)
                    return;

                showStartButton = false;

                addedPlayers[0].RpcOnMatchStart(addedPlayers[0], addedPlayers[1]);
                addedPlayers[1].RpcOnMatchStart(addedPlayers[1], addedPlayers[0]);
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            clientIndex++;

            if (Utils.IsSceneActive(RoomScene))
            {
                allPlayersReady = false;

                GameObject gamePlayer;
                Transform startPos = GetStartPosition();
                gamePlayer = startPos != null
                    ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                    : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

                gamePlayer.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

                Player player = gamePlayer.GetComponent<Player>();
                addedPlayers.Add(player);

                NetworkServer.AddPlayerForConnection(conn, gamePlayer);
            }
            else
            {
                Debug.Log($"Not in Room scene...disconnecting {conn}");
                conn.Disconnect();
            }
        }

        public void RespawnPlayers()
        {
            foreach (Transform startPosition in startPositions)
            {
                startPosition.gameObject.SetActive(true);
            }

            foreach (Player player in addedPlayers)
            {
                Transform startPosition = GetStartPosition();
                player.Respawn(startPosition.position, startPosition.rotation);
            }
        }

        public override Transform GetStartPosition()
        {
            startPositions.RemoveAll(t => t == null);

            List<Transform> activeStartPositions = startPositions
                .Where(x => x.gameObject.activeSelf)
                .ToList();

            if (activeStartPositions.Count() == 0)
                return null;

            Transform startPosition = activeStartPositions[Random.Range(0, activeStartPositions.Count())];
            startPosition.gameObject.SetActive(false);

            return startPosition;
        }

        public override void OnStartClient()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("NetworkRoomManager no GamePlayer prefab is registered. Please add a GamePlayer prefab.");
            }

            OnRoomStartClient();
        }

        public void DisableRoomGUI()
        {
            showRoomGUI = false;
        }

        public override void OnRoomServerPlayersReady()
        {
#if UNITY_SERVER
        base.OnRoomServerPlayersReady();
#else
            showStartButton = true;
#endif
        }

        public override void Awake()
        {
            base.Awake();

            instance = this;

            CursorStateChanger.Unlock();
            CursorStateChanger.Show();
        }

        private static Room instance;
        public static Room Instance
        {
            get
            {
                return instance;
            }
        }
    }
}