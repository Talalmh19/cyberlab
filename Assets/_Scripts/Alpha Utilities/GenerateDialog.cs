using System.Collections.Generic;
using UnityEngine;

public static class GenerateDialog
{
    private static readonly List<string> dialogStrings = new()
    {
        "Ok Officer, you can inspect me.",
        "Sure, I have nothing to hide.",
        "Feel free to check, Officer.",
        "Go ahead, I’m not hiding anything.",
        "Alright, you can inspect me.",
        "Fine, take a look.",
        "Of course, Officer, do your job.",
        "No problem, check away.",
        "Alright, I trust you, Officer.",
        "I understand, please go ahead.",
        "Absolutely, I’ll cooperate.",
        "Sure thing, I’m clean.",
        "Okay, let’s get this done.",
        "Sure, inspect me if you must.",
        "Go on, Officer, I’m innocent.",
        "I’ve got nothing to worry about, go ahead.",
        "Yes, Officer, check if you need to.",
        "Do what you have to do.",
        "Alright, let’s make it quick.",
        "It’s fine, check as you need.",
        "Yes, Officer, I’m okay with it.",
        "Sure, I have nothing suspicious.",
        "Fine by me, check as you like.",
        "No issues, Officer. Do it.",
        "You’re just doing your job, inspect away.",
        "I’m clean. Go ahead.",
        "Sure, let’s get this over with.",
        "Ok, I understand. Check me.",
        "Alright, Officer, do your thing.",
        "Fine, check whatever you need.",
        "Yes, you can search me.",
        "Sure, I have no objections.",
        "Go ahead, Officer, nothing to hide.",
        "Yes, you’re welcome to inspect me.",
        "Alright, Officer, proceed.",
        "Yes, I’ll cooperate fully.",
        "I have no issues, check away.",
        "Fine, do what you need to.",
        "Sure, I understand the procedure.",
        "You can inspect me, Officer.",
        "Go ahead, nothing suspicious here.",
        "I’m okay with it, go ahead.",
        "Absolutely, you’re free to check.",
        "Yes, it’s all fine by me.",
        "Do your duty, Officer. I’ll comply.",
        "Go on, I have no problems with it.",
        "Yes, Officer, proceed with the inspection.",
        "Sure thing, do what you need.",
        "Alright, make it quick, please.",
        "No worries, I’ll cooperate."
    };

    public static string GetRandomDialog => dialogStrings[Random.Range(0, dialogStrings.Count)];
}
