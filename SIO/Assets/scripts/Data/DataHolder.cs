using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance;

    public Dictionary<byte, string> prefabsName = new Dictionary<byte, string>()
    {
        { 0,"Berz" },
        { 1,"Machou" },
    };

    void Awake()
    {
        if (DataHolder.Instance == null)
        {
            DataHolder.Instance = this;
        }
        else
        {
            if (DataHolder.Instance != this)
            {
                Destroy(DataHolder.Instance.gameObject);
                DataHolder.Instance = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
