using Fusion;
using UnityEngine;

public class CharacterSpawner : SimulationBehaviour, IPlayerJoined
{
    [Header("Spawn Settings")]
    [SerializeField] private NetworkObject _playerPrefab;
    [SerializeField] private Vector2 _spawnRangeX = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 _spawnRangeZ = new Vector2(-10f, 10f);
    [SerializeField] private float _spawnY = 1f;

    [Header("Debug")]
    [SerializeField] private bool _showSpawnArea = true;


    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(_playerPrefab, GetRandomSpawnPosition(), Quaternion.identity,
                Runner.LocalPlayer, (runner, obj) =>
                {
                    var _player = obj.GetComponent<PlayerSetup>();
                    _player.SetupCamera();
                }
                );

            //Vector3 spawnPos = GetRandomSpawnPosition();
            //NetworkObject playerObj = Runner.Spawn(
            //    _playerPrefab,
            //    spawnPos,
            //    Quaternion.identity,
            //    player
            //    player,
            //    (runner, obj) => SetupPlayerCamera(obj, player)
            //);
        }
    }

    //private void SetupPlayerCamera(NetworkObject playerObj, PlayerRef player)
    //{
    //    // Chỉ xử lý cho local player
    //    if (player == Runner.LocalPlayer)
    //    {
    //        // Tìm camera trong player prefab
    //        Camera playerCamera = playerObj.GetComponentInChildren<Camera>(true);

    //        if (playerCamera != null)
    //        {
    //            // Kích hoạt camera
    //            playerCamera.gameObject.SetActive(true);
    //            playerCamera.enabled = true;

    //            // Đặt làm camera chính
    //            playerCamera.tag = "MainCamera";

    //            // Tắt các camera khác nếu có
    //            DisableOtherCameras(playerCamera);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Không tìm thấy camera trong player prefab!");
    //        }
    //    }
    //    else // Remote player
    //    {
    //        // Tắt camera nếu có
    //        Camera playerCamera = playerObj.GetComponentInChildren<Camera>(true);
    //        if (playerCamera != null)
    //        {
    //            playerCamera.gameObject.SetActive(false);
    //        }
    //    }
    //}

    private void DisableOtherCameras(Camera keepActive)
    {
        Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Camera cam in allCameras)
        {
            if (cam != keepActive && cam.CompareTag("MainCamera"))
            {
                cam.enabled = false;
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(_spawnRangeX.x, _spawnRangeX.y),
            _spawnY,
            Random.Range(_spawnRangeZ.x, _spawnRangeZ.y)
        );
    }

    private void OnDrawGizmos()
    {
        if (!_showSpawnArea) return;

        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3(
            (_spawnRangeX.x + _spawnRangeX.y) / 2,
            _spawnY,
            (_spawnRangeZ.x + _spawnRangeZ.y) / 2
        );
        Vector3 size = new Vector3(
            Mathf.Abs(_spawnRangeX.y - _spawnRangeX.x),
            0.1f,
            Mathf.Abs(_spawnRangeZ.y - _spawnRangeZ.x)
        );
        Gizmos.DrawWireCube(center, size);
    }
}