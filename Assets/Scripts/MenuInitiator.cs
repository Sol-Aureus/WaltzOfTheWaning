using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInitiator : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private Camera camera;
    [SerializeField] private Light lighting;

    [Header("UI")]

    [Header("Game Objects")]
    [SerializeField] private GameObject[] playerCharacters;
    [SerializeField] private GameObject[] playerModels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async Awaitable Start()
    {
        BindObjects();
        await CreateObjects();
        PrepareGame();
    }

    private void BindObjects()
    {
        camera = Instantiate(camera);
        lighting = Instantiate(lighting);
    }

    private async Awaitable CreateObjects()
    {
        SceneManager.LoadScene("CharacterSelectScene", LoadSceneMode.Additive);

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i] = Instantiate(playerModels[i]);
        }
    }

    private void PrepareGame()
    {
        lighting.enabled = true;

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i].transform.position = Vector3.zero;
            playerModels[i].SetActive(false);
        }
    }
}
