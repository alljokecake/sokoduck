using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagment; @TODO: load levels
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public int maxHistory = 25;

    public GameObject consolePanel, textObject;
    public TMP_InputField consoleBox;

    [SerializeField]
    List<Command> commandHistory = new List<Command>();

    void Start() {
    }

    void Update() {
        if (consoleBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                ProcessCommand(consoleBox.text);
                consoleBox.text = "";
                consoleBox.ActivateInputField();
            }
        } else {
            // @FIXME: if not focused and not empty and f1
            if(!consoleBox.isFocused && Input.GetKeyDown(KeyCode.F1)) {
                consoleBox.ActivateInputField();
            }
        }
    }

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

    public void ProcessCommand(string text) {
        string[] args = text.Split(' ');
        string command = args[0];

        switch (command.ToLower()) {
            case "clear":
                ClearHistory();
                break;
            case "level":
                if (args.Length > 1) {
                    string levelName = args[1];
                    InputText($"Loading level '{levelName}'");
                } else {
                    InputText("Please provide a level name");
                }
                break;
            default:
                InputText($"Unknown command: {args[0]}");
                break;
        }
    }

    public void ClearHistory() {
        foreach (Command command in commandHistory) {
            Destroy(command.textObject.gameObject);
        }

        commandHistory.Clear();
    }

}

[System.Serializable]
public class Command {
    public string text;
    public TMP_Text textObject;
}
