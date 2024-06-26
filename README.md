# DropBoxExercise/README.md

This app is intended to do the following:

1. Create a client that observes a directory for any changes.
2. The client then uploads the changes to a server.
3. The server then processes the changes and stores them in another target directory.

The app is written in C# and uses .NET 8.0.
The app has been written and tested on a Mac OS machine.

## How to run the app

1. Clone the repository.
2. Be sure to have .NET 8.0 installed.
3. Add appSettings.json files to both the client and server projects.

  ***In the case in which you don't have the appSettings.json file, the app will use the default values of Ip: 127.0.0.1 and port 8080***

4. An example of the appSettings.json file ( for both projects ) is shown below:

```json
{
  "Settings" : {
    "ServerIpAddress" : "127.0.0.1",
    "ServerPort" : 5000
  }
}
```

5. Run the server project with 1 argument - the target directory where the files will be stored. For example:
```bash
dotnet run --project DropBoxExercise/DropBoxExercise.Server.csproj /path/to/target/directory
```
6. Run the client project with 1 argument - the directory to observe. For example:
```bash
dotnet run --project DropBoxExercise/DropBoxExercise.Client.csproj /path/to/source/directory
```

7. Make changes to the directory being observed by the client. Enjoy :)

### Highlights

1. The app is built with minimal extra frameworks / libraries / packages:
    - Microsoft.Extensions.Configuration
    - Microsoft.Extensions.Configuration.Json
    - Moq ( for testing )
    - FluentAssertions ( for testing )

    Besides these, the app uses the built-in .NET libraries.

2. The app is built with a client-server architecture.
3. The app is built with a file watcher that observes a directory for changes.
4. The app is built with a client that uploads the changes to the server.
5. The app is built with a server that processes the changes and replicates them in a target directory.
6. The app can handle the following changes:
    - File created
    - File deleted
    - File renamed
    - File modified
    - Directory created
    - Directory deleted
    - Directory renamed
7. The app can handle the following edge cases: 
    - Copy-pasting a file
    - Copy-pasting a directory with files
    - Copy-pasting a directory with directories
    - Copy-pasting a directory with files and directories
    - Deleting a directory with files
    - Deleting a directory with directories
    - Deleting a directory with files and directories


***Please be sure to run both the client and server.***

***Does not work if monitored folder and destination folder are the same***

***Did not test the app over a network, just locally.***


Improvements:

1. Client is a bit tightly coupled. Could use some refactoring.
2. Add more error handling.
3. Create more interfaces for the classes. Use injection for the classes.
4. Add support for more OS systems. ( Windows / Linux )
5. Add support for network connections. ( Not tested )

Approximate time spent on the project: 12 - 16 hours.
