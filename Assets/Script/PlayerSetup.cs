using Fusion;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    public void SetupCamera()
    {
        if (Object.HasInputAuthority)
        {
            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.AssignCamera(transform); // Gán nhân vật hiện tại làm đối tượng theo dõi
            }
        }
    }
    
}