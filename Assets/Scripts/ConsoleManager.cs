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
        Color info = new Color(147f / 255f, 161f / 255f, 161f / 255f);
        Color warning = new Color(181f / 255f, 137f / 255f, 0f);
        Color error = new Color(220f / 255f, 50f / 255f, 47f / 255f);
        Color debug = new Color(108f / 255f, 113f / 255f, 196f / 255f);
        Color userInput = new Color(1f, 1f, 1f);

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
        Info,       // (147, 161, 161) blue
        Warning,    // (181, 137, 0) yellow
        Error,      // (220, 50, 47) red
        Debug,      // (108, 113, 196) violet
        UserInput   // (255, 255, 255) white
    }
}
