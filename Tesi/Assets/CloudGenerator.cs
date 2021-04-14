using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject cloudPrefab;
    bool startGame = false;
    public bool endGame = false;
    public float cloudGenerationTime = 2.5f;

    [SerializeField] private List<GameObject> positionsList;


    public void EndGame()
    {
        endGame = true;

    }
    public void ResetGame()
    {
        endGame = false;
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void StartGame()
    {
        startGame = true;
        StopAllCoroutines();
        StartCoroutine(CloudGeneration());
    }

    private IEnumerator CloudGeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(cloudGenerationTime);
            if (startGame && !PauseMenu.gamePaused && !endGame)
            {
                Debug.Log("Nuvola generata");
                System.Random r = new System.Random();
                int index = r.Next(0, positionsList.Count);
                GameObject cloud = Instantiate(cloudPrefab, positionsList[index].transform.position, Quaternion.identity) as GameObject;
                cloud.transform.parent = this.transform;
                cloud.transform.localScale = new Vector3(1, 1, 1);
            }

        }
    }
}
