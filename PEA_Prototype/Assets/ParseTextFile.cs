using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class LineReader : MonoBehaviour
{
    string[] allLinesArray;
    string[] notesArray;
    string myFilePath, fileName;
    public List<string> listOfNotes = new();

    void Start()
    {
        fileName = "smFile.sm";
        myFilePath = Application.dataPath + "/" + fileName;
        //change file path of myFile when building :) - https://www.youtube.com/watch?v=N02o3EaNkXk
        ReadFromFile();
    }

    void Update()
    {
       
    }

    public void ReadFromFile()
    {
        allLinesArray = File.ReadAllLines(myFilePath);
        foreach (string line in allLinesArray)
        {
            int lineLength = line.Length;
            if (!line.Contains(":") && lineLength == 6)
            {
                listOfNotes.Add(line);
                print(line);
            }
            /*if (line.Contains("Song Title"))
            {
                set bpm as line thingy
            }*/
        }
        /*
        foreach (string line in owoArray)
        {
            print(line);lineLength == 6 && 
        }
        */

    }
}
