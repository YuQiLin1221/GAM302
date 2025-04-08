using UnityEngine;
using Fusion;

public class Items : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player không
        PlayerProperties player = other.GetComponent<PlayerProperties>();
        if (player != null)
        {
            // Nếu va chạm với Player, gọi phương thức DestroyItem
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        // Hủy item trên tất cả các client
        if (Object.HasStateAuthority) // Kiểm tra quyền sở hữu
        {
            Runner.Despawn(Object);
        }
    }
}
