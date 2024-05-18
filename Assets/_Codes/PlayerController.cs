using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 playerVelocity;
    public CharacterController characterController;

    [Header("Configs")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    void Start() => characterController = GetComponent<CharacterController>();

    void Update()
    {
        if (!characterController.enabled) return;

        Vector3 move = new(0, 0, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        characterController.Move(playerVelocity * Time.deltaTime + movementSpeed * Time.deltaTime * move);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
            SceneHandler.Instance.FinishLevel();
    }
}
