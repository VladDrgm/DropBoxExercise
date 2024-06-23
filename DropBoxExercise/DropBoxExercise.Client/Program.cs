using System.Net;
using DropBoxExercise.Client.Domain;
using DropBoxExercise.CommonUtils;
using Microsoft.Extensions.Configuration;

ArgumentValidator.Validate(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appSettings.json", optional: true, reloadOnChange: false)
    .Build();

var sourceDirectory = args[0];

var serverPort = int.Parse(configuration["Settings:ServerPort"] ?? "8080");
var serverAddress = IPAddress.Parse(configuration["Settings:ServerIpAddress"] ?? "127.0.0.1");

var client = new Client(sourceDirectory);

var fileSyncObserver = new FileSyncObserver(serverAddress.ToString(), serverPort);
client.AddObserver(fileSyncObserver);

client.StartMonitoring();

Console.WriteLine("Press Enter to stop...");
Console.ReadLine();

client.StopMonitoring();

Console.WriteLine("Stopped.");