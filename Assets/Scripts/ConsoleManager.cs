using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public int maxHistory = 25;

    // @O: chatPanel, textObject
    public GameObject consolePanel, textObject;
    public TMP_InputField consoleBox;

    // @O: messageList
    [SerializeField]
    List<Command> commandHistory = new List<Command>();

    void Start() {
    }

    // @O: chatBox
    void Update() {
        if (consoleBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                // @T: process command
                InputText(consoleBox.text);
                consoleBox.text = "";
                consoleBox.ActivateInputField();
            }
        } else {
            if(!consoleBox.isFocused && Input.GetKeyDown(KeyCode.F1)) {
                consoleBox.ActivateInputField();
            }

            else if(consoleBox.isFocused && Input.GetKeyDown(KeyCode.F1)) {
                consoleBox.DeactivateInputField();
            }
        }


        if(consoleBox.isFocused) {
            // disable wasd
        } else {
            // enable wasd
        }
    }

    // @O: SendMessageToChat
    public void InputText(string text) {
        if (commandHistory.Count >= maxHistory) {
            Destroy(commandHistory[0].textObject.gameObject);
            commandHistory.Remove(commandHistory[0]);
        }

        Command newCommand = new Command();

        newCommand.text = text;

        GameObject newText = Instantiate(textObject, consolePanel.transform);

        newCommand.textObject = newText.GetComponent<TMP_Text>();

        newCommand.textObject.text = newCommand.text;

        commandHistory.Add(newCommand);
    }
}

// @O: Message
[System.Serializable]
public class Command {
    public string text;
    public TMP_Text textObject;
}
