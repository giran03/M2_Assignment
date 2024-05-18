using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Controller : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    GameObject playerRef;
    PlayerController playerController;

    [Header("Detection Configs")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] GameObject bangLabel;

    [Header("FOV Configs")]
    [SerializeField] float attackRange;
    [SerializeField] float radius; [Range(0, 360)]
    [SerializeField] float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    bool canDamage = true;
    bool canSeePlayer;

    [Header("Waypoint Configs")]
    [SerializeField] List<GameObject> waypointCollection;
    int currentWaypointIndex;


    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerController = playerRef.GetComponent<PlayerController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }

    private void Update()
    {
        if (canSeePlayer)
            ChasePlayer();
        else
            Patrol();
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    void Patrol()
    {
        if (!navMeshAgent.isActiveAndEnabled) return;

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.speed = walkSpeed;
            WaypointMove();
        }
    }

    void ChasePlayer()
    {
        if (!navMeshAgent.isActiveAndEnabled) return;

        transform.LookAt(playerRef.transform.position);
        Vector3 moveTo = Vector3.MoveTowards(transform.position, playerRef.transform.position, 5f);

        // increase move speed
        navMeshAgent.speed = runSpeed;

        navMeshAgent.SetDestination(moveTo);

        if (Vector3.Distance(navMeshAgent.transform.position, playerRef.transform.position) < attackRange && canDamage)
            StartCoroutine(PlayerDamage());
    }

    // Single Tick
    IEnumerator PlayerDamage()
    {
        Debug.Log("Player Shot!\nEnding Game~");

        canDamage = false;
        navMeshAgent.enabled = false;
        playerController.characterController.enabled = false;
        bangLabel.SetActive(true);

        yield return new WaitForSeconds(1);
        SceneHandler.Instance.GoToScene("_Dead");
    }

    void WaypointMove()
    {
        if (waypointCollection.Count == 0) return;

        float distanceToWaypoint = Vector3.Distance(waypointCollection[currentWaypointIndex].transform.position, transform.position);

        if (distanceToWaypoint <= 3)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointCollection.Count;

        navMeshAgent.SetDestination(waypointCollection[currentWaypointIndex].transform.position);
    }
}
