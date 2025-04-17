using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class NetworkRunnerHanlder : NetworkBehaviour
{
    [Obsolete]
    private void Start()
    {
        // Check if we are in shared mode
        if (PlayerPrefs.GetInt("IsSharedMode", 0) == 1)
        {
            string roomId = PlayerPrefs.GetString("RoomID", string.Empty);
            string roomMode = PlayerPrefs.GetString("RoomMode", string.Empty);

            // Start the shared client
            var fusionBootstrap = FindObjectOfType<FusionBootstrap>();
            if (fusionBootstrap != null)
            {
                if (roomMode == "Create")
                {
                    //fusionBootstrap.StartSharedClient();
                    fusionBootstrap.StartSinglePlayer();
                }
                else if (roomMode == "Join")
                {
                    fusionBootstrap.StartSharedClient();
                }
            }
        }
    }
}