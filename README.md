# 🌐 Webchat Server
Version 1.0  
License: MIT

## 📝 What is WebChat?
WebChat is an educational project created by me. I wanted to create something cool involving sockets, connections, etc., and so I did.

**REMEMBER THAT THIS PROJECT IS SOLELY EDUCATIONAL.  
IT IS UNOPTIMIZED, AND MANY THINGS DESERVE BETTER SOLUTIONS. 😓**

## 🕵🏻 How do I use it?
It's really simple. In this repository, you can find the server which you will be using to connect to handle the clients (the client can be found in the WebChatClient repository).

**Step by step (project files):**
- Download everything by clicking the green code button and then downloading the zip.
- Unzip it anywhere you would like.
- Run **WebChatServer/WebChatServer.sln**.
- And that's it. Happy coding! 🥳

**Step by step (server executable):**
- Download WebChatServerApp_1.0.zip from releases tab.
- Unzip it anywhere you would like.
- Run **WebChatServer.exe**.
- Configure server with auto configurator.
- And that's it. Happy hosting! 🥳

## 💉 How do I modify server's code?
It's quite easy. Just download project files and run server project in Visual Studio. You might need to download WatsonWebSockets nuget? (https://github.com/jchristn/WatsonWebsocket).\
You have really only 3 events on the server for handling clients:
- ClientConnected(object sender, ConnectionEventArgs args)
- ClientDisconnected(object sender, DisconnectionEventArgs args)
- MessageReceived(object sender, MessageReceivedEventArgs args)
You also have other functions like:
- SendMessagetToEveryone(Guid _senderGuid, string _message)
- SendMessageToSpecificClient(Guid _receiverGuid, string _message)
- SendServerMessage(Guid _receiverGuid, string _message, bool _toEveryone)
You can use those to interact with chat ^\
Also
- ExecuteCommand(string _message, MessageReceivedEventArgs _senderArgs)\
for executing commands (default syntax for starting command is '?'). Clients can execute commands from chat.

## 🔧 Known issues
There are a few known issues that I might fix in the future, but to be honest, it's just an educational project.

**Issues:**
- As of now, the whole chat history is downloaded from the server. It can cause performance issues when there are a lot of messages on the server.
- Channels are not working (well, it's not a real issue; the channel system is just not implemented lol).
- The client website is very unresponsive. I didn't design it to be responsive, so it will look bad on mobile and on small windows in general (I should've used Bootstrap).
- Possible very small memory leak on the server? Probably caused by sockets not clearing themselves (changing library to hadnle connections can help?)
