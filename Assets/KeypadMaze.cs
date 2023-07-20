using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class KeypadMaze : MonoBehaviour {
    public int maze;
    int mazeSize = 6;
    int mazeCount = 9;
    public KMBombModule module;
    public KMBombInfo info;
    public KMAudio sfx;
    public int[] coord;
    public int[] yellow = {0, 0, 0, 0, 0};
    public char pos;
    public KMSelectable[] keypad;
    public KMSelectable[] directional;
    public GameObject[] walls;
    public TextMesh MazeDisp;
    public TextMesh[] Inputs;
    public int[] usersanswer = {0, 0};
    public int expanswer;
    public bool active;
    public bool solved;
    public int[][] order = { new[]{2, 2, 3, 4, 1, 1, 4, 1, 3, 3, 3, 1, 1, 2, 4, 3, 2, 2,
        4, 2, 2, 4, 1, 3, 3, 3, 4, 2, 4, 1, 3, 4, 4, 1, 1, 2},
    new[]{4, 2, 4, 4, 3, 4, 3, 2, 4, 1, 1, 2, 3, 1, 4, 3, 1, 1,
    1, 4, 1, 3, 2, 4, 2, 4, 3, 2, 2, 3, 1, 3, 2, 2, 1, 3},
    new[]{4, 3, 2, 1, 1, 2, 3, 1, 2, 3, 2, 3, 2, 4, 4, 1, 1, 4,
    4, 1, 4, 1, 1, 4, 1, 4, 3, 3, 4, 2, 2, 2, 3, 2, 3, 3},
    new[]{3, 2, 4, 3, 3, 2, 1, 4, 3, 1, 4, 1, 4, 4, 3, 3, 2, 2,
    4, 3, 1, 1, 2, 1, 2, 4, 1, 2, 4, 2, 2, 3, 1, 3, 1, 4},
    new[]{3, 2, 2, 1, 3, 2, 4, 4, 2, 1, 3, 1, 4, 3, 3, 4, 4, 1,
    1, 1, 3, 3, 2, 3, 1, 4, 4, 2, 3, 2, 2, 1, 1, 2, 4, 4},
    new[]{3, 3, 3, 2, 4, 4, 3, 2, 4, 1, 1, 4, 3, 4, 1, 3, 2, 1,
    2, 4, 4, 1, 3, 1, 2, 2, 1, 2, 1, 3, 3, 2, 4, 1, 2, 4},
    new[]{3, 2, 4, 4, 1, 2, 1, 1, 4, 3, 1, 1, 1, 2, 3, 4, 3, 2,
    3, 4, 3, 2, 2, 4, 2, 2, 1, 4, 4, 1, 4, 3, 3, 2, 3, 1},
    new[]{3, 2, 3, 2, 3, 1, 3, 2, 2, 4, 1, 2, 1, 3, 3, 4, 4, 1,
    4, 4, 1, 1, 4, 1, 2, 3, 1, 4, 2, 2, 4, 1, 3, 3, 4, 2},
    new[]{1, 3, 1, 4, 3, 1, 2, 1, 4, 3, 3, 2, 3, 1, 3, 2, 4, 2,
    1, 1, 3, 1, 4, 4, 4, 4, 3, 2, 4, 2, 1, 3, 4, 2, 2, 2} };
    /* 0 - no walls, 1 - up wall, 2 - right wall, 3 - down wall, 4 - left wall
    5 - up right walls, 6 - down right walls, 7 - down left walls, 8 - up left walls,
    9 - up down walls, a - right left walls, b - left down right walls, c - up left down walls,
    d - up left right walls*/
    public char[][] mazes = { new[] {'8', 'e', '8', '1', '1', 'e','7', '9', '2', 'a', '7', '5', 'd', '8', '6', 'a', '8', '6', '7',
        '2', '8', '6', '7', '5', '8', '6', '7', '5', 'd', 'a', '7', '9', 'e', 'b', '7', '6'},
    new[]{'8', '9', '1', '1', '1', '5','a', '8', '6', 'a', 'a', 'a', 'a', '7', 'e', 'a', 'a', 'a',
        'a', '8', '1', '2', 'a', 'b', 'a', 'a', 'a', 'b', '7', '5', 'b', 'b', 'b', 'c', '9', '6'},
    new[]{'8', '9', '5', '8', '5', 'd', '4', '5', '7', '6', '4', '2', 'a', '7', '5', 'd', 'a', 'a',
        '7', 'e', 'a', '7', '6', 'a', '8', '9', '3', '5', '8', '6', '7', '9', 'e', 'b', '7', 'e'},
    new[]{'8', '5', '8', '9', '5', 'd', 'a', 'a', '7', 'e', '4', '6', 'a', '7', '9', '5', 'a', 'd',
        'a', '8', '9', '6', '7', '2', 'a', '7', '9', '9', '9', '2', '7', '9', '9', 'e', 'c', '6'},
    new[]{'8', '9', '9', '9', '5', 'd', '4', '9', '9', '5', '7', '6', 'a', 'd', '8', '3', '9', '5',
        '7', '6', '4', '9', '5', 'b', '8', '1', '3', 'e', '7', '5', 'b', '7', 'e', 'c', '9', '6'},
    new[]{'8', '9', '9', '5', '8', '5', 'a', 'd', '8', '6', 'a', 'a', '4', '6', '7', '5', 'a', 'a',
        '7', '9', '5', '7', '6', 'a', '8', '9', '6', '8', '5', 'a', '7', '9', 'e', 'b', '7', '6'},
    new[]{'8', '9', '9', '9', '9', '5', 'a', '8', '9', '5', '8', '6', 'b', '7', '5', 'a', '7', '5',
        '8', '9', '6', '7', '9', '6', 'a', '8', '1', '9', '1', '5', '7', '6', 'b', 'c', '6', 'b'},
    new[]{'c', '5', '8', '5', '8', '5', '8', '0', '6', '7', '2', 'a', 'a', 'a', 'c', '9', '6', 'b',
        'a', '7', '9', '9', '9', '5', '4', '9', '5', 'c', '5', 'a', '7', 'e', '7', 'e', '7', '6'},
    new[]{'8', '9', 'e', '8', '9', '5', '4', '9', '9', '3', 'e', 'a', 'b', '8', '9', '9', '9', '2',
        '8', '2', '8', '5', '8', '6', 'a', 'a', 'a', 'a', '7', '5', 'b', '7', '6', 'b', 'c', '6', }};
    public int[] dispNumbers = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,};
    private static int _moduleIDCounter = 1;
    private int _moduleID;
    void Awake()
    {
        maze = Random.Range(0, mazeCount);
        while (yellow[4] != 1)
        {
            yellow[0] = Random.Range(0, mazeSize * mazeSize);
            yellow[4] = order[maze][yellow[0]];
        }
        while (yellow[4] != 2)
        {
            yellow[1] = Random.Range(0, mazeSize * mazeSize);
            yellow[4] = order[maze][yellow[1]];
        }
        while (yellow[4] != 3)
        {
            yellow[2] = Random.Range(0, mazeSize * mazeSize);
            yellow[4] = order[maze][yellow[2]];
        }
        while (yellow[4] != 4)
        {
            yellow[3] = Random.Range(0, mazeSize*mazeSize);
            yellow[4] = order[maze][yellow[3]];
        }
        module.OnActivate += delegate
        {
            _moduleID = _moduleIDCounter++;
            for (int i = 0; i < mazeSize*mazeSize; i++)
            {
                dispNumbers[i] = Random.Range(0, 10);
            }
            active = true;
            coord[0] = Random.Range(0, mazeSize);
            coord[1] = Random.Range(0, mazeSize);
            pos = mazes[maze][coord[1] * mazeSize + coord[0]];
            wallchange();
            expanswer = dispNumbers[yellow[0]] * 1000 + dispNumbers[yellow[1]] * 100 + dispNumbers[yellow[2]] * 10 + dispNumbers[yellow[3]];
            Debug.LogFormat("[Keypad Maze #{0}] You are in maze {1}", _moduleID, maze+1);
            Debug.LogFormat("[Keypad Maze #{0}] Coordinates are counted in ''row, column'' format, with top left being 0, 0.", _moduleID);
            Debug.LogFormat("[Keypad Maze #{0}] Initial coordintaes are: {1}, {2}.", _moduleID, coord[1], coord[0]);
            Debug.LogFormat("[Keypad Maze #{8}] Yellow cells are: {1}, {0}; {3}, {2}; {5}, {4}; {7}, {6}.", yellow[0] % mazeSize, (yellow[0]-yellow[0] % mazeSize) / mazeSize, yellow[1] % mazeSize, (yellow[1] - yellow[1] % mazeSize) / mazeSize, yellow[2] % mazeSize, (yellow[2] - yellow[2] % mazeSize) / mazeSize, yellow[3] % mazeSize, (yellow[3] - yellow[3] % mazeSize) / mazeSize, _moduleID);
            Debug.LogFormat("[Keypad Maze #{0}] Correct answer is {1}", _moduleID, expanswer.ToString().PadLeft(4, '0'));
        };
    }
	// Use this for initialization
	void Start () {
        DirecPress(0);
        DirecPress(1);
        DirecPress(2);
        DirecPress(3);
        NumberPress(0);
        NumberPress(1);
        NumberPress(2);
        NumberPress(3);
        NumberPress(4);
        NumberPress(5);
        NumberPress(6);
        NumberPress(7);
        NumberPress(8);
        keypad[9].OnInteract += delegate { if (!active || solved) { return false; } inputcheck('c'); sfx.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); GetComponent<KMSelectable>().AddInteractionPunch(0.1f); return false; };
        keypad[10].OnInteract += delegate { if (!active || solved || usersanswer[1] > 3) { return false; } input(0); sfx.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); GetComponent<KMSelectable>().AddInteractionPunch(0.1f); return false; };
        keypad[11].OnInteract += delegate { if (!active || solved || usersanswer[1] <= 3) { return false; } inputcheck('s'); sfx.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); GetComponent<KMSelectable>().AddInteractionPunch(0.1f); return false; };
    }

    void DirecPress (int x)
    {
        directional[x].OnInteract += delegate { if (!active || solved || !movecheck(x)) { return false; } GetComponent<KMSelectable>().AddInteractionPunch(0.1f); sfx.PlaySoundAtTransform("Moved", transform); return false; };
    }

    void NumberPress (int x)
    {
        keypad[x].OnInteract += delegate { if (!active || solved || usersanswer[1] > 3) { return false; } input(x+1); sfx.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); GetComponent<KMSelectable>().AddInteractionPunch(0.1f); return false; };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    bool movecheck(int c)
    {
        switch (c)
        {
            case 0:
                if (pos.EqualsAny('1', '5', '8', '9', 'c', 'd', 'e'))
                {
                    return false;
                }
                else
                {
                    coord[1] -= 1;
                    while (coord[1] < 0)
                    {
                        coord[1] += mazeSize;
                    }
                    pos = mazes[maze][coord[1] * mazeSize + coord[0]];
                    wallchange();
                }
                break;
            case 1:
                if (pos.EqualsAny('2', '5', '6', 'a', 'b', 'd', 'e'))
                {
                    return false;
                }
                else
                {
                    coord[0] += 1;
                    coord[0] = coord[0] % mazeSize;
                    pos = mazes[maze][coord[1] * mazeSize + coord[0]];
                    wallchange();
                }
                break;
            case 2:
                if (pos.EqualsAny('4', '7', '8', 'a', 'b', 'c', 'd'))
                {
                    return false;
                }
                else
                {
                    coord[0] -= 1;
                    while (coord[0] < 0)
                    {
                        coord[0] += mazeSize;
                    }
                    pos = mazes[maze][coord[1] * mazeSize + coord[0]];
                    wallchange();
                }
                break;
            case 3:
                if (pos.EqualsAny('3', '6', '7', '9', 'b', 'c', 'e'))
                {
                    return false;
                }
                else
                {
                    coord[1] += 1;
                    coord[1] = coord[1] % mazeSize;
                    pos = mazes[maze][coord[1] * mazeSize + coord[0]];
                    wallchange();
                }
                break;
        }
        return true;
    }
    void wallchange()
    {
        switch(pos)
        {
            case '0':
                foreach(GameObject wall in walls)
                {
                    wall.SetActive(false);
                }
            break;
            case '1':
                walls[0].SetActive(true);
                for (int i=1; i<4; i++)
                {
                    walls[i].SetActive(false);
                }
            break;
            case '2':
                foreach (GameObject wall in walls)
                {
                    wall.SetActive(false);
                }
                walls[1].SetActive(true);
                break;
            case '3':
                foreach (GameObject wall in walls)
                {
                    wall.SetActive(false);
                }
                walls[2].SetActive(true);
                break;
            case '4':
                foreach (GameObject wall in walls)
                {
                    wall.SetActive(false);
                }
                walls[3].SetActive(true);
            break;
            case '5':
                walls[0].SetActive(true);
                walls[1].SetActive(true);
                walls[2].SetActive(false);
                walls[3].SetActive(false);
            break;
            case '6':
                walls[0].SetActive(false);
                walls[1].SetActive(true);
                walls[2].SetActive(true);
                walls[3].SetActive(false);
            break;
            case '7':
                walls[0].SetActive(false);
                walls[1].SetActive(false);
                walls[2].SetActive(true);
                walls[3].SetActive(true);
            break;
            case '8':
                walls[0].SetActive(true);
                walls[1].SetActive(false);
                walls[2].SetActive(false);
                walls[3].SetActive(true);
            break;
            case '9':
                walls[0].SetActive(true);
                walls[1].SetActive(false);
                walls[2].SetActive(true);
                walls[3].SetActive(false);
                break;
            case 'a':
                walls[1].SetActive(true);
                walls[0].SetActive(false);
                walls[3].SetActive(true);
                walls[2].SetActive(false);
                break;
            case 'b':
                walls[0].SetActive(false);
                walls[1].SetActive(true);
                walls[2].SetActive(true);
                walls[3].SetActive(true);
            break;
            case 'c':
                walls[1].SetActive(false);
                walls[0].SetActive(true);
                walls[2].SetActive(true);
                walls[3].SetActive(true);
            break;
            case 'd':
                walls[2].SetActive(false);
                walls[1].SetActive(true);
                walls[0].SetActive(true);
                walls[3].SetActive(true);
            break;
            case 'e':
                walls[3].SetActive(false);
                walls[1].SetActive(true);
                walls[2].SetActive(true);
                walls[0].SetActive(true);
            break;
        }
        MazeDisp.text = dispNumbers[coord[1] * mazeSize + coord[0]].ToString();
        if ((coord[1] * mazeSize + coord[0]).EqualsAny(yellow[0], yellow[1], yellow[2], yellow[3]))
        {
            foreach (GameObject wall in walls)
            {
                wall.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 0f);
            }
            MazeDisp.color = new Color(1f, 1f, 0f);
        }
        else
        {
            foreach (GameObject wall in walls)
            {
                wall.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f);
            }
            MazeDisp.color = new Color(1f, 1f, 1f);
        }
    }
    void input(int i)
    {
        if (usersanswer[1] > 3)
        {
            return;
        }
        Inputs[0].text = Inputs[1].text;
        Inputs[1].text = Inputs[2].text;
        Inputs[2].text = Inputs[3].text;
        if (i == 1)
        {
            Inputs[3].text = " " + i.ToString();
        }
        else
        {
            Inputs[3].text = i.ToString();
        }
        usersanswer[0] += i * (int)Mathf.Pow(10, 3 - usersanswer[1]);
        usersanswer[1] += 1;
        return;
    }
    void inputcheck(char c)
    {
        switch (c)
        {
            case 'c':
                usersanswer[0] = 0;
                usersanswer[1] = 0;
                Inputs[3].text = "";
                Inputs[2].text = "";
                Inputs[1].text = "";
                Inputs[0].text = "";
                break;
            case 's':
                if (usersanswer[1] <= 3)
                {
                    break;
                }
                if (expanswer == usersanswer[0])
                {
                    Debug.LogFormat("[Keypad Maze #{0}] You submitted {1} which is correct. Module solved.", _moduleID, usersanswer[0].ToString().PadLeft(4, '0'));
                    StartCoroutine(solveanim());
                }
                else
                {
                    Debug.LogFormat("[Keypad Maze #{0}] You submitted {1} which is incorrect. Module struck.", _moduleID, usersanswer[0].ToString().PadLeft(4, '0'));
                    module.HandleStrike();
                    usersanswer[0] = 0;
                    usersanswer[1] = 0;
                    Inputs[3].text = "";
                    Inputs[2].text = "";
                    Inputs[1].text = "";
                    Inputs[0].text = "";
                }
                break;
        }
        return;
    }
    IEnumerator solveanim()
    {
        walls[0].GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
        walls[1].GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
        walls[2].GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
        walls[3].GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
        sfx.PlaySoundAtTransform("Solve", transform);
        MazeDisp.color = new Color(0f, 1f, 0f);
        solved = true;
        module.HandlePass();
        string yeah = "Done";
        foreach (TextMesh input in Inputs)
        {
            input.color = new Color(0f, 1f, 0f);
        }
        Inputs[0].text = "G";
        Inputs[1].text = "o";
        Inputs[2].text = "o";
        Inputs[3].text = "d";
        walls[3].SetActive(false);
        walls[1].SetActive(false);
        walls[2].SetActive(false);
        walls[0].SetActive(true);
        MazeDisp.text = yeah[0].ToString();
        for (int i = 0; i<3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            walls[i + 1].SetActive(true);
            MazeDisp.text = yeah[i+1].ToString();
        }
        yield return new WaitForSeconds(0.5f);
        MazeDisp.text = "-";
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 4; i++)
        {
            Inputs[i].text = "";
            yield return new WaitForSeconds(0.05f);
        }
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Command for this module consist of string of valid characters. Valid characters are: u, r, l, d, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, s, c. urld - directional buttons, to move within the maze. Digits 0-9 are digits" +
        " on number pad for code submission. s - submit c - clear. For example, command !{0} ur123c4567s will move you up, then right, then enter digits 123, then clear the input, then enter digihts 4567 and submit them.";
