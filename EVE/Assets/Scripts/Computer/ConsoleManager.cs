using UnityEngine;
using TMPro;
using System.Collections;

public class ConsoleManager : MonoBehaviour
{
    public TMP_Text consoleOutput; // Reference to the TextMeshPro console output.
    public TMP_InputField consoleInput; // Reference to the TextMeshPro input field.

    // Mail system data structure
    private class Mail
    {
        public string Subject;
        public string Body;
        public bool IsRead;

        public Mail(string subject, string body)
        {
            Subject = subject;
            Body = body;
            IsRead = false; // Initialize mail as unread.
        }
    }

    private Mail[] mail = new Mail[]
    {
        new Mail("Welcome", "Hello and welcome to earth."),
        new Mail("Mission Brief", "Hello EVE. Please explore this new island."),
    };

    private void Start()
    {
        // Hook up the input field's submit event.
        consoleInput.onSubmit.AddListener(ProcessCommand);
        consoleOutput.text = "Console Ready...\n"; // Initial message.
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
        consoleInput.ActivateInputField(); // Keeps the input field focused.
    }

    private string HandleCommand(string command)
    {
        // Handle commands related to mail and basic operations.
        switch (command.ToLower())
        {
            case "help":
                return "Available commands: help, clear, hello, mail, mark read <number>, mark unread <number>";
            case "clear":
                ClearConsole();
                return "Console cleared.";
            case "hello":
                return "Hello, user!";
            case "quit":
                StartCoroutine(quitFunction());
                return "Closing terminal";
            case "mail":
                return DisplayMailList();
            default:
                if (command.StartsWith("read mail "))
                {
                    int index;
                    if (int.TryParse(command.Substring(10), out index))
                    {
                        return ReadMail(index);
                    }
                    else
                    {
                        return "Invalid mail number. Use 'read mail <number>'.";
                    }
                }
                else if (command.StartsWith("mark read "))
                {
                    return MarkMailStatus(command, true);
                }
                else if (command.StartsWith("mark unread "))
                {
                    return MarkMailStatus(command, false);
                }
                return $"Unknown command: {command}";
        }
    }

    private string DisplayMailList()
    {
        // Display the list of mail with read/unread status.
        string result = "Your mails:\n";
        for (int i = 0; i < mail.Length; i++)
        {
            string status = mail[i].IsRead ? "(Read)" : "(Unread)";
            result += $"{i + 1}. {mail[i].Subject} {status}\n";
        }

        return result + "Use 'read mail <number>' to read a specific message.";
    }

    IEnumerator quitFunction()
    {
        yield return new WaitForSeconds(5);
        this.transform.parent.gameObject.SetActive(false);
        ClearConsole();
    }

    private string ReadMail(int index)
    {
        // Validate index and retrieve the mail.
        if (index < 1 || index > mail.Length)
        {
            return "Invalid mail number.";
        }

        Mail selectedMail = mail[index - 1];
        selectedMail.IsRead = true; // Mark as read when accessed.

        return $"Subject: {selectedMail.Subject}\n\n{selectedMail.Body}";
    }

    private string MarkMailStatus(string command, bool markAsRead)
    {
        // Extract the mail index from the command.
        int index;
        if (int.TryParse(command.Split(' ')[2], out index))
        {
            if (index < 1 || index > mail.Length)
            {
                return "Invalid mail number.";
            }

            mail[index - 1].IsRead = markAsRead; // Update the read status.
            string status = markAsRead ? "read" : "unread";
            return $"Mail {index} marked as {status}.";
        }
        else
        {
            return "Invalid command. Use 'mark read <number>' or 'mark unread <number>'.";
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
