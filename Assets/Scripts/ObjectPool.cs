using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _initialPoolSize = 10;

    protected Queue<T> _pool = new Queue<T>();
    protected List<T> _activeObjects = new List<T>();

    protected virtual void Awake()
    {
        InitializePool();
    }

    protected virtual void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
            CreateNewObject();
    }

    protected virtual T CreateNewObject()
    {
        T newObj = Instantiate(_prefab, transform);
        newObj.gameObject.SetActive(false);
        _pool.Enqueue(newObj);

        return newObj;
    }

    public virtual T GetObject()
    {
        if (_pool.Count == 0)
            CreateNewObject();

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        _activeObjects.Add(obj);

        return obj;
    }

    public virtual void ReturnObject(T obj)
    {
        if (obj == null) 
            return;

        obj.gameObject.SetActive(false);
        _activeObjects.Remove(obj);
        _pool.Enqueue(obj);
    }

    public virtual void ReturnAllObjects()
    {
        foreach (T obj in _activeObjects.ToArray())
            ReturnObject(obj);
    }
}