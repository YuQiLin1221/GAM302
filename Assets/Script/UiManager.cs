using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("UI Elements - Create Room")]
    [SerializeField] public TMP_InputField createRoomInput;
    [SerializeField] public Button createRoomButton;

    [Header("UI Elements - Join Room")]
    [SerializeField] public TMP_InputField joinRoomInput;
    [SerializeField] public Button joinRoomButton;

    [Header("Error Message")]
    [SerializeField] private TextMeshProUGUI errorText; // (Optional) Hiển thị lỗi

    private void Awake()
    {
        // Gán sự kiện cho các nút
        createRoomButton.onClick.AddListener(OnCreateRoomClicked);
        joinRoomButton.onClick.AddListener(OnJoinRoomClicked);

        // Ẩn thông báo lỗi ban đầu (nếu có)
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }

    public void OnCreateRoomClicked()
    {
        string roomId = createRoomInput.text.Trim();

        if (string.IsNullOrEmpty(roomId))
        {
            ShowError("Vui lòng nhập ID phòng để tạo!");
            return;
        }

        // Thêm cờ để xác định chế độ Shared
        PlayerPrefs.SetInt("IsSharedMode", 1);
        PlayerPrefs.SetString("RoomID", roomId);
        PlayerPrefs.SetString("RoomMode", "Create");

        // Load the GamePlay scene
        SceneManager.LoadScene("GamePlay");
    }

    public void OnJoinRoomClicked()
    {
        string roomId = joinRoomInput.text.Trim();

        if (string.IsNullOrEmpty(roomId))
        {
            ShowError("Vui lòng nhập ID phòng để tham gia!");
            return;
        }

        // Thêm cờ để xác định chế độ Shared
        PlayerPrefs.SetInt("IsSharedMode", 1);
        PlayerPrefs.SetString("RoomID", roomId);
        PlayerPrefs.SetString("RoomMode", "Join");

        // Load the GamePlay scene
        SceneManager.LoadScene("GamePlay");
    }

    public void ShowError(string message)
    {
        if (errorText == null) return;

        errorText.text = message;
        errorText.gameObject.SetActive(true);

        // Tự động ẩn sau 3 giây
        Invoke(nameof(HideError), 3f);
    }

    public void HideError()
    {
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }
}
