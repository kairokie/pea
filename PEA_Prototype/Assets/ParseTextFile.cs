using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Unity.VisualScripting;

public class LineReader : MonoBehaviour
{
    // Reading .sm file
    public string myFilePathSM, myFilePathMP3, fileName;
    List<string> listOfNotes = new(); 
    public float noteSpeed = 6.5f;
    private float noteTravelTime;
    public AudioSource music;

    string[] allLinesArray;
    string[] notesArray;

    string offsetStr;
    float bpm;
    string bpmStr;
    string musicName;
    float offset;

    // Playing the game
    public GameObject singleNotePrefab;
    public GameObject doubleNotePrefab;
    GameObject notePrefab;

    public float beatDuration;
    public float dspTimeSong;
    public float lastBeat = 0f;
    public double songposition = AudioSettings.dspTime;
    public int beatLine = 0;

    bool songStarted = false;
    bool firstNote = true;

    private IEnumerator syncBeat;

    void Start()
    {
        //Set-up
        myFilePathSM = Application.dataPath + "/Resources/Music/" + fileName + ".sm";
        myFilePathMP3 = Application.dataPath + "/Resources/Music/" + fileName + ".mp3";
        //change file path of myFile when building :) - https://www.youtube.com/watch?v=N02o3EaNkXk

        AudioClip resource = Resources.Load<AudioClip>("Music/" + fileName);

        noteTravelTime = (7.2f/6.5f);

        //Start the level
        ReadFromFile();
        StartSong(resource);
    }

    void Update()
    {
        //only after song starts

        if(songStarted)
        {
            if(offset>0)
            {
                songposition = (AudioSettings.dspTime - dspTimeSong + offset + noteTravelTime);
            }
            else
            {
                songposition = (AudioSettings.dspTime - dspTimeSong + offset + noteTravelTime);
            }
            
            // - offset;

            if (songposition > lastBeat + beatDuration)
            {
                lastBeat += beatDuration; 
                SpawnNote(beatLine);
                //StartCoroutine("BeatUpdate");
                beatLine += 1;
            }
        }
    }

    /*IEnumerator BeatUpdate()
    {
        if (firstNote)
        {
            yield return new WaitForSeconds(0f);
            firstNote = false;

            //float startDelay = (7.2f / 6.5f) / beatDuration;
            //beatLine += Convert.ToInt32(startDelay);
        }
        else
        {

            beatLine += 1;
        }
    }*/

    public void ReadFromFile()
    {
        allLinesArray = File.ReadAllLines(myFilePathSM);
        foreach (string line in allLinesArray)
        {
            // Take all lines with notes
            int lineLength = line.Length;
            if (!line.Contains(":") && lineLength == 6)
            {
                listOfNotes.Add(line);
            }

            // Take offset
            if (line.Contains("#OFFSET"))
            {
                offsetStr = line;
                offsetStr = offsetStr.Replace("#OFFSET:", string.Empty);
                offsetStr = offsetStr.Replace(";", string.Empty);
                float.TryParse(offsetStr, out float tempOffset);

                offset = tempOffset;

                /*if (tempOffset<0)
                {
                    tempOffset = -tempOffset;
                }*/


                //offset = tempOffset;
                print("Offset: " + offset);
            }

            // Take BPM
            if (line.Contains("#BPM"))
            {
                bpmStr = line;
                bpmStr = bpmStr.Substring(bpmStr.IndexOf('=') + 1);
                bpmStr = bpmStr.Replace(";", string.Empty);
                float.TryParse(bpmStr, out float bpm);
                print("BPM: " + bpm);

                beatDuration = 60 / bpm;
            }

            // Take Music Title
            if (line.Contains("#MUSIC"))
            {
                musicName = line;
                musicName = musicName.Substring(musicName.IndexOf(':') + 1);
                musicName = musicName.Replace(".mp3;", string.Empty);
                print("Song title: " + musicName);
            }
        }
    }

    void StartSong(AudioClip resource)
    {

        dspTimeSong = (float)AudioSettings.dspTime;
        music.PlayOneShot(resource);
        songStarted = true;

        //this line!!! idek man, it's close i swear
        //float startDelay = (7.2f / 6.5f)/ beatDuration;
        //beatLine += Convert.ToInt32(startDelay);
    }

    void SpawnNote(int _beatLine)
    {
        if(_beatLine < listOfNotes.Count)
        {
            string beat = listOfNotes[_beatLine];
            int notesOnLine = beat.Split('1').Length;

            if (notesOnLine == 2)
            {
                notePrefab = singleNotePrefab;
            }
            else
            {
                notePrefab = doubleNotePrefab;
            }

            print(beat);
            if (beat[2].ToString() == "1")
            {
                GameObject noteInstance = Instantiate(notePrefab, new Vector3(0, 4.3f, 0), Quaternion.identity);
                Variables.Object(noteInstance).Set("Direction_Right", -1);
            }
            if (beat[1].ToString() == "1")
            {
                GameObject noteInstance = Instantiate(notePrefab, new Vector3(0, 1, 0), Quaternion.identity);
                Variables.Object(noteInstance).Set("Direction_Right", -1);
            }
            if (beat[0].ToString() == "1")
            {
                GameObject noteInstance = Instantiate(notePrefab, new Vector3(0, -2.3f, 0), Quaternion.identity);
                Variables.Object(noteInstance).Set("Direction_Right", -1);
            }
            if (beat[3].ToString() == "1")
            {
                GameObject noteInstance = Instantiate(notePrefab, new Vector3(0, 4.3f, 0), Quaternion.identity);
                Variables.Object(noteInstance).Set("Direction_Right", 1);
            }
            if (beat[4].ToString() == "1")
            {
                GameObject noteInstance = Instantiate(notePrefab, new Vector3(0, 1, 0), Quaternion.identity);
                Variables.Object(noteInstance).Set("Direction_Right", 1);
            }
            if (beat[5].ToString() == "1")
            {
                GameObject noteInstance = Instantiate(notePrefab, new Vector3(0, -2.3f, 0), Quaternion.identity);
                Variables.Object(noteInstance).Set("Direction_Right", 1);
            }
            else if (beat == "000000")
            {
                print("no spawn");
            }
        }
        else
        {
            print("End of song");
        }
        
    }
}