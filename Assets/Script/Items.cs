using UnityEngine;
using Fusion;
using System.Collections;

public class Items : NetworkBehaviour
{
    private ObjectPool objectPool;

    public override void Spawned()
    {
        // Tìm ObjectPool trong scene
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerProperties player = other.GetComponent<PlayerProperties>();
        if (player != null && Object.HasStateAuthority)
        {
            // Tăng điểm cho player
            player.AddScore(1);
            objectPool.ReturnObject(Object);
            //DestroyItem();

            // Bắt đầu coroutine để hiển thị lại sau 1 giây
            //StartCoroutine(RespawnAfterDelay(1f));
        }
    }

    //private void DestroyItem()
    //{
    //    if (Object.HasStateAuthority)
    //    {
    //        // Thay vì Despawn, trả về ObjectPool
    //        objectPool.ReturnObject(Object);
    //        //FindObjectOfType<ObjectPool>().ReturnObject(gameObject);
    //    }
    //}

    //private IEnumerator RespawnAfterDelay(float delay)
    //{
        //yield return new WaitForSeconds(delay);

        //if (Object.HasStateAuthority)
        //{
            //gameObject.SetActive(true);
        //}

        //if (Object.HasStateAuthority)
        //{
        //    objectPool.GetNetworkObject();
        //}
    //}

}
