# 🌐 WebChat Server (Archive)
Version 1.0  
License: MIT

# IMPORTANT (PLEASE READ)
**Update 2025**: It’s an old one-time project made quickly for school purposes. It won’t receive any updates. The code is a development mess, it works but some of the solutions are at least questionable (though sufficient for a quick school project).
I’m planning to remake WebChat in the future once I have more time, as I’m currently focused on developing my game. This time, it will be cross-platform (web, mobile, desktop clients) and much more advanced.



## 📝 What is WebChat?
WebChat is an educational school project created by me. I wanted to build something cool involving sockets, connections, etc., and I did!

![WebChatClientPresentation](https://github.com/user-attachments/assets/6790c414-26cc-4a48-91b9-6378a798fa5b)


**REMEMBER THAT THIS PROJECT IS SOLELY FOR EDUCATIONAL SCHOOL PURPOSES.
IT IS UNOPTIMIZED AND NOT REALLY SUITED FOR FURTHER DEVELOPMENT.** 😓

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
- **SendMessageToEveryone**(Guid _senderGuid, string _message)
- **SendMessageToSpecificClient**(Guid _receiverGuid, string _message)
- **SendServerMessage**(Guid _receiverGuid, string _message, bool _toEveryone)
\
You can use those 3 functions to interact with clients' chat ^\
Also
- **ExecuteCommand**(string _message, MessageReceivedEventArgs _senderArgs)\
for executing commands (default syntax for starting command is '?'). Clients can execute commands from chat.

## 🔧 Known issues
There are a few known issues that I might fix in the future, but honestly, it's just an educational project.

**Issues:**
- Currently, the entire chat history is downloaded from the server, which can cause performance issues if there are a lot of messages.
- Channels are not working (well, this isn't really an issue; the channel system just hasn't been implemented yet, lol).
- Everything is in one file (each section - config, chat and user management events should be seperated).
- Config part is nested.
