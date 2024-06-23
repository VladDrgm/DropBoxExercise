﻿using System.Net;
using BoxDropperServer.Domain.ClientHandler;
using BoxDropperServer.Domain.TcpListener;
using DropBoxExercise.CommonUtils;
using Microsoft.Extensions.Configuration;


ArgumentValidator.Validate(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: false)
    .Build();

var destinationDirectory = args[0];
var serverPort = int.Parse(configuration["Secrets:ServerPort"] ?? "8080");
var serverAddress = IPAddress.Parse(configuration["Secrets:ServerIpAddress"] ?? "127.0.0.1");

var listener = new TcpListenerWrapper(serverAddress, serverPort);
var clientHandler = new ClientHandler(destinationDirectory);
var server = new Server(listener, clientHandler);

server.RunServer();
