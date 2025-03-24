/*
    FmodGuids.cs - FMOD Studio API

    Generated GUIDs for project 'GrizzlyGame.fspro'
*/

using System;
using System.Collections.Generic;

namespace Klang.Seed.Audio
{
    public class AudioEvent
    {
        public static readonly FMOD.GUID Enemy = new FMOD.GUID { Data1 = 624951414, Data2 = 1102676664, Data3 = 484741554, Data4 = -2073277004 };
        public static readonly FMOD.GUID PlayerAttack = new FMOD.GUID { Data1 = -982986684, Data2 = 1299884347, Data3 = 1040659360, Data4 = -1784076136 };
        public static readonly FMOD.GUID PlayerMovement = new FMOD.GUID { Data1 = -1804072002, Data2 = 1218128428, Data3 = -1924718700, Data4 = 1745613375 };
        public static readonly FMOD.GUID Test = new FMOD.GUID { Data1 = -9758484, Data2 = 1245021375, Data3 = 1395276450, Data4 = 767652684 };


        public static readonly Dictionary<string, FMOD.GUID> AudioEventNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"Enemy", Enemy}, {"PlayerAttack", PlayerAttack}, {"PlayerMovement", PlayerMovement}, {"Test", Test}, 
        };
    }

    public class AudioBus
    {
        public static readonly FMOD.GUID MasterBus = new FMOD.GUID { Data1 = -988115165, Data2 = 1228846850, Data3 = -1090998861, Data4 = 252002649 };
        public static readonly FMOD.GUID Reverb = new FMOD.GUID { Data1 = -1710510160, Data2 = 1140030815, Data3 = -632194644, Data4 = 2090800667 };


        public static readonly Dictionary<string, FMOD.GUID> AudioBusNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"MasterBus", MasterBus}, {"Reverb", Reverb}, 
        };
    }

    public class AudioBank
    {
        public static readonly FMOD.GUID Master = new FMOD.GUID { Data1 = -1450428026, Data2 = 1150626526, Data3 = -370190429, Data4 = -1375635118 };


        public static readonly Dictionary<string, FMOD.GUID> AudioBankNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"Master", Master}, 
        };
    }

}

