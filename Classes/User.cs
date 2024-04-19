using System;
namespace WebChatServer.Classes
{
    public class User
    {
        public Guid guid { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }

        public User(Guid guid, string name, string avatar)
        {
            this.guid = guid;
            this.name = name;
            this.avatar = avatar;
        }
    }
}
