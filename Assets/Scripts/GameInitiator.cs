using UnityEngine;

public class GameInitiatorScript : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject lightingPrefab;
    [SerializeField] private GameObject[] characterPrefabs;

    GameObject lighting;
    GameObject character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        BindObjects();
        PlaceObjects();
    }

    private void BindObjects()
    {
        lighting = Instantiate(lightingPrefab);
        character = Instantiate(characterPrefabs[0]);
    }

    private void PlaceObjects()
    {
        character.transform.position = new Vector3(0, 0, 0);
    }
}
