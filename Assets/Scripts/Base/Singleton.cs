using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJLFrame
{
    public class Singleton<T> where T : new()
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
        protected Singleton()
        {

        }
    }
    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        //记录单例对象是否存在。用于防止在OnDestroy方法中访问单例对象报错
        //public static bool IsExisted { get; private set; } = false;
        protected static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        return instance;
                    }

                    if (instance == null)
                    {
                        string instanceName = typeof(T).Name;
                        GameObject instanceGO = GameObject.Find(instanceName);

                        if (instanceGO == null)
                            instanceGO = new GameObject(instanceName);
                        instance = instanceGO.AddComponent<T>();
                        DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
                        //IsExisted = true;
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        { }

        protected virtual void OnEnable()
        { }

        protected virtual void OnDisable()
        {
            //IsExisted = false;
        }

        protected virtual void Start()
        { }

        protected virtual void LateUpdate()
        { }

        protected virtual void Update()
        { }

        protected virtual void OnGUI()
        { }

        protected virtual void OnDestroy()
        {
           
        }
    }





}


