using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcApplyDamageToPlayer(PlayerRef targetPlayer, int damage)
    {
        Runner.TryGetPlayerObject(targetPlayer, out var plObject);
        if (plObject == null)
        {
            return;
        }
        plObject.GetComponent<PlayerProperties>().TakeDMG(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        var targetPlayer = other.gameObject.GetComponent<NetworkObject>().InputAuthority;
        
    }
}
