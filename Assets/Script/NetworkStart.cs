using Fusion;
using UnityEngine;
using System;
using System.Collections;


public class NetworkStart : MonoBehaviour
{
    private NetworkRunner _networkRunner;

    // Hàm để tham gia vào phòng (Join)
    public void JoinGame()
    {
        // Tạo một NetworkRunner mới nếu chưa có
        if (_networkRunner == null)
        {
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        // Cấu hình NetworkRunner và bắt đầu game
        StartCoroutine(JoinRoom());
    }

    // Quá trình tham gia phòng
    private IEnumerator JoinRoom()
    {
        // Tạo và bắt đầu game với NetworkRunner
        yield return _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client, // Chế độ Shared Client
            SceneManager = _networkRunner.SceneManager // Dùng SceneManager mặc định của NetworkRunner
        });

        Debug.Log("Đã tham gia phòng Shared Client.");
    }

    // Tắt phòng khi không cần thiết
    public void LeaveGame()
    {
        if (_networkRunner != null)
        {
            _networkRunner.Shutdown();
            Debug.Log("Đã thoát khỏi phòng.");
        }
    }
}
