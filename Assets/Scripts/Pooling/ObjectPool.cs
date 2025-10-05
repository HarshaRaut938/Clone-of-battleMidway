using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic Object Pool for MonoBehaviour objects (Bullets, Enemies, etc.)
/// </summary>
public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Transform parent;
    private Stack<T> poolStack;

    public ObjectPool(T prefab, Transform parent, int initialSize)
    {
        this.prefab = prefab;
        this.parent = parent;
        poolStack = new Stack<T>(initialSize);

        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            if (obj is IPoolable poolable) poolable.OnDespawn();
            poolStack.Push(obj);
        }
    }

    /// <summary>Get an object from the pool</summary>
    public T Get()
    {
        T obj = poolStack.Count > 0 ? poolStack.Pop() : GameObject.Instantiate(prefab, parent);

        obj.gameObject.SetActive(true);

        if (obj is IPoolable poolable)
            poolable.OnSpawn();

        return obj;
    }

    /// <summary>Return an object to the pool</summary>
    public void Return(T obj)
    {
        if (obj == null) return;

        if (obj is IPoolable poolable)
            poolable.OnDespawn();

        obj.gameObject.SetActive(false);
        poolStack.Push(obj);
    }
}
