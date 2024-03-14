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

    // @TODO: store commands in a hash table
    private Dictionary<string, Action<string[]>> commands;

    public ConsoleManager () {
        commands = new Dictionary<string, Action<string[]>>();

        RegisterCommand("level", LevelCommand);
        RegisterCommand("clear", ClearCommand);
        RegisterCommand("help", HelpCommand);
        RegisterCommand("example_debug", ExampleDebugCommand);
        RegisterCommand("example_warning", ExampleWarningCommand);
    }

    public void RunCommand(string input) {
        InputText(input, Command.Type.UserInput);
        string[] parts = input.Split(' ');
        string command = parts[0];
        string[] arguments = parts.Length > 1 ? parts[1..] : new string[0];
        Array.Copy(parts, 1, arguments, 0, arguments.Length);

        if (commands.ContainsKey(command)) {
            commands[command](arguments);
        } else {
            InputText($"ERROR: Unknown command `{command}`", Command.Type.Error);
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
        consoleBox.ActivateInputField();
        if (consoleBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                RunCommand(consoleBox.text);
                consoleBox.text = "";
            }
        }
    }

    // @TODO: rename PrintText / OutputText
    public void InputText(string text, Command.Type type) {
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
        newCommand.textObject.color = Colorize(type);

        commandHistory.Add(newCommand);
    }

    Color Colorize(Command.Type type) {
        Color info = Color.white;
        Color warning = new Color(0.902f, 0.784f, 0.745f, 1f);
        Color error = Color.yellow;
        Color debug = Color.red;
        Color userInput = Color.green;

        Color color = info;

        switch(type) {
            case Command.Type.Warning:
                color = warning;
                break;
            case Command.Type.Error:
                color = error;
                break;
            case Command.Type.Debug:
                color = debug;
                break;
            case Command.Type.UserInput:
                color = userInput;
                break;
        }

        return color;
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
            InputText($"Loading level '{levelName}'", Command.Type.Info);
        } catch (Exception e) {
            InputText($"ERROR: {e}", Command.Type.Error);
            InputText("ERROR: Provide a level name", Command.Type.Error);
        }
    }

    private void HelpCommand(string[] args) {
        InputText("Available Commands:", Command.Type.Info);
        InputText("- clear", Command.Type.Info);
        InputText("- level", Command.Type.Info);
    }

    private void ExampleDebugCommand(string[] args) {
        InputText("This is a debug output", Command.Type.Debug);
        InputText("This is a debug output", Command.Type.Debug);
        InputText("This is a debug output", Command.Type.Debug);
        InputText("This is a debug output", Command.Type.Debug);
        InputText("This is a debug output", Command.Type.Debug);
        InputText("This is a debug output", Command.Type.Debug);
        InputText("This is a debug output", Command.Type.Debug);
    }

    private void ExampleWarningCommand(string[] args) {
        InputText("This is a warning output", Command.Type.Warning);
        InputText("This is a warning output", Command.Type.Warning);
        InputText("This is a warning output", Command.Type.Warning);
        InputText("This is a warning output", Command.Type.Warning);
        InputText("This is a warning output", Command.Type.Warning);
        InputText("This is a warning output", Command.Type.Warning);
        InputText("This is a warning output", Command.Type.Warning);
    }
}

// @FIXME: has nothing to do with the console commands
//         rename it! maybe change it to Message 

// @FIXME: naming conventions!
[System.Serializable]
public class Command {
    public string text;
    public TMP_Text textObject;
    public Type type;

    public enum Type
    {
        Info,
        Warning,
        Error,
        Debug,
        UserInput
    }
}
