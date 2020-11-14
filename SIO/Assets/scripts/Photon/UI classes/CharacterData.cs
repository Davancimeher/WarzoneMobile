using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "GameData/Characters", order = 1)]
public class CharacterData : ScriptableObject
{
    public byte ID;
    public string Name;
    public Sprite CharacterImage;
}
