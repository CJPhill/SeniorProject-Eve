using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public TMP_Text consoleOutput;  // Reference to the TextMeshPro console output.
    public TMP_InputField consoleInput;  // Reference to the TextMeshPro input field.

    private void Start()
    {
        // Hook up the input field's submit event.
        consoleInput.onSubmit.AddListener(ProcessCommand);
        consoleOutput.text = "Console Ready...\n";  // Initial message.
    }

    private void ProcessCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return;

        // Display the entered command on the console.
        AppendToConsole($"> {input}");

        // Process the command entered.
        string response = HandleCommand(input);
        AppendToConsole(response);

        // Clear the input field for the next command.
        consoleInput.text = "";
        consoleInput.ActivateInputField();  // Keeps the input field focused.
    }

    private string HandleCommand(string command)
    {
        // Basic commands example. You can extend this with more commands.
        switch (command.ToLower())
        {
            case "help":
                return "Available commands: help, clear, hello";
            case "clear":
                ClearConsole();
                return "Console cleared.";
            case "hello":
                return "Hello, user!";
            default:
                return $"Unknown command: {command}";
        }
    }

    private void AppendToConsole(string message)
    {
        consoleOutput.text += message + "\n";
    }

    private void ClearConsole()
    {
        consoleOutput.text = "";
    }
}
