using UnityEngine;
using UnityEngine.AI;

public class KartOpponentAI : MonoBehaviour
{
    public Transform finishLine; 
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(finishLine.position); 
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            agent.isStopped = true; 
        }
    }
}
