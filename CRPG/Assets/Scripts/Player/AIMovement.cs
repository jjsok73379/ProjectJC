using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : CharacterMovement
{
    protected void MoveToPositionByNav(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, path))
        {
            switch (path.status)
            {
                case NavMeshPathStatus.PathComplete:
                    break;
                case NavMeshPathStatus.PathPartial:
                    break;
                case NavMeshPathStatus.PathInvalid:
                    break;
            }
            StartCoroutine(MovingByPath(path.corners));
        }
    }
    IEnumerator MovingByPath(Vector3[] path)
    {
        int i = 1;
        while (i < path.Length)
        {
            bool bDone = false;
            MoveToPosition(path[i], () => bDone = true);
            while (!bDone)
            {
                yield return null;
            }
            ++i;
        }
    }
}
