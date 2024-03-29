﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DidntHappen
{
    class Program
    {
        private const int ERROR_PATH_NOT_FOUND = 0x3;
        private const int ERROR_BAD_ARGUMENTS = 0xA0;

        /// <summary>
        /// Entry point of application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("                               If it's not documented it\n");
            sb.Append("██████╗ ██╗██████╗ ███╗   ██╗█╗████████╗    ██╗  ██╗ █████╗ ██████╗ ██████╗ ███████╗███╗   ██╗\n");
            sb.Append("██╔══██╗██║██╔══██╗████╗  ██║╚╝╚══██╔══╝    ██║  ██║██╔══██╗██╔══██╗██╔══██╗██╔════╝████╗  ██║\n");
            sb.Append("██║  ██║██║██║  ██║██╔██╗ ██║     ██║       ███████║███████║██████╔╝██████╔╝█████╗  ██╔██╗ ██║\n");
            sb.Append("██║  ██║██║██║  ██║██║╚██╗██║     ██║       ██╔══██║██╔══██║██╔═══╝ ██╔═══╝ ██╔══╝  ██║╚██╗██║\n");
            sb.Append("██████╔╝██║██████╔╝██║ ╚████║     ██║       ██║  ██║██║  ██║██║     ██║     ███████╗██║ ╚████║\n");
            sb.Append("╚═════╝ ╚═╝╚═════╝ ╚═╝  ╚═══╝     ╚═╝       ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝     ╚══════╝╚═╝  ╚═══╝\n");
            Console.WriteLine(sb.ToString());

            // Test if input arguments were supplied:
            if (args.Length == 0)
            {
                Console.WriteLine("No path was passed in to check.\nClosing application.");
                Environment.Exit(ERROR_BAD_ARGUMENTS);
            }

            if (!Directory.Exists(args[0]))
            {
                Console.Write("The path specified does not exist;\n {0}\nClosing application.", args[0]);
                Environment.Exit(ERROR_PATH_NOT_FOUND);
            }

            // Get the files
            string[] filePaths = new string[]{};

            try
            {
                filePaths = Directory.GetFiles(args[0], "*.cs", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to load the files.");
                Environment.Exit(ERROR_BAD_ARGUMENTS);
            }

            bool foundErrorInFolder = false;

            // Regular expressions
            const string rgxMethod = @"(^\s*(private|public|static)+.*[)]$)";
            const string rgxComment = @"^([ \t]*)([//]{2,})(.*)$";
            var r = new Regex(rgxMethod);
            var rr = new Regex(rgxComment);

            // Check if the file contains documentation
            foreach (var filePath in filePaths)
            {
                Console.WriteLine("Checking if {0} contains documentation...", filePath);

                using (StreamReader sr = new StreamReader(filePath))
                {
                    int lineNumber = 0;

                    bool foundError = false;

                    string prevLine = String.Empty;
                    string curLine = String.Empty;

                    while (true)
                    {
                        lineNumber++;
                        prevLine = curLine;
                        curLine = sr.ReadLine();
                        if (curLine == null)
                        {
                            break;
                        }

                        if (r.Match(curLine).Success && !(rr.IsMatch(prevLine)))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\tMissing documentation on line: {0}", lineNumber);
                            Console.WriteLine("\t\tPrevious Line:\t{0}\n\t\tCurrent Line:\t{1}", prevLine, curLine);
                            Console.ResetColor();
                            foundError = true;
                            foundErrorInFolder = true;
                        }
                    }

                    string preText = String.Empty;
                    if (!foundError)
                    {
                        preText = "No errors";
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        preText = "Errors";
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("\t{0} found in:  {1}", preText, filePath);
                    Console.ResetColor();
                }
            }
            
            if (foundErrorInFolder)
            {
                Environment.Exit(1);
            }
        }
    }
}
