using UnityEngine;
using UnityEngine.AI;

public class KartOpponentAI : MonoBehaviour
{
    public Transform[] waypoints; 
    public Transform finishLine; 
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isHeadingToFinish = false;

    public float raycastOffset = 1.0f; 
    public float rotationSpeed = 10f; 

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
        AlignToGround(); 

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

    void AlignToGround()
    {
        RaycastHit hitFront, hitBack;

        Vector3 frontPosition = transform.position + transform.forward * raycastOffset;
        Vector3 backPosition = transform.position - transform.forward * raycastOffset;

        bool frontHit = Physics.Raycast(frontPosition + Vector3.up, Vector3.down, out hitFront, 5f);
        bool backHit = Physics.Raycast(backPosition + Vector3.up, Vector3.down, out hitBack, 5f);

        if (frontHit && backHit)
        {
            Vector3 groundNormal = (hitFront.normal + hitBack.normal).normalized;

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = targetRotation;

        }
        else
        {
            Debug.LogWarning("Nu s-a detectat terenul pentru alinierea kartului.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward * raycastOffset + Vector3.up, Vector3.down * 2f);
        Gizmos.DrawRay(transform.position - transform.forward * raycastOffset + Vector3.up, Vector3.down * 2f);
    }
}
