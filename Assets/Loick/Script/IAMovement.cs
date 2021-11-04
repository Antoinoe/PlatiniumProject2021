using UnityEngine;
using UnityEngine.AI;

//A placer en parent de toutes les IA

public class IAMovement : MonoBehaviour
{
    //public bool axisMovement = true;
    //public float patrollingRange = 4f;
    //private Vector2 _target = Vector3.zero;
    //private Vector2 _currentTarget = Vector2.zero;

    //public enum MovementAxis
    //{
    //    X, Y
    //}

    //#region Gizmos

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(_target, patrollingRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(_target, 0.5f);
    //}

    //#endregion

    //#region Moving
    //public void NewSubTarget(NavMeshAgent entity)
    //{
    //    Vector2 iaPos = entity.gameObject.transform.position;
    //    float myX = iaPos.x;
    //    float myY = iaPos.y;
    //    float yPos = myY + Random.Range(myY - (patrollingRange * 2), myY + patrollingRange);
    //    float xPos = myX + Random.Range(myX - patrollingRange, myX + patrollingRange);
    //    if (axisMovement)
    //    {
    //        MovementAxis movement = (MovementAxis)Random.Range(0, 2);
    //        switch (movement)
    //        {
    //            case MovementAxis.X:
    //                _currentTarget = new Vector2(xPos, myY);
    //                break;
    //            case MovementAxis.Y:
    //                _currentTarget = new Vector2(myX, yPos);
    //                break;
    //        }
    //    }
    //    SetDestination(entity);
    //    #endregion 
    //}

    //public void SetTarget(NavMeshAgent entity)
    //{
    //    Vector2 iaPos = entity.gameObject.transform.position;
    //    if (axisMovement)
    //    {
    //        MovementAxis movement = (MovementAxis)Random.Range(0, 2);
    //        switch (movement)
    //        {
    //            case MovementAxis.X:
    //                _currentTarget = new Vector2(_target.x, iaPos.y);
    //                break;
    //            case MovementAxis.Y:
    //                _currentTarget = new Vector2(iaPos.x, _target.y);
    //                break;
    //        }
    //    }
    //    SetDestination(entity);
    //}

    //private void SetDestination(NavMeshAgent entity)
    //{
    //    NavMeshPath path = new NavMeshPath();
    //    if (entity.CalculatePath(_currentTarget, path))
    //    {
    //        entity.SetDestination(_currentTarget);
    //    }
    //}

    public static bool NavmeshReachedDestination(NavMeshAgent meshAgent, Vector2 target,float targetRadius)
    {
        Vector2 meshPos = meshAgent.transform.position;
        float targetMeshPosDistance =  Vector2.Distance(meshPos, target);
        bool isArrive = targetMeshPosDistance <= targetRadius;
        return isArrive;
    }

    //public Vector3 GetCurrentTarget()
    //{
    //    return _currentTarget;
    //}

}