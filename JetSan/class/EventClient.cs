using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyTemplate
{
    class TEvent
    {
        public string MessageName;
        public Dictionary<string, string> EventData;// = new Dictionary<string, string>();

        public TEvent()
        {
            MessageName = "";
            EventData = new Dictionary<string, string>();
        }
    }

    class EventClient
    {
        static Dictionary<object, EventClient> dicEventClients = new Dictionary<object, EventClient>();
        //public event EventHandler OnEventHandler;
        //public delegate void MyHandler(object sender, EventArgs args);
        public delegate void MyHandler(string m_MessageName, TEvent m_EventData);
        public event MyHandler OnEventHandler;

        public EventClient(object m_Object)
        {
            if (dicEventClients.ContainsKey(m_Object)) return;

            dicEventClients.Add(m_Object, this);
        }

        private void doProcessMessage(TEvent m_Event)
        {
            foreach (KeyValuePair<object, EventClient> client in dicEventClients)
            {
                var OnReceiveMessage = client.Value.OnEventHandler;
                if (OnReceiveMessage != null)
                {
                    OnReceiveMessage(m_Event.MessageName, m_Event);
                }
            }
        }

        public void SendMessage(TEvent m_Event)
        {
            doProcessMessage(m_Event);
        }
    }
}
