using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static WayPoints instance;
    private Transform[] waypoints;
    public Dictionary<Transform, Transform[]> pathToWaypoints = new Dictionary<Transform, Transform[]>();

    void Awake()
    {
        instance = this;
        
        foreach (Transform path in transform)
        {
            List<Transform> waypoints = new List<Transform>();

            foreach (Transform waypoint in path)
            {
                waypoints.Add(waypoint);
            }

            pathToWaypoints.Add(path, waypoints.ToArray());
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Transform[] GetRandomPath() {
        var randomIndex = Random.Range(0, pathToWaypoints.Keys.Count);
        return pathToWaypoints[pathToWaypoints.Keys.ElementAt(randomIndex)];
    }
}
