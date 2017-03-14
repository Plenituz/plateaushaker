using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class AIRendererCam : MonoBehaviour {
    private SortedDictionary<int, List<Action>> onRenderObject = new SortedDictionary<int, List<Action>>();

    public void Add(Action action, int order)
    {
        List<Action> entry;
        onRenderObject.TryGetValue(order, out entry);

        if(entry == null)
        {
            //this is a new order
            entry = new List<Action>();
            entry.Add(action);
            onRenderObject.Add(order, entry);
        }
        else
        {
            //this order is already in there
            entry.Add(action);
        }
    }

    public void Remove(int order, Action action)
    {
        //onRenderObject.Remove(action);
        try
        {
            onRenderObject[order].Remove(action);
        }
        catch (Exception) { }
    }

	void OnRenderObject () {
	    if(onRenderObject != null)
        {
            foreach(KeyValuePair<int, List<Action>> pair in onRenderObject)
            {
                for(int i = 0; i < pair.Value.Count; i++)
                {
                    pair.Value[i]();
                }
            }
        }
	}

    void OnDrawGizmos()
    {
        //Debug.Log("drawgizmos");
        OnRenderObject();
    }
}
