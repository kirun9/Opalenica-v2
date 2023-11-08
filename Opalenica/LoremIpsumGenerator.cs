// Copyright (c) Krystian Pawełek from PKMK. All rights reserved.

namespace Opalenica;

public static class LoremIpsumGenerator
{
    public static string GenerateText(int length)
    {
        string loremIpsumText = File.ReadAllText("loremipsum.txt"); //Read all the text from the file
        int nextDotIndex = loremIpsumText.IndexOf('.', length); //Find the next dot position
        int toIndex = nextDotIndex + 1; //Index of the next dot + 1
        if (toIndex > loremIpsumText.Length) //Check if the result exceeds the string length
            toIndex = loremIpsumText.Length;
        string result = loremIpsumText[..toIndex]; //Substring from 0 to toIndex
        return result;
    }
}