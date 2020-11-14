using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManagement : MonoBehaviour
{
    public UIDataHolder UIDataHolder;
    public GameObject characterUIPrefab;
    public Transform CharacterUIParent;
    public PlayerInfo PlayerInfo;
    private void Awake()
    {
        foreach (var Character in UIDataHolder.characterDatas)
        {
            var obj = Instantiate(characterUIPrefab, CharacterUIParent);
            Character characterData = obj.GetComponent<Character>();
            characterData.prefabId = Character.ID;
            characterData.name = Character.name;
            Image image = obj.GetComponent<Image>();
            image.sprite = Character.CharacterImage;
            characterData.PlayerInfo = PlayerInfo;
            PlayerInfo.PrefabDict.Add(Character.ID, obj.GetComponent<Character>());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
