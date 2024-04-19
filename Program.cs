using System.Text;
using System.Text.RegularExpressions;
using WatsonWebsocket;
using WebChatServer.Classes;

namespace WebChatServer
{
    internal class Program
    {
        static WatsonWsServer server;
        
        // default server values
        static string servername = "⠀";
        static string ip = "127.0.0.1";
        static int port = 8888;
        static char commandPrefix = '?';
        // end of default server values

        static List<User> userList = new List<User>();

        static void Main(string[] args)
        {
            ConfigServer();
            StartServer();
        }

        public static void ConfigServer()
        {
            Console.WriteLine("Loading config file...");

            Console.WriteLine("Checking if config file exists...");

            if (File.Exists("ServerConfig.txt"))
            {
                Console.WriteLine("Config file exists. Reading data...");

                string serverConfigString = File.ReadAllText("ServerConfig.txt");

                string[] configValues = serverConfigString.Split(';');


                // configValues = 0 - servername; 1 - ip; 2 - port; 3 - commandPrefix
                servername = configValues[0];
                ip = configValues[1];
                port = int.Parse(configValues[2]);
                commandPrefix = Char.Parse(configValues[3]);

                Console.WriteLine("Config loaded and ready to be used.");
            }
            else
            {
                Console.WriteLine("Config file doesn't exist. Would you like to make new config for server? Y/N");

                string? userInput = Console.ReadLine();

                if (userInput == null)
                {
                    userInput = "";
                }

                if (userInput == "Y" || userInput == "y")
                {
                    Console.Write("How would you like to name your server? ");
                    userInput = Console.ReadLine();

                    if (!(string.IsNullOrEmpty(userInput)))
                    {
                        servername = userInput;
                    }
                    else
                    {
                        Console.WriteLine("Wrong server name. Skipping and using default value.");
                    }

                    Console.Write("Server's IP address (default: 127.0.0.1): ");

                    userInput = Console.ReadLine();

                    userInput = userInput.Trim();

                    if (!(string.IsNullOrEmpty(userInput)))
                    {
                        ip = userInput;
                    }
                    else
                    {
                        Console.WriteLine("Wrong IP. Skipping and using default value.");
                    }

                    Console.Write("Server's port (default: 8888): ");
                    userInput = Console.ReadLine();

                    userInput = userInput.Trim();

                    if (!(string.IsNullOrEmpty(userInput)))
                    {
                        try
                        {
                            port = Convert.ToInt32(userInput);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Wrong port. Couldn't convert given port to int. Skipping and using default value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong port. Skipping and using default value.");
                    }

                    Console.Write("Server's commands prefix. (default: '?'): ");

                    userInput = Console.ReadLine();

                    userInput = userInput.Trim();

                    if (!(string.IsNullOrEmpty(userInput)))
                    {
                        try
                        {
                            commandPrefix = Convert.ToChar(userInput);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Wrong command prefix. Couldn't convert given port to int. Skipping and using default value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong command prefix. Skipping and using default value.");
                    }
                }
                else
                {
                    Console.WriteLine("Skipping config creation proccess. Using default values.");
                }

                File.WriteAllText("ServerConfig.txt", $"{servername};{ip};{port};{commandPrefix}");
            }
        }

        public static void StartServer()
        {

            Console.WriteLine("Starting WebChat server...");

            server = new WatsonWsServer(ip, port, false); // args: ip (string), port (int), ssh (bool)

            server.ClientConnected += ClientConnected;
            server.ClientDisconnected += ClientDisconnected;
            server.MessageReceived += MessageReceived;
            server.Start();
            Console.WriteLine($"WebChat server started! You can now connect to {ip}:{port}");
            Console.WriteLine("------ Press any key to stop server ------");
            Console.ReadKey();
            server.Stop();
        }

        static void ClientConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.Client.ToString());

            Guid connectedClientGuid = args.Client.Guid;

            userList.Add(new User(connectedClientGuid, "null", "Media/Images/profile.png"));

            SendMessageToSpecificClient(connectedClientGuid, "•ready");
            SendMessageToSpecificClient(connectedClientGuid, $"•servername•{servername}");
            SendUpdatedConnectedUsersList();
            SendOldChatMessages(args);
        }

        static void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.Client.ToString());

            Guid disconnectedClientGuid = args.Client.Guid;

            DeleteUserFromUserList(disconnectedClientGuid);
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            string receivedMessage = Encoding.UTF8.GetString(args.Data);

            Console.WriteLine("Message received from " + args.Client.ToString() + ": " + receivedMessage);

