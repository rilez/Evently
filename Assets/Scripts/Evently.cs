using System;
using System.Collections.Generic;
using UnityEngine;

public class Evently
{
    private static Evently eventManagerInstance;
    public static Evently Instance => eventManagerInstance ?? (eventManagerInstance = new Evently());
    private readonly Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();
 
    public void Subscribe<T>(Action<T> del)
    {
        if (delegates.ContainsKey(typeof(T)))
        {
//            // This doesn't work!
//            delegates[typeof(T)] += del;
//            // This is ok
//            delegates[typeof(T)] = (Action<T>)delegates[typeof(T)] + del;
            // This is ok
            delegates[typeof(T)] = Delegate.Combine(delegates[typeof(T)], del);
        }
        else
        {
            delegates[typeof(T)] = del;
        }
    }
    
    public void Unsubscribe<T>(Action<T> del)
    {
        if (!delegates.ContainsKey(typeof(T))) return;
        
        var currentDel = Delegate.Remove(delegates[typeof(T)], del);
 
        if (currentDel == null)
        {
            delegates.Remove(typeof(T));
        }
        else
        {
            delegates[typeof(T)] = currentDel;
        }
    }
 
    public void Publish<T> (T e)
    {
        if (e == null)
        {
            Debug.Log("Invalid event argument: " + e.GetType());
            return;
        }
 
        if (delegates.ContainsKey(e.GetType()))
        {
            delegates[e.GetType()].DynamicInvoke(e);
        }
    }
}