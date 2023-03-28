using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;

    //use objectpool as pool's main, object's name(string) as key, object storing in queue as value
    private Dictionary<string, Queue<GameObject>> objectpool = new Dictionary<string, Queue<GameObject>>();
    //Parent
    private GameObject pool;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;
        //search if prefab exists & Instantiated number
        if (!objectpool.ContainsKey(prefab.name) || objectpool[prefab.name].Count == 0)
        {

            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
            }

            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        //out queue & active
        _object = objectpool[prefab.name].Dequeue();
        _object.SetActive(true);

        return _object;
    }
    //put used prefab to pool
    public void PushObject(GameObject prefab)
    {
        //get name
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectpool.ContainsKey(_name))
        {
            //new an objectpool
            objectpool.Add(_name, new Queue<GameObject>());
        }
        //in queue
        objectpool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
    public GameObject GetObjectPos(GameObject prefab, Vector2 genPoint)
    {
        GameObject _object;
        //search if prefab exists & Instantiated number
        if (!objectpool.ContainsKey(prefab.name) || objectpool[prefab.name].Count == 0)
        {

            _object = GameObject.Instantiate(prefab, genPoint, Quaternion.identity);
            PushObject(_object);

            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
            }

            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        //out queue & active
        _object = objectpool[prefab.name].Dequeue();
        _object.SetActive(true);

        return _object;
    }
}
