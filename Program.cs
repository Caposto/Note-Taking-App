using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

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
                case "cls":
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
            Console.WriteLine("Please Enter Note:\n");
            string input = Console.ReadLine();

            XmlWriterSettings NoteSettings = new XmlWriterSettings();

            NoteSettings.CheckCharacters = false;
            NoteSettings.ConformanceLevel = ConformanceLevel.Auto;
            NoteSettings.Indent = true;

            string fileName = DateTime.Now.ToString("dd-MM-yy") + ".xml";

            using (XmlWriter NewNote = XmlWriter.Create(NoteDirectory + fileName, NoteSettings))
            {

                NewNote.WriteStartDocument();
                NewNote.WriteStartElement("Note");
                NewNote.WriteElementString("body", input);
                NewNote.WriteEndElement();

                NewNote.Flush();
                NewNote.Close();
            }
        }

        // Allows user to edit specific note by specifing file name
        private static void EditNote()
        {
            Console.WriteLine("Enter File Name:\n");
            string fileName = Console.ReadLine().ToLower();

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

        private static void CommandsAvailable()
        {
            throw new NotImplementedException();
        }

        private static void Exit()
        {
            throw new NotImplementedException();
        }

        private static void NotesDirectory()
        {
            throw new NotImplementedException();
        }

        private static void ShowNotes()
        {
            throw new NotImplementedException();
        }

        private static void DeleteNote()
        {
            throw new NotImplementedException();
        }

        private static void ReadNote()
        {
            throw new NotImplementedException();
        }


    }
}
