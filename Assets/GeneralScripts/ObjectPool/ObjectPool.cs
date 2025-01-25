using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable 
{
    private readonly List<T> activePool = new();
    private readonly List<T> inactivePool = new();

    private readonly T prefab;

    public ObjectPool(int startingCount, T prefab)
    {
        this.prefab = prefab;
        for (int i = 0; i < startingCount; i++)
        {
            AddNewItemToPool();
        }
    }

    private T AddNewItemToPool()
    {
        T instance = Object.Instantiate(prefab);
        instance.Active = false;
        inactivePool.Add(instance);
        return instance;
    }

    public T RequestObject()
    {
        if (inactivePool.Count > 0)
        {
            return ActivateItem(inactivePool[0]);
        }
        return ActivateItem(AddNewItemToPool());
    }

    public T ActivateItem(T item)
    {
        item.OnEnableObject();
        item.Active = true;

        if (inactivePool.Contains(item)) { inactivePool.Remove(item); }
        if (!activePool.Contains(item)) { activePool.Add(item); }

        return item;
    }

    public void ReturnObjectToPool(T item)
    {
        item.OnDisableObject();
        item.Active = false;

        if (activePool.Contains(item)) { activePool.Remove(item); }
        if (!inactivePool.Contains(item)) { inactivePool.Add(item); }
    }
}