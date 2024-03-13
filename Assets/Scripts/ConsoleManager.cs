using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    public int maxHistory = 25;

    // @O: messageList
    [SerializeField]
    List<Command> commandHistory = new List<Command>();

    void Start() {
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
           InputText("space");
           Debug.Log("Space");
        }
    }

    // @O: SendMessageToChat
    public void InputText(string text) {
        if (commandHistory.Count >= maxHistory) {
            commandHistory.Remove(commandHistory[0]);
        }

        Command newCommand = new Command();

        newCommand.text = text;

        commandHistory.Add(newCommand);
    }
}

// @O: Message
[System.Serializable]
public class Command {
    public string text;
}
