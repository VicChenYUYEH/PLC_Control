using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyTemplate
{
    class ProxyMessage
    {
        public const string MSG_WRITE_LOG = "MSG_WRITE_LOG";
        public const string MSG_RECIPE_SAVE = "MSG_RECIPE_SAVE";
        public const string MSG_RECIPE_SET = "MSG_RECIPE_SET";
        public const string MSG_PARAMETER_SET = "MSG_PARAMETER_SET";
        public const string MSG_PLC_CONNECT = "MSG_PLC_CONNECT";
        public const string MSG_USER_REGISTER_CHANGED = "MSG_USER_REGISTER_CHANGED";
        public const string MSG_USER_LOGIN = "MSG_USER_LOGIN";
        public const string MSG_ALARM_OCCURE = "MSG_ALARM_OCCURE";
        public const string MSG_ALARM_RESET = "MSG_ALARM_RESET";

        public const string MSG_PROCESS_START = "MSG_PROCESS_START";
        public const string MSG_PROCESS_STOP = "MSG_PROCESS_STOP";

        public const string MSG_PROCESS_VACUUM = "MSG_PROCESS_VACUUM";
        public const string MSG_PROCESS_VACUUM_COMPLETE = "MSG_PROCESS_VACUUM_COMPLETE";
        public const string MSG_PROCESS_VENT = "MSG_PROCESS_VENT";

    }

    class PlcDeviceName
    {
        public const string PLC_IN_EMO = "X00028";
    }
}
