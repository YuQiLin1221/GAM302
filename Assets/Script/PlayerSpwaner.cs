using UnityEngine;
using Fusion;

public class PlayerSpwaner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public int xPos;
    public int zPos;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            xPos = Random.Range(-10, 10);
            zPos = Random.Range(-10, 10);
            //Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            Runner.Spawn(PlayerPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity,
                Runner.LocalPlayer, (runner, obj) =>
                {
                    var _player = obj.GetComponent<PlayerSetup>();
                    _player.SetupCamera();
                }

                );
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}