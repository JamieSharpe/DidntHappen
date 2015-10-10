# Didn'tHappen

If it wasn't documented, it didn't happen.

A simple program that checks for missing documentation for C# files in a given directory.

Also includes a .bat file which can be used as a prebuild script to fail the build process if no documentation is found.

# To Run

Compile the program.

Run the program with a directory as the first argument.

# PreBuild Script

Replace the .exe call in `PreBuild.bat` to where you compiled the `DidntHappen.exe`

Place `PreBuild.bat $(ProjectDir)` as a Pre-Build event script for your C# project.
