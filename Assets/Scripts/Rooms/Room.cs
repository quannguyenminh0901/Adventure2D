using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPos;

    private void Awake()
    {
        // Save the initial positions of the enemies
        initialPos = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPos[i] = enemies[i].transform.position;
        }
    }

    public void ActiveRoom(bool status)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(status);
                enemies[i].transform.position = initialPos[i];
            }
        }
    }
}
