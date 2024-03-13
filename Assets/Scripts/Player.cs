using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // @FIXME: fix hardcoded stuff
    [SerializeField] public float moveSpeed = 7f;
    [SerializeField] public float rotateSpeed = 10f;
    [SerializeField] public bool consoleMode = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            consoleMode = !consoleMode;
        }

        if (consoleMode) {
            moveSpeed = 0f;
            rotateSpeed = 0f;
        } else {
            moveSpeed = 7f;
            rotateSpeed = 10f;
        }

        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
