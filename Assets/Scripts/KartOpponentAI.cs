using UnityEngine;
using UnityEngine.AI;

public class KartOpponentAI : MonoBehaviour
{
    [SerializeField] private RaceTimer raceTimer; // Reference to RaceTimer
    [SerializeField] private Collider finishLineCol; // Reference to the finish line collider

    public Transform[] waypoints;
    public Transform finishLine;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isHeadingToFinish = false;

    public float raycastOffset = 1.0f;
    public float rotationSpeed = 10f;

    private bool isRaceStarted = false; 
    private bool hasFinished = false;
    private Vector3 initialPosition; 
    private Quaternion initialRotation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        agent.updatePosition = false; 
        agent.updateRotation = false; 

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        RaceTimer.OnRaceStart += StartRace; 
    }

    void Update()
    {
        if (!isRaceStarted)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            return;
        }

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
            else if (!hasFinished && agent.remainingDistance < 1f)
            {
                hasFinished = true;
                agent.isStopped = true;
                Debug.Log("KartOpponentAI: The AI kart has reached the finish line!");

                RaceTimer raceTimer = Object.FindAnyObjectByType<RaceTimer>();
                if (raceTimer != null)
                {
                    raceTimer.StopRaceTimer(false); 
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
            Debug.LogWarning("Ground for kart alignment not detected.");
        }
    }

    void StartRace()
    {
        isRaceStarted = true; 

        agent.Warp(transform.position);

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("No waypoints are set!");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward * raycastOffset + Vector3.up, Vector3.down * 2f);
        Gizmos.DrawRay(transform.position - transform.forward * raycastOffset + Vector3.up, Vector3.down * 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished) return; // Prevent multiple triggers

        if (other == finishLineCol) // Check if the finish line collider is hit
        {
            hasFinished = true; // Mark the kart as finished
            raceTimer.StopRaceTimer(false); // Call RaceTimer to display results for the AI
            Debug.Log("AI finished the race!");
        }
    }

    private void OnDestroy()
    {
        RaceTimer.OnRaceStart -= StartRace; 
    }
}
