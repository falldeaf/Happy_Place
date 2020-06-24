# Happy_Place
VR Based Happy Place simulator for HTC Vive

![In-game screenshot](https://falldeaf.com/falldeaf-content/uploads/2020/06/vlcsnap-2020-06-23-20h25m52s458.png)

This is a Unity 2019.3.10f1 based project that puts you into a spooky forest scene with a few items to play with, like a flashlight, old telephone and the Clam Shell reader from the movie It Follows. The clamshell has a special functionality, however. It allow you to load up cartridges into it to give it special functionality, based on javascript programs dynamically interpreted at runtime using Jint, a C# based javascript engine.

Controls:
Side grips pick up objects.
Swipe up on the pad to open the clamshell when it's in your hand. Swipe down to close it.
Click in the center of the pad on the controller holding a cartridge when it's aligned to the slot in the back of the Clam to insert it.

A Javascript program hello world file would look like the following:
```javascript
//Set the user configurable LED (On the back of the cartidge)
led("purple")

//Write to the first screen (Text based)
write1("Hello world!")
//Write to second text screen if available
write2("Hello other world!")

//Receive action/input from the hardware
function action(a) {
  log("Saw the button/command: " + a)
}
```

And should be stored in the StreamingAssets/scripts folder.
