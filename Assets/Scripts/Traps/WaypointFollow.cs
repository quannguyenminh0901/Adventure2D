using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed;
    private int currentWayponitIndex = 0;

    private void Update()
    {
        
        if (Vector2.Distance(waypoints[currentWayponitIndex].transform.position, transform.position) < 0.1f)
        {
            currentWayponitIndex++;
            if (currentWayponitIndex >= waypoints.Length)
            {
                currentWayponitIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayponitIndex].transform.position, Time.deltaTime * speed);
    }
}
