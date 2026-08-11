using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuInitiator : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private Camera camera;
    [SerializeField] private Light lighting;

    [Header("UI")]
    [SerializeField] private UIController uiController;
    [SerializeField] private PanelRenderer panelRenderer;

    [Header("Game Objects")]
    [SerializeField] private GameObject[] playerModels;

    [Header("Character Data")]
    [SerializeField] private CharacterMenuData[] characterData;

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
        uiController = Instantiate(uiController);
        panelRenderer = Instantiate(panelRenderer);
    }

    private async Awaitable CreateObjects()
    {
        await SceneManager.LoadSceneAsync("CharacterSelectScene", LoadSceneMode.Additive);

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i] = Instantiate(playerModels[i]);
        }
    }

    private void PrepareGame()
    {
        lighting.enabled = true;

        Debug.Log("Setting up UI Controller with PanelRenderer and Character Data...");
        uiController.SetCharacterData(characterData);
        uiController.SetPanelRenderer(panelRenderer);

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i].transform.position = Vector3.zero;
            playerModels[i].SetActive(false);
        }
    }
}
