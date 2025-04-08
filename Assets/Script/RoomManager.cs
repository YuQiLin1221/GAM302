using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : NetworkBehaviour
{
    public static RoomManager Instance;

    [Networked] public int PlayerCount { get; set; } = 0;
    [Networked] public bool GameStarted { get; set; } = false;
    private Dictionary<PlayerRef, int> playerIDs = new Dictionary<PlayerRef, int>();
    public Button startButton;
    [Networked] public bool isStarted { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (Object.HasStateAuthority)
        {
            RPC_DisablePlayerMovement();
        }
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            PlayerCount++;
            playerIDs[Runner.LocalPlayer] = PlayerCount;
            Debug.Log("Player Count = " + PlayerCount);
            CheckUI();
        }
    }

    void CheckUI()
    {
        int localPlayerID = playerIDs[Runner.LocalPlayer];
        startButton.gameObject.SetActive(localPlayerID == 1);
    }

    public void OnClickStartGame()
    {
        if (Object.HasStateAuthority)
        {
            RPC_StartGame();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartGame()
    {
        GameStarted = true;
        startButton.gameObject.SetActive(false);

        if (Object.HasStateAuthority)
        {
            StartCoroutine(DelayEnableMovement());
        }
    }

    private IEnumerator DelayEnableMovement()
    {
        yield return new WaitForSeconds(3f);
        RPC_EnablePlayerMovement();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_EnablePlayerMovement()
    {
        isStarted = true;
        Debug.Log("Movement Enabled: " + isStarted);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DisablePlayerMovement()
    {
        isStarted = false;
        Debug.Log("Movement Disabled: " + isStarted);
    }
}