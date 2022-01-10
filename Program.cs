using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Diagnostics;

// Tutorial Link: https://www.thecodingguys.net/blog/csharp-create-command-line-notes-application
// XML Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter?view=net-6.0

namespace Note_Taking_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadCommand();
            Console.ReadLine();
        }

        // Directory/Folder that holds the notes created
        private static string NoteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Notes\";

        // Method that reads inputs from user: execute a certain "note action/method" based on user string
        private static void ReadCommand()
        {
            // cd to the proper file path
            Console.Write(Directory.GetDirectoryRoot(NoteDirectory));
            string Command = Console.ReadLine();

            switch (Command.ToLower())
            {
                case "new":
                    NewNote();
                    Main(null);
                    break;
                case "edit":
                    EditNote();
                    Main(null);
                    break;
                case "read":
                    ReadNote();
                    Main(null);
                    break;
                case "delete":
                    DeleteNote();
                    Main(null);
                    break;
                case "shownotes":
                    ShowNotes();
                    Main(null);
                    break;
                case "dir":
                    NotesDirectory();
                    Main(null);
                    break;
                case "close":
                    Console.Clear();
                    Main(null);
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    CommandsAvailable();
                    Main(null);
                    break;
            }
        }

        // Create a new note
        private static void NewNote()
        {
            // Get Note Title
            Console.WriteLine("Enter note title:\n");
            string inputTitle = Console.ReadLine();

            // Get Note Content
            Console.WriteLine("Enter note content\n");
            string inputBody = Console.ReadLine();

            XmlWriterSettings NoteSettings = new XmlWriterSettings();

            NoteSettings.CheckCharacters = false;
            NoteSettings.ConformanceLevel = ConformanceLevel.Auto;
            NoteSettings.Indent = true;

            string fileName = inputTitle + ".xml";

            using (XmlWriter NewNote = XmlWriter.Create(NoteDirectory + fileName, NoteSettings))
            {

                NewNote.WriteStartDocument();
                NewNote.WriteStartElement("Note");
                NewNote.WriteElementString("body", inputBody);
                NewNote.WriteEndElement();

                NewNote.Flush();
                NewNote.Close();
            }
        }

        // Allows user to edit specific note by specifing file name
        private static void EditNote()
        {
            Console.WriteLine("Enter File Name:\n");
            string fileName = Console.ReadLine().ToLower() + ".xml";

            // Check to see if file exists
            if (File.Exists(NoteDirectory + fileName))
            {
                XmlDocument doc = new XmlDocument();

                try
                {
                    doc.Load(NoteDirectory + fileName);
                    Console.Write(doc.SelectSingleNode("//body").InnerText);
                    string ReadInput = Console.ReadLine();

                    // Exit edit if user types "cancel"
                    if (ReadInput.ToLower() == "cancel")
                    {
                        Main(null);
                    }
                    // Write the user edit to the document
                    else
                    {
                        string newText = doc.SelectSingleNode("//body").InnerText = ReadInput;
                        doc.Save(NoteDirectory + fileName);
                    }
                }

                // If any error encountered display error message
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit note, following message occured: " + ex.Message);
                }

            }

            // If file does not exist
            else
            {
                Console.WriteLine("File not found\n");
            }
        }

        // Delete a note
        private static void DeleteNote()
        {
            Console.WriteLine("Enter File Name:\n");
            string fileName = Console.ReadLine().ToLower();

            // Check to see if Note exists
            if (File.Exists(NoteDirectory + fileName))
            {
                Console.WriteLine(Environment.NewLine + "Are you sure you wish to delete this file? Y/N\n");

                string Confirmation = Console.ReadLine().ToLower();

                if (Confirmation == "y")
                {
                    try
                    {
                        File.Delete(NoteDirectory + fileName);
                        Console.WriteLine("File has been deleted\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File not deleted following error occured: " + ex.Message);
                    }
                }

                else if (Confirmation == "n")
                {
                    Main(null);
                }

                // If input is neither "y" or "n"
                else
                {
                    Console.WriteLine("Invalid command\n");
                    DeleteNote();
                }
            }

            else
            {
                Console.WriteLine("File does not exist\n");
                DeleteNote();
            }
        }

        // Open a note for user to view
        private static void ReadNote()
        {
            Console.WriteLine("Enter File Name:\n");
            string fileName = Console.ReadLine() + ".xml";

            if (File.Exists(NoteDirectory + fileName))
            {
                XmlDocument doc = new XmlDocument();

                try
                {
                    doc.Load(NoteDirectory + fileName);
                    Console.Write(doc.SelectSingleNode("//body\n").InnerText);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Could not read note, following message occured: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File not found\n");
            }
        }

        // Print all possible commands
        private static void CommandsAvailable()
        {
            string[] commandList = new string[8];
            commandList[0] = "new - Create a new note";
            commandList[1] = "edit - Edit a note";
            commandList[2] = "read - Read a note";
            commandList[3] = "delete - Delete a note";
            commandList[4] = "shownotes - List all notes";
            commandList[5] = "dir - Open Note Directory";
            commandList[6] = "close - Clear Console";
            commandList[7] = "exit - Exit Application";

            for (int i = 0; i < commandList.Length; i++)
            {
                Console.WriteLine(commandList[i] + "\n");
            }
        }

        private static void ShowNotes()
        {
            string NoteLocation = NoteDirectory;

            DirectoryInfo Dir = new DirectoryInfo(NoteLocation);

            if (Directory.Exists(NoteLocation))
            {
                FileInfo[] NoteFiles = Dir.GetFiles("*.xml");

                if (NoteFiles.Count() != 0)
                {
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 2);
                    Console.WriteLine("+------------+");
                    foreach (var Note in NoteFiles)
                    {
                        Console.WriteLine("   " + Note.Name);
                    }
                    Console.WriteLine(Environment.NewLine);

                }
                else
                {
                    Console.WriteLine("No notes found.\n");
                }
            }
            else
            {
                Console.WriteLine(" Directory does not exist.....creating directory\n");
                Directory.CreateDirectory(NoteLocation);
                Console.WriteLine(" Directory: " + NoteLocation + " created successfully.\n");
            }
        }

        private static void Exit()
        {
            Environment.Exit(0);
        }

        private static void NotesDirectory()
        {
            Process.Start("explorer.exe", NoteDirectory);
        }
    }
}
