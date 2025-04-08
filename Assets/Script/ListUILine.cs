using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;

public class ListUILine : MonoBehaviour
{
    public TextMeshProUGUI NameRoom;
    public TextMeshProUGUI NumberOfPlayer;
    public Button JoinBtn;
    
    SessionInfo SessionInfo;

    //event
    public event Action<SessionInfo> OnJoinSession;

    public void SetInformation(SessionInfo sessionInfo)
    {
        this.SessionInfo = sessionInfo;
        NameRoom.text = sessionInfo.Name;
        NumberOfPlayer.text = $"{sessionInfo.PlayerCount.ToString()}/{SessionInfo.MaxPlayers.ToString()}";

        bool isJoinBtnAction = true;

        if (sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
            isJoinBtnAction = false;

        JoinBtn.gameObject.SetActive(isJoinBtnAction);
    }

    public void Onclick()
    {
        OnJoinSession?.Invoke(SessionInfo);
    }
}