            if (!((receivedMessage == "") || (receivedMessage.Contains('•'))))
            {
                receivedMessage = PreventJSCodeInjection(receivedMessage);

                if (receivedMessage[0] == commandPrefix)
                {
                    Console.WriteLine("Executing command...");
                    ExecuteCommand(receivedMessage, args);
                }
                else
                {
                    SendMessageToEveryone(args.Client.Guid, receivedMessage);
                    SaveMessageToLog(receivedMessage, args.Client.Guid);
                }
            }
            else
            {
                Console.WriteLine("Illegal message detected. Skipping...");
            }
        }

        static string PreventJSCodeInjection(string _message)
        { 
            return Regex.Replace(_message, "<", "&lt;");
        }

        static void SendOldChatMessages(ConnectionEventArgs _receiverArgs)
        {
            if (File.Exists("ChatLog.txt"))
            {
                foreach (var line in File.ReadLines("ChatLog.txt"))
                {
                    if (!(string.IsNullOrEmpty(line)))
                    {
                        SendMessageToSpecificClient(_receiverArgs.Client.Guid, line);
                    }
                }
            }
        }

        static void SaveMessageToLog(string _message, Guid _userGuid)
        {
            User tempUserObj = null;

            foreach (var item in userList)
            {
                if (item.guid == _userGuid)
                {
                    tempUserObj = item;
                    break;
                }
            }

            if (tempUserObj != null)
            {
                File.AppendAllText("ChatLog.txt", _message + "•" + tempUserObj.name + "•" + tempUserObj.avatar + Environment.NewLine);
            }
        }

        static void DeleteUserFromUserList(Guid _guid)
        {

            userList.RemoveAll(user => user.guid == _guid);

            GC.Collect();

            /*if (flag)
            {
                Console.WriteLine("ERROR: Couldn't delete user with given guid.");
            }*/

            SendUpdatedConnectedUsersList();
        }

        static void ExecuteCommand(string _message, MessageReceivedEventArgs _senderArgs)
        {
            string[] splittedMessage = _message.Split(' ');

            switch (splittedMessage[0])
            {
                case "?avatar":
                    UpdateAvatar(_senderArgs.Client.Guid, splittedMessage[1]);
                    break;

                case "?name":
                    string name = "";

                    for (int i = 1; i < splittedMessage.Length; i++)
                    {
                        name += splittedMessage[i] + " ";
                    }
                    UpdateName(_senderArgs.Client.Guid, name);
                    SendUpdatedConnectedUsersList();
                    break;

                case "?help":
                    SendServerMessage(Guid.NewGuid(), "<b>List of commands:</b><br>- ?name [new name] - changes your name<br>- ?avatar [new avatar url] - changes your avatar<br>- ?help - displays list of available commands", true);
                    break;

                default:
                    Console.WriteLine("Wrong command syntax");
                    break;
            }

            if (splittedMessage[0] == "?avatar" || splittedMessage[0] == "?name")
            {
                SendUpdatedConnectedUsersList();
            }
        }

        static void UpdateAvatar(Guid _guid, string _newAvatarUrl)
        {
            bool flag = true;

            foreach (var item in userList)
            {
                if (item.guid == _guid)
                {
                    item.avatar = _newAvatarUrl;
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                Console.WriteLine("ERROR: Couldn't find any user with given guid while updating AVATAR.");
            }
        }

        static void UpdateName(Guid _guid, string _newName)
        {
            bool flag = true;

            foreach (var item in userList)
            {
                if (item.guid == _guid)
                {
                    item.name = _newName;
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                Console.WriteLine("ERROR: Couldn't find any user with given guid while updating NAME.");
            }
        }

        static void SendMessageToSpecificClient(Guid _clientGuid, string _message)
        {
            server.SendAsync(_clientGuid, _message);
        }

        static void SendServerMessage(Guid _receiverGuid, string _message, bool _toEveryone) // if message to everyone, set _clientGuid to Guid.NewGuid() or just anything else
        {
            if (_toEveryone)
            {
                foreach (var user in userList)
                {
                    server.SendAsync(user.guid, _message + "•" + "Server Bot" + "•" + "Media/Images/bot.png");
                }
            }
            else
            {
                server.SendAsync(_receiverGuid, _message + "•" + "Server Bot" + "•" + "Media/Images/bot.png");
            }
        }

        static void SendMessageToEveryone(Guid _senderGuid, string _message)
        {
            User tempUserObj = null;

            foreach (var item in userList)
            {
                if (item.guid == _senderGuid)
                {
                    tempUserObj = item;
                    break;
                }
            }
            if (tempUserObj != null)
            {
                foreach (var user in userList)
                {
                    server.SendAsync(user.guid, _message + "•" + tempUserObj.name + "•" + tempUserObj.avatar);
                }
            }
        }

        static void SendUpdatedConnectedUsersList()
        {
            string updatedList = "";

            // get updated list
            foreach (var user in userList)
            {
                updatedList += user.name + ";";
            }
    
            if(updatedList.Length > 0)
            {
                updatedList = updatedList.Remove(updatedList.Length - 1);
            }

            // send updated list
            foreach (var user in userList)
            {
                server.SendAsync(user.guid, "•updateUserList•" + updatedList);
            }
        }
    }
}