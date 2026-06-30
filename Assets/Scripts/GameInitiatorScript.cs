using System.Collections;
using UnityEngine;

public class GameInitiatorScript : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject lightingPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private GameObject characterPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        BindObjects();
    }

    private void BindObjects()
    {
        GameObject lighting = Instantiate(lightingPrefab);
        GameObject camera = Instantiate(cameraPrefab);
        GameObject character = Instantiate(characterPrefab);
    }
}