#pragma warning restore 414
    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant().Trim();
        var split = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length == 0)
        {
            yield break;
        }
        if (split[0].StartsWith("press") || split.Length == 1)
        {
           
                var input = split.Skip(1).Join("");
            if (split.Length == 1)
            {
                input = split[0];
            }
            foreach (char i in input)
            {
                if (input.Any(letter => !letter.EqualsAny('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'u', 'l', 'd', 'r', 'c', 's')))
                {
                    yield break;
                }
            }
            yield return null;
            foreach (char i in input)
            {
                yield return new WaitForSeconds(0.1f);
                switch(i)
                {
                    case '0':
                        keypad[10].OnInteract();
                        break;
                    case '1':
                        keypad[0].OnInteract();
                        break;
                    case '2':
                        keypad[1].OnInteract();
                        break;
                    case '3':
                        keypad[2].OnInteract();
                        break;
                    case '4':
                        keypad[3].OnInteract();
                        break;
                    case '5':
                        keypad[4].OnInteract();
                        break;
                    case '6':
                        keypad[5].OnInteract();
                        break;
                    case '7':
                        keypad[6].OnInteract();
                        break;
                    case '8':
                        keypad[7].OnInteract();
                        break;
                    case '9':
                        keypad[8].OnInteract();
                        break;
                    case 'u':
                        directional[0].OnInteract();
                        break;
                    case 'r':
                        directional[1].OnInteract();
                        break;
                    case 'l':
                        directional[2].OnInteract();
                        break;
                    case 'd':
                        directional[3].OnInteract();
                        break;
                    case 'c':
                        keypad[9].OnInteract();
                        break;
                    case 's':
                        keypad[11].OnInteract();
                        break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    }
