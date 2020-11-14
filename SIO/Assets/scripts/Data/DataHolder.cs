using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance;
    public UIDataHolder UIDataHolder;

    public Dictionary<byte, string> prefabsName = new Dictionary<byte, string>();

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
    private void Start()
    {
        foreach (var character in UIDataHolder.characterDatas)
        {
            prefabsName.Add(character.ID, character.name);
        }
    }
}
