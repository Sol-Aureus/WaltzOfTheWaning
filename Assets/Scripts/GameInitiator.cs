using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private Light lighting;

    [Header("UI")]

    [Header("Game Objects")]
    [SerializeField] private GameObject[] playerCharacters;

    [Header("Character Data")]
    [SerializeField] private CharacterSelection characterSelection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async Awaitable Start()
    {
        BindObjects();
        await CreateObjects();
        PrepareGame();
    }

    private void BindObjects()
    {
        lighting = Instantiate(lighting);
    }

    private async Awaitable CreateObjects()
    {
        await SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        playerCharacters[characterSelection.selectedCharacterIndex] = Instantiate(playerCharacters[characterSelection.selectedCharacterIndex]);
    }

    private void PrepareGame()
    {
        lighting.enabled = true;
        playerCharacters[characterSelection.selectedCharacterIndex].transform.position = Vector3.zero;
    }
}
