using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class Lagger : MonoBehaviour {

    public static List<Lagger> all = new List<Lagger>();
    public static bool enableLag = true;
    
    private static bool checkedd = false;
    private const string LAG_PATH = "lag";

    public static void UpdateAll(bool lag)
    {
        enableLag = lag;
        for(int i = 0; i < all.Count; i++)
        {
            all[i].CheckLag();
        }
        DataStorer.Store(LAG_PATH, enableLag.ToString());
    }

    public Behaviour toDisable;

    void Awake()
    {
        if (!checkedd)
        {
            DataStorer.InitFile(LAG_PATH, enableLag.ToString());
            string str = DataStorer.Read(LAG_PATH);
            enableLag = bool.Parse(str);
            checkedd = true;
        }
    }

    void Start()
    {
        all.Add(this);
        CheckLag();
    }

    void OnDestroy()
    {
        all.Remove(this);
    }

    private void CheckLag()
    {
        if (toDisable == null)
            gameObject.SetActive(enableLag);
        else
        {
            toDisable.enabled = enableLag;
        }
    }
}
