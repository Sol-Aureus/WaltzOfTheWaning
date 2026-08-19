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
    [SerializeField] private GameObject[] characterModels;

    [Header("Character Data")]
    [SerializeField] private CharacterMenuData[] characterData;
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
        camera = Instantiate(camera);
        lighting = Instantiate(lighting);
        uiController = Instantiate(uiController);
        panelRenderer = Instantiate(panelRenderer);
    }

    private async Awaitable CreateObjects()
    {
        await SceneManager.LoadSceneAsync("CharacterSelectScene", LoadSceneMode.Additive);

        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i] = Instantiate(characterModels[i]);
        }
    }

    private void PrepareGame()
    {
        lighting.enabled = true;

        camera.transform.position = new Vector3(-0.7f, 1.7f, 2.15f);
        camera.transform.rotation = Quaternion.Euler(20, 155, 0);

        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].transform.position = Vector3.zero;
        }

        uiController.SetCharacterData(characterData, characterModels, characterSelection);
        uiController.SetPanelRenderer(panelRenderer);
    }
}
