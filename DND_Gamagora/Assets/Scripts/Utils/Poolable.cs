using System;


public interface Poolable<T> {
    T Create ( );

    bool IsReady ( );
    void Duplicate ( T a_template );
    void Register ( UnityEngine.Object pool );
    void Release ( );
}
