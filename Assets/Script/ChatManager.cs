using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;
    private List<string> chatMessages = new List<string>();
    private List<string> PlayerIdRpc = new List<string>();
    public ChatUI chatUI;

    private void Awake()
    {
        instance = this;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcServiceChatMessage(string playerName, string message)
    {
        string rpcMessage = $"{message}";
        string rpcPlayerId = $"{playerName}";
        chatMessages.Add(rpcMessage);
        PlayerIdRpc.Add(rpcPlayerId);
        chatUI.chatContent.text = rpcMessage;
        chatUI.PlayerIdText.text = rpcPlayerId;
    }

    public void SendChatMessage(string message)
    {
        string playerName = Runner.LocalPlayer.PlayerId.ToString();
        RpcServiceChatMessage(playerName, message);
    }
}
