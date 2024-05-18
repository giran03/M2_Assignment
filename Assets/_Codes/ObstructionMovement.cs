using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstructionMovement : MonoBehaviour
{
    [SerializeField] Vector3 maxPosition;
    [SerializeField] float speed;

    private void Start()
    {
        StartCoroutine(MoveRight());
    }

    IEnumerator MoveRight()
    {
        while (true)
        {
            Vector3 initialPos = transform.position;
            initialPos.y = -0.62639f;
            initialPos.z = 0f;

            maxPosition.y = -0.62639f;
            maxPosition.z = 0;

            transform.position = Vector3.MoveTowards(initialPos, maxPosition, speed * Time.deltaTime);
            yield return new WaitForSeconds(4f);
            
        }
    }
}
