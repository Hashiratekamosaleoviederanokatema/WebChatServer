# 🌐 WebChat Server
Version 1.0  
License: MIT

## 📝 What is WebChat?
WebChat is an educational project created by me. I wanted to build something cool involving sockets, connections, etc., and I did!

**REMEMBER THAT THIS PROJECT IS SOLELY FOR EDUCATIONAL PURPOSES.
IT IS UNOPTIMIZED, AND MANY AREAS COULD BENEFIT FROM BETTER SOLUTIONS.** 😓

## 🕵🏻 How do I use it?
It's really simple. In this repository, you'll find the client you'll use to connect to the server (the server can be found in the WebChatServer repository).

**Step by step (project files):**
- Download everything by clicking the green **"Code"** button, then download the ZIP file.
- Unzip it to any location you'd like.
- Run **WebChatServer/WebChatServer.sln**.
- And that's it. Happy coding! 🥳

**Step by step (server executable):**
- Download WebChatServerApp_1.0.zip from releases tab.
- Unzip it to any location you'd like.
- Run **WebChatServer.exe**.
- Configure server with auto configurator.
- And that's it. Happy server hosting! 🥳

## 💉 How do I modify server's code?
It's quite easy. Just download project files and run server project in Visual Studio. You might need to download WatsonWebSockets nuget (https://github.com/jchristn/WatsonWebsocket).\
You only have 3 events on the server for handling clients (yes, it's that simple):
- **ClientConnected**(object sender, ConnectionEventArgs args)
- **ClientDisconnected**(object sender, DisconnectionEventArgs args)
- **MessageReceived**(object sender, MessageReceivedEventArgs args)
You also have other functions like:
- **SendMessagetToEveryone**(Guid _senderGuid, string _message)
- **SendMessageToSpecificClient**(Guid _receiverGuid, string _message)
- **SendServerMessage**(Guid _receiverGuid, string _message, bool _toEveryone)
\
You can use those to interact with chat ^\
Also
- **ExecuteCommand**(string _message, MessageReceivedEventArgs _senderArgs)\
for executing commands (default syntax for starting command is '?'). Clients can execute commands from chat.

## 🔧 Known issues
There are a few known issues that I might fix in the future, but honestly, it's just an educational project.

**Issues:**
- Currently, the entire chat history is downloaded from the server, which can cause performance issues if there are a lot of messages.
- Channels are not working (well, this isn't really an issue; the channel system just hasn't been implemented yet, lol).
