using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class MovingEnemy : Enemy
{
    public NavMeshAgent agent;

    public GameObject currentWaypoint;

    public float pivotRange;
    public float minPivotRange;
    public void SetPositionCloseToTarget(PjBase target)
    {
        Vector3 point = new Vector3(Random.Range(-pivotRange, pivotRange), Random.Range(-pivotRange, pivotRange), transform.position.z);
        point += target.transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, 100, 1);
        agent.destination = point;
        Vector3 pointDist = target.transform.position - point;
        int iterations = 0;
        while (pointDist.magnitude > minPivotRange && Physics2D.Raycast(point, pointDist, pointDist.magnitude, GameManager.Instance.wallLayer) && iterations < 30)
        {
            point = new Vector3(Random.Range(-pivotRange, pivotRange), Random.Range(-pivotRange, pivotRange), transform.position.z);
            point += target.transform.position;

            NavMesh.SamplePosition(point, out hit, 100, 1);
            pointDist = target.transform.position - point;
            agent.destination = point;
            iterations++;
        }

        NavMesh.SamplePosition(point, out hit, 100, 1);
        agent.destination = point;
    }

    public override void Stunn(float stunnTime)
    {
        base.Stunn(stunnTime);
        agent.SetDestination(transform.position);
    }

    public GameObject GetRandomWaypoint()
    {
        return GameManager.Instance.waypoints[Random.Range(0,GameManager.Instance.waypoints.Count-1)];
    }
}
