# Setting up Tucxbot:

## Required Programs:
Visual Studio 2017 (I am not sure if Newtonsoft.Json is currently supported in 2015, it is not in 2012). Community version is ok.

## Instructions: 
- You will need to fill the Settings.json file located in (Project Folder)/TwitchIRC/TwitchIRC/Data/Settings.json with the following:
	1. Twitch bot's name
	2. Twitch bot's OAuth key [including the "oauth:"]. You can click here: [OAuth Link](https://twitchapps.com/tmi/) to get your OAuth key

- Upon filling the Settings.json, you will need to compile the Twitch IRC project to generate the TwitchIRC.dll
- Now open the TwitchForm project and build the project, generating the exe.
- Upon generating the exe, move the TwitchIRC.dll & Data folder [located (Project Folder)/TwitchIRC/TwitchIRC/bin/Debug] and move it to the exe's folder [(Project Folder)/TucxbotForm/TucxbotForm/bin/Debug]
- Now you should be able to run TwitchForm, either through Visual Studio or through the exe.

## Notes:
If there are any issues building the project make sure the following is added as a reference:

### TwitchIRC:
- Newtonsoft.JSON (ie. JSON.Net) [Right click References -> Manage NuGet Packages]

### TwitchForm:
- Newtonsoft.JSON
- TwitchIRC.dll (You will have to browse for this)

--- 

# Setting up Mods:

## Instructions:
- Create a Folder called Mods in the folder containing the TwitchForm exe
- Place any DLLs from the separate projects in the folder.

## Notes:
- The Mod projects cannot reference TwitchForm or it will be a cyclical reference. However it must reference TwitchIRC.
- Mods must inherit from any of the interfaces in the TwitchIRC project (ie. IChannelInputMod, IWhisperInputMod, IChannelInputMod, etc)
- It is recommended your projects be a Class Library (.NET) DO NOT USE Class Library (Standard)