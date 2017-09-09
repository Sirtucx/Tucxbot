using System.Text.RegularExpressions;

namespace Twitch_Websocket
{
    static class IRCParser
    {
        public static string GetMessageType(string sIRCRaw, string sChannel)
        {
            if (sIRCRaw.Contains(" "))
            {
                bool bIsChannelMessage = false;
                string[] sMessageSplit = sIRCRaw.Split(' ');
                foreach (string word in sMessageSplit)
                {
                    if (word[0] == '#')
                    {
                        if (word == $"#{sChannel}")
                        {
                            bIsChannelMessage = true;
                        }
                    }
                }

                if (bIsChannelMessage)
                {
                    string[] sChannelSplitter = Regex.Split(sIRCRaw, $" #{sChannel}");
                    string[] sContentSplitter = sChannelSplitter[0].Split(' ');

                    var readType = sContentSplitter[sContentSplitter.Length - 1];
                    return readType;
                }
                else
                {
                    if (sMessageSplit.Length > 1 && sMessageSplit[1] == "NOTICE")
                    {
                        return "NOTICE";
                    }
                    else if (sMessageSplit.Length > 2)
                    {
                        if (sMessageSplit[1] == "WHISPER")
                        {
                            string[] sUserSplit = sMessageSplit[2].Split(':');
                            if (sUserSplit[0] == TwitchClient.GetInstance().Credentials.TwitchUsername.ToLower())
                            {
                                return "WHISPER";
                            }
                        }
                        else if (sMessageSplit[2] == "WHISPER")
                        {
                            string[] sUserSplit = sMessageSplit[3].Split(':');
                            if (sUserSplit[0] == TwitchClient.GetInstance().Credentials.TwitchUsername.ToLower())
                            {
                                return "WHISPER";
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static string GetTwitchTagsValue(string sIRCRaw, string sKey)
        {
            string[] sParts = sIRCRaw.Split(';');
            foreach (string part in sParts)
            {
                if (part.Contains($"{sKey}="))
                {
                    string[] sKeyValue = part.Split('=');
                    return sKeyValue[1];
                }
            }
            return null;
        }
    }
}
