using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ALO.WebSite
{
    public class MessageHub : Hub
    {

        public void senmessage(string LOGIN, string MENSAJE,string SISTEMA)
        {
            this.Clients.All.receivenotification(LOGIN, MENSAJE,SISTEMA);
        }

        public string receivenotification(string Message)
        {
            return Message;
        }

    }
}