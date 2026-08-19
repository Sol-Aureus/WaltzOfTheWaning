using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private VisualElement rootElement;

    private CharacterMenuData[] characterData;
    private GameObject[] characterModels;
    private CharacterSelection characterSelection;

    /// <summary>
    /// SetPanelRenderer sets the PanelRenderer for this UIController and registers a callback for when the UI is reloaded.
    /// </summary>
    /// <param name="renderer">The UI panel renderer</param>
    public void SetPanelRenderer(PanelRenderer renderer)
    {
        if (renderer == null) return;

        panelRenderer = renderer;
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    /// <summary>
    /// OnUIReload is called when the UI is reloaded. It updates the root element and refreshes the character list and info box if character data is available.
    /// </summary>
    /// <param name="renderer">The UI panel renderer</param>
    /// <param name="root">The root node in the UI tree</param>
    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        rootElement = root;

        if (characterData != null && characterData.Length > 0)
        {
            UpdateCharacterList();
            UpdateCharacterSelection(0);
        }
    }

    /// <summary>
    /// SetCharacterData sets the character data for this UIController. It should be called before the UI is reloaded to ensure that the character list and info box are populated correctly.
    /// </summary>
    /// <param name="characterData">The list of Menu Character Data</param>
    public void SetCharacterData(CharacterMenuData[] newCharacterData, GameObject[] newCharacterModels, CharacterSelection newCharacterSelection)
    {
        characterData = newCharacterData;
        characterModels = newCharacterModels;
        characterSelection = newCharacterSelection;
    }

    /// <summary>
    /// UpdateCharacterList populates the character list in the UI with buttons for each character. Each button displays the character's title and portrait, and clicking a button updates the info box with that character's data.
    /// </summary>
    public void UpdateCharacterList()
    {
        if (rootElement == null) return;

        VisualElement characterList = rootElement.Q<VisualElement>("CharacterList");
        characterList.Clear();

        if (characterData == null || characterData.Length == 0) return;

        for (int i = 0; i < characterData.Length; i++)
        {
            // 1. Capture index locally to avoid closure issues
            int charIndex = i;

            // 2. Instantiate button
            Button btn = new Button();
            btn.text = characterData[i].GetCharacterTitle; // Adjust field name to match your data asset
            btn.style.backgroundImage = new StyleBackground(characterData[i].GetCharacterPortrait); // Adjust field name to match your data asset

            // 3. Register click callback
            btn.clicked += () => UpdateCharacterSelection(charIndex);

            // 4. Add button to list container
            characterList.Add(btn);
        }
    }

    /// <summary>
    /// UpdateCharacterSelection calls UpdateInfoBox and ShowModel to update the character info box and display the corresponding character model based on the selected index. It also updates the selected character index in the CharacterSelection ScriptableObject.
    /// </summary>
    /// <param name="charIndex">The index for the character to load</param>
    public void UpdateCharacterSelection(int charIndex)
    {
        UpdateInfoBox(charIndex);
        ShowModel(charIndex);
        SetSelectedCharacter(charIndex);
    }

    /// <summary>
    /// UpdateInfoBox updates the character info box with the data of the character at the specified index.
    /// </summary>
    /// <param name="charIndex">The index for the character to load</param>
    public void UpdateInfoBox(int charIndex)
    {
        if (rootElement == null) return;

        VisualElement characterInfoBox = rootElement.Q<VisualElement>("CharacterInfoBox");
        VisualElement characterName = rootElement.Q<VisualElement>("CharacterName");

        characterInfoBox.dataSource = characterData[charIndex];
        characterName.dataSource = characterData[charIndex];
    }

    /// <summary>
    /// ShowModel activates the character model corresponding to the specified index and deactivates all other models.
    /// </summary>
    /// <param name="charIndex">The index for the character model to load</param>
    private void ShowModel(int charIndex)
    {
        if (characterModels == null || charIndex < 0 || charIndex >= characterModels.Length) return;

        HideAllModels();

        GameObject modelToShow = characterModels[charIndex];
        if (modelToShow != null)
        {
            modelToShow.SetActive(true);
        }
    }

    /// <summary>
    /// HideAllModels deactivates all character models in the characterModels array.
    /// </summary>
    private void HideAllModels()
    {
        if (characterModels == null) return;

        foreach (var model in characterModels)
        {
            if (model != null)
            {
                model.SetActive(false);
            }
        }
    }

    /// <summary>
    /// SetSelectedCharacter updates the selected character index in the CharacterSelection ScriptableObject to the specified index.
    /// </summary>
    /// <param name="index">The index for the character to select</param>
    private void SetSelectedCharacter(int index)
    {
        if (characterSelection == null)
        {
            Debug.LogWarning("CharacterSelection ScriptableObject is not assigned.");
            return;
        }
        characterSelection.selectedCharacterIndex = index;
    }
}
