using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagment; @TODO: load levels
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    // @FIXME: history stuff
    // (public int maxHistory = 100;

    public GameObject consolePanel, textObject;
    public TMP_InputField consoleBox;

    [SerializeField]
    List<Command> commandHistory = new List<Command>();

// ----------------------------------------------------------------------------
//     - Manage Commands -
// ----------------------------------------------------------------------------

    private Dictionary<string, Action<string[]>> commands;

    public ConsoleManager () {
        commands = new Dictionary<string, Action<string[]>>();

        RegisterCommand("level", LevelCommand);
        RegisterCommand("clear", ClearCommand);
        RegisterCommand("help", HelpCommand);
    }

    public void RunCommand(string input) {
        string[] parts = input.Split(' ');
        string command = parts[0];
        string[] arguments = parts.Length > 1 ? parts[1..] : new string[0];
        Array.Copy(parts, 1, arguments, 0, arguments.Length);

        if (commands.ContainsKey(command)) {
            commands[command](arguments);
        } else {
            InputText($"ERROR: Unknown command `{command}`");
        }
    }

    private void RegisterCommand(string commandName, Action<string[]> action) {
        commands.Add(commandName, action);
    }

// ----------------------------------------------------------------------------
//     - Unity Stuff -
// ----------------------------------------------------------------------------
    void Start() {
    }

// ----------------------------------------------------------------------------
//     - Spawn/Draw Logic -
// ----------------------------------------------------------------------------
    void Update() {
        // @FIXME: active input field at start
        if (consoleBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                RunCommand(consoleBox.text);
                consoleBox.text = "";
                consoleBox.ActivateInputField();
            } else if (Input.GetKeyDown(KeyCode.F1)) {
                if (consoleBox.isFocused) {
                    consoleBox.ActivateInputField();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            consoleBox.ActivateInputField();
        }
    }

    public void InputText(string text) {
        // @FIXME: find a better way to handle history
        // if (commandHistory.Count >= maxHistory) {
        //     Destroy(commandHistory[0].textObject.gameObject);
        //     commandHistory.Remove(commandHistory[0]);
        // }

        Command newCommand = new Command();

        newCommand.text = text;
        GameObject newText = Instantiate(textObject, consolePanel.transform);

        newCommand.textObject = newText.GetComponent<TMP_Text>();
        newCommand.textObject.text = newCommand.text;

        commandHistory.Add(newCommand);
    }

// ----------------------------------------------------------------------------
//     - Commands -
// ----------------------------------------------------------------------------

    private void ClearCommand(string[] args) {
        foreach (Command command in commandHistory) {
            Destroy(command.textObject.gameObject);
        }

        commandHistory.Clear();
    }

    private void LevelCommand(string[] args) {
        try {
            string levelName = args[0];
            InputText($"Loading level '{levelName}'");
        } catch (Exception e) {
            InputText($"ERROR: '{e}'");
            InputText("ERROR: provide a level name");
        }
    }

    private void HelpCommand(string[] args) {
        InputText("Available Commands:");
        InputText("- clear");
        InputText("- level");
    }

}

// @FIXME: has nothing to do with the console commands
//         rename it! 
[System.Serializable]
public class Command {
    public string text;
    public TMP_Text textObject;
}
