using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardSpawner : MonoBehaviour
{
    public GameObject keyCardPrefab;

    private void Start()
    {
        // Ensure the keycard is hidden when the scene starts
        keyCardPrefab.SetActive(false);
    }

    public void SpawnKeyCard()
    {
        // Spawn the keycard (if it was disabled in the scene)
        keyCardPrefab.SetActive(true);

        // Optionally, you can instantiate a new keycard if you don't want to use the one that's already in the scene
        // Instantiate(keyCardPrefab, transform.position, Quaternion.identity);
    }
}