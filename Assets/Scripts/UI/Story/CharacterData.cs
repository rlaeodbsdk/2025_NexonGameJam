using UnityEngine;

[CreateAssetMenu(fileName = "newCharacter", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public Sprite CharacterImage;
    public string CharacterName;
    public GameObject CharacterModel;
    public Sprite CryCharacter;
 

}