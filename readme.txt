*** *** *** Summary *** *** *** 

This project is a C#/XNA application used for prototyping animations as well as running the actual system installed on the dome of the JellyFish12000 art car (www.jellyfish12000.com).

The code is as-is and you're free to do anything you want with it. If you have questions please send them directly to me at jeremy.carver@gmail.com

Also, if improvements/enhancements are made to this project, I would appreciate having a copy! Please send updates to me at jeremy.carver@gmail.com 

*** *** *** Prerequisites *** *** *** 

The project as it sits is built with Microsoft Visual C# 2010 and XNA 4.0. Download and install MSVC# 2010 and XNA 4.0. Once both are installed the project should build. There might also be a .NET requirement, but I'm not certain.


*** *** *** meaningful code *** *** *** 

(MainForm.cs)
Mostly a good place to start looking. There are some notes I've kept about the project near the top of the file and this is also where the bulk of the dispatching happens. Trace from here to see how everything is constructed.

(Dome.cs)
Main render and hardware update sections live here. Also contains a bunch of meaningful constants used throughout the project.

m_AttemptConnect = false; //Controls if the app tries to connect to the Jelly hardware.
m_RenderEnabled = true; //Controls if the app renders a preview to the main form.

(AnimationManager.cs) 
Manages the animation updates, blending, and queuing

(Animation.cs)
The base class for animations (subclasses live in the Animations folder)

(Blender.cs)
The base class for blenders -transitions between animations- (subclasses live in the Blenders folder)

(ColorManager.cs)
Handles loading color ramps and giving out colors for the animations. 

(DomeViewer.cs)
DomeViewer is a designer friendly control you can drop in any form and the Dome will render to it. 

(CoreWindow.cs)
The baseclass for DomeViewer that handles input, clearing and setting up the device for the render, and dispatching the render.