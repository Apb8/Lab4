using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberFSM : MonoBehaviour
{
    public Transform cop;
    public GameObject treasure;
    public float distToSteal = 10f;
    public float approachDistance = 2f;
    private NavMeshAgent agent;

    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    private delegate IEnumerator State();
    private State state;

    private float wanderTimer = 2f;
    private float timer = 0f;
    private bool hasStolenTreasure = false; // for testing

    IEnumerator Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        yield return wait;

        state = Wander;

        while (enabled)
            yield return StartCoroutine(state());
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander state");

        WanderMov();

        while (Vector3.Distance(cop.position, treasure.transform.position) < distToSteal && !hasStolenTreasure)
        {
            timer += Time.deltaTime;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && timer >= wanderTimer)
            {
                WanderMov();
                timer = 0f;
                Debug.Log("New wander destination set to: " + agent.destination);
            }

            if (Vector3.Distance(transform.position, treasure.transform.position) <= approachDistance)
            {
                state = Approaching;
                yield break;
            }

            yield return wait;
        };

        state = Approaching;
    }

    IEnumerator Approaching()
    {
        Debug.Log("Approaching state");

        agent.speed = 2f;
        Seek(treasure.transform.position);

        while (Vector3.Distance(cop.position, treasure.transform.position) > distToSteal && !hasStolenTreasure)
        {
            if (Vector3.Distance(treasure.transform.position, transform.position) < approachDistance)
            {
                hasStolenTreasure = true;
                treasure.GetComponent<Renderer>().enabled = false;
                Debug.Log("Treasure stolen!");
                state = Hiding;
                yield break;
            }
            yield return wait;
        };

        if (!hasStolenTreasure)
        {
            agent.speed = 1f;
            state = Wander;
        }
    }

    IEnumerator Hiding()
    {
        Debug.Log("Hiding state");

        Hide();

        while (true)
        {
            yield return wait; // mantengo en Hiding indefinidamente, modificar? y q vuelva a wander
        }
    }

    // Movement methods

    private void WanderMov()
    {
        Vector3 wanderPos = RandomNavMeshLocation();
        agent.SetDestination(wanderPos);
    }

    private void Seek(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private void Hide()
    {
        Vector3 hidingSpot = FindBestHidingSpot();
        if (hidingSpot != Vector3.zero)
        {
            agent.SetDestination(hidingSpot);
        }
    }

    private Vector3 RandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10.0f;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, 10.0f, NavMesh.AllAreas);

        return navHit.position;
    }

    private Vector3 FindBestHidingSpot()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        Vector3 bestSpot = Vector3.zero;
        float maxDistance = 0.0f;

        foreach (GameObject obstacle in obstacles)
        {
            float distanceFromCop = Vector3.Distance(obstacle.transform.position, cop.position);
            if (distanceFromCop > maxDistance)
            {
                maxDistance = distanceFromCop;
                bestSpot = obstacle.transform.position;
            }
        }

        return bestSpot;
    }

}
