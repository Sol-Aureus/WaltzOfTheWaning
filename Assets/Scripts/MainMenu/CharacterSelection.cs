using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private GameObject selectedCharacter;

    public void SelectCharacter(GameObject character)
    {
        selectedCharacter = character;
    }
}
