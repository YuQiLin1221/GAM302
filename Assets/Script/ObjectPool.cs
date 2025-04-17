using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : NetworkBehaviour
{
    [SerializeField] private NetworkObject prefab;
    [SerializeField] private int initialPoolSize = 10;
    public int xPos;
    public int zPos;

    private List<NetworkObject> pool = new List<NetworkObject>();

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewObject();
            }
            StartCoroutine(SpawnItemsRoutine());
        }
    }

    private NetworkObject CreateNewObject()
    {
        if (!Runner.IsRunning) return null;

        NetworkObject obj = Runner.Spawn(prefab, GetRandomPosition(), Quaternion.identity);

        if (obj != null && obj.IsValid)
        {
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }

        return obj;
    }

    public NetworkObject GetNetworkObject()
    {
        foreach (NetworkObject obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                ActivateObject(obj);
                return obj;
            }
        }

        // Nếu không có đối tượng nào khả dụng, tạo mới
        return CreateNewObject();
    }

    private void ActivateObject(NetworkObject obj)
    {
        if (obj != null && obj.IsValid)
        {
            obj.transform.position = GetRandomPosition();
            obj.gameObject.SetActive(true);
        }
    }

    public void ReturnObject(NetworkObject obj)
    {
        if (obj != null && obj.IsValid)
        {
            obj.gameObject.SetActive(false);
        }
    }

    private Vector3 GetRandomPosition()
    {
        xPos = Random.Range(-10, 10);
        zPos = Random.Range(-10, 10);
        return new Vector3(xPos, 1, zPos);
    }

    private IEnumerator SpawnItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (CountActiveItems() < 10)
            {
                GetNetworkObject();
            }
        }
    }

    private int CountActiveItems()
    {
        int count = 0;
        foreach (NetworkObject obj in pool)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
}