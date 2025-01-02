using UnityEngine;
using UnityEngine.AI;

public class KartOpponentAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform finishLine; 
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isHeadingToFinish = false; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("Nu exista waypoint-uri setate!");
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (!isHeadingToFinish)
            {
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Length)
                {
                    isHeadingToFinish = true;
                    agent.SetDestination(finishLine.position); 
                }
                else
                {
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
            else
            {
                if (agent.remainingDistance < 1f)
                {
                    agent.isStopped = true;
                    Debug.Log("Kartul a ajuns la linia de finish!");
                }
            }
        }
    }
}
