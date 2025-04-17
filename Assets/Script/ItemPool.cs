using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ItemPool : NetworkBehaviour
{
    public static ItemPool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private NetworkObject itemPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private Queue<NetworkObject> pool = new Queue<NetworkObject>();

    public override void Spawned()
    {
        if (Instance == null)
        {
            Instance = this;

            if (Object.HasStateAuthority)
            {
                InitializePool();
            }
        }
        else
        {
            Runner.Despawn(Object);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewItem();
        }
    }

    private NetworkObject CreateNewItem()
    {
        NetworkObject newItem = Runner.Spawn(itemPrefab, Vector3.zero, Quaternion.identity);
        newItem.gameObject.SetActive(false);
        pool.Enqueue(newItem);
        return newItem;
    }

    public void SpawnItemAtRandomPosition(Vector3 areaSize)
    {
        if (!Object.HasStateAuthority || pool.Count == 0)
        {
            Debug.LogWarning("Cannot spawn item: No authority or empty pool");
            return;
        }

        try
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                0.5f,
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            NetworkObject item = pool.Dequeue();
            if (item != null)
            {
                item.transform.position = randomPos;
                item.gameObject.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Spawn item error: {e.Message}");
        }
    }

    public void ReturnItem(NetworkObject item)
    {
        if (!Object.HasStateAuthority) return;

        item.gameObject.SetActive(false);
        pool.Enqueue(item);
    }
}