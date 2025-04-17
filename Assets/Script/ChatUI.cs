using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI chatContent;
    public TextMeshProUGUI PlayerIdText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    public void SendMessage()
    {
        string mess = inputField.text;
        if (!string.IsNullOrEmpty(mess))
        {
            ChatManager.instance.SendChatMessage(mess);
            inputField.text = "";
        }
    }
    
}
