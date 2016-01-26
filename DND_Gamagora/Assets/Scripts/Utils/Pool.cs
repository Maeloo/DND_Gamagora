using System;
using UnityEngine;
using System.Collections.Generic;


public class Pool<T> : UnityEngine.Object where T : Poolable<T>, new ( ) {
    T           template;

    List<T>     unavailableObjects,
                availableObjects;

    public List<T> usedObjects
    {
        get { return unavailableObjects;  }
    }

    public uint sizeMax = 5;
    public bool automaticReuseUnavailables = false;

    private uint size = 0;


    public Pool ( T a_template, uint a_sizeInit, uint a_sizeMax ) {
        template            = a_template;
        unavailableObjects  = new List<T> ( );
        availableObjects    = new List<T> ( );
        sizeMax             = a_sizeMax;

        for ( int i = 0; i < a_sizeInit; ++i ) {
            T obj = new T ( );
            obj = obj.Create ( );
            obj.Register ( this );
            obj.Duplicate ( template );
            availableObjects.Add ( obj );
            ++size;
        }
    }

    public bool GetAvailable ( bool a_forceExpand, out T obj ) {
        if (availableObjects.Count > 0) {
            obj = availableObjects[availableObjects.Count - 1];
            availableObjects.Remove(obj);
            unavailableObjects.Add(obj);
        }
        else if (automaticReuseUnavailables)
        {
            for(int i = unavailableObjects.Count - 1; i >=0; i--)
            {
                obj = unavailableObjects[i];
                unavailableObjects.Remove(obj);
                availableObjects.Add(obj);
            }

            obj = availableObjects[availableObjects.Count - 1];
            availableObjects.Remove(obj);
            unavailableObjects.Add(obj);
        }
        else {
            if ((size < sizeMax) || a_forceExpand)
            {
                obj = new T();
                obj = obj.Create();
                obj.Register(this);
                obj.Duplicate(template);
                unavailableObjects.Add(obj);
                ++size;
            }
            else {
                obj = new T();
                return false;
            }
        }

        return true;
    }

    public void onRelease ( T obj ) {
        unavailableObjects.Remove ( obj );
        availableObjects.Add ( obj );
    }
}
