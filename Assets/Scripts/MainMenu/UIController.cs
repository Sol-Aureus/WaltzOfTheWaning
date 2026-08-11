using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private PanelRenderer panelRenderer;

    private VisualElement rootElement;
    private CharacterMenuData[] characters;

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

        if (characters != null && characters.Length > 0)
        {
            UpdateCharacterList();
            UpdateInfoBox(0);
        }
    }

    /// <summary>
    /// SetCharacterData sets the character data for this UIController. It should be called before the UI is reloaded to ensure that the character list and info box are populated correctly.
    /// </summary>
    /// <param name="characterData">The list of Menu Character Data</param>
    public void SetCharacterData(CharacterMenuData[] characterData)
    {
        characters = characterData;
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

        characterInfoBox.dataSource = characters[charIndex];
        characterName.dataSource = characters[charIndex];
    }

    /// <summary>
    /// UpdateCharacterList populates the character list in the UI with buttons for each character. Each button displays the character's title and portrait, and clicking a button updates the info box with that character's data.
    /// </summary>
    public void UpdateCharacterList()
    {
        if (rootElement == null) return;

        VisualElement characterList = rootElement.Q<VisualElement>("CharacterList");
        characterList.Clear();

        if (characters == null || characters.Length == 0) return;

        for (int i = 0; i < characters.Length; i++)
        {
            // 1. Capture index locally to avoid closure issues
            int charIndex = i;

            // 2. Instantiate button
            Button btn = new Button();
            btn.text = characters[i].GetCharacterTitle; // Adjust field name to match your data asset
            btn.style.backgroundImage = new StyleBackground(characters[i].GetCharacterPortrait); // Adjust field name to match your data asset

            // 3. Register click callback
            btn.clicked += () => UpdateInfoBox(charIndex);

            // 4. Add button to list container
            characterList.Add(btn);
        }
    }
}
