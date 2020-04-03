# Unity branching dialogue
A basic branching dialogue system for Unity (wip)

Made in Unity 2019.3.0f6

My attempt at elegantly solving the headache that is branching dialogue in Unity. Work in progress but already in a functional state. Now working on json importing to allow for external editing and better management of dialogue data.

UnityPackage Download: https://git.io/JvFb0

Implementation notes:
- Player needs to be tagged 'Player'
- Target NPC's need to be tagged 'NPC'
- DialogHelper -> on Player object
- DialogueData -> on target npc
- DialogueMe -> on target npc
- TextMeshPro is required for demo scene

For reference acquisition to work the UI elements need to be parented in a certain way or the 'ReferenceAcquire' method of DialogueMe.cs needs to be edited and adjusted. I recommend importing TextMeshPro and essentials before importing the package to avoid any weirdness.

Brief demo:

[![IMAGE ALT TEXT](http://img.youtube.com/vi/jMAN-cNCyBQ/0.jpg)](http://www.youtube.com/watch?v=jMAN-cNCyBQ "Demo")
