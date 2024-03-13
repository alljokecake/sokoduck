using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInput : MonoBehaviour
{
    [SerializeField] GameObject consoleGameObject;

    private void Start() {
        consoleGameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            consoleGameObject.SetActive(!consoleGameObject.activeSelf);
        }
    }
}
