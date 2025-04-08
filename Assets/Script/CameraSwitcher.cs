using UnityEngine;
using Fusion;

public class CameraSwitcher : NetworkBehaviour
{
    public Camera[] cameras; // Danh sách các camera
    private int currentCameraIndex = 0; // Chỉ mục camera hiện tại

    private void Start()
    {
        if (cameras.Length == 0)
        {
            Debug.LogError("No cameras assigned in the list.");
            return;
        }

        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        SwitchCamera(0);
    }

    private void Update()
    {
        if (HasStateAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(2);
        }
    }

    private void SwitchCamera(int cameraIndex)
    {
        if (cameraIndex < 0 || cameraIndex >= cameras.Length)
        {
            Debug.LogError("Invalid camera index");
            return;
        }

        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        cameras[cameraIndex].gameObject.SetActive(true);
        currentCameraIndex = cameraIndex;
    }

    // Phương thức để xử lý khi một nhân vật chết
    public void OnPlayerDeath(PlayerProperties deadPlayer)
    {
        // Tìm nhân vật còn sống tiếp theo
        PlayerProperties[] allPlayers = FindObjectsOfType<PlayerProperties>();
        foreach (PlayerProperties player in allPlayers)
        {
            if (player != deadPlayer && player.Health > 0)
            {
                // Chuyển camera sang nhân vật còn sống
                SwitchCameraToPlayer(player);
                break;
            }
        }
    }

    private void SwitchCameraToPlayer(PlayerProperties player)
    {
        // Chuyển camera theo dõi nhân vật
        CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.AssignCamera(player.transform);
        }
    }
}
