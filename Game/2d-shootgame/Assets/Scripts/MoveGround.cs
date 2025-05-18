using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public Vector3 tempPosition;
    public Vector3 targetPosition;
    public Vector3 orginPosition;

    public float moveSpeed;
    private void Awake()
    {
        orginPosition = transform.position;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(transform.position == orginPosition)
        {
            targetPosition = tempPosition;
        }else if(transform.position == tempPosition)
        {
            targetPosition = orginPosition;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
