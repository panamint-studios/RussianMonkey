using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyProperty<T>
{
    public event Action Updated;
    private T m_Value;
    public T Value
    {
        get
        {
            return m_Value;
        }
        set
        {
            if (!EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                m_Value = value;
                Updated?.Invoke();
            }
        }
    }
}
