﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC
{
    public interface IWhisperInputMod
    {
        void ProcessMessage(string sUsername, string sMessage);
    }
}