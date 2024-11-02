using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CopFSM : MonoBehaviour
{

    private NavMeshAgent agent;
    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    private delegate IEnumerator State();
    private State state;

    // Start is called before the first frame update
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
        Debug.Log("Cop in Wander state");
        WanderMov();
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
    {
        yield return null;
    }

    yield return wait;
    }

// Movement methods
    private void WanderMov()
    {
        Vector3 wanderPos = RandomNavMeshLocation();
        agent.SetDestination(wanderPos);
    }

    private Vector3 RandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10.0f;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, 10.0f, NavMesh.AllAreas);

        return navHit.position;
    }
}
