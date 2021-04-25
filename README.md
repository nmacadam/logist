# logist
Logist is a proof of concept extension/replacement for Unity's built in logging system.  It works by capturing log data at runtime via its own `Logist.Log()` command and by capturing data from Unity's native `Debug.Log()` & friends.  These logs then get exported for use with the [Logist web-app](https://nmacadam.github.io/logist-web/)

![screenshot](https://raw.githubusercontent.com/nmacadam/logist/master/images/logistScreenshot.PNG)

## why logist
Unity's native debug logging system is totally useable for development if you just want to get a few updates from your code every once in a while, but if you want to generate a full log of what has occurred in game mode session or in a build, you'll quickly end up with a dense, hard to read log file or lots of unfiltered `Debug.Log` messages that carry with them a decent performance penalty.  
The goal of Logist is to make logging a bit easier by generating an output log file to be viewed with the Logist web-application.  Logist also provides additional utilities not found in Unity's debug logging system like log categories, which can be filtered in the web-app.

## roadmap
- Provide utilities to integrate Logist into Unity's debug system, rather than just the other way around
- Allow users to add custom info to logs
- Allow users to attach images to logs
- Allow for serializing log content as generated instead of at application shutdown
- Support smaller serialization formats
- Add customizable categories
