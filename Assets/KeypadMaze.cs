using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.Linq;
using System;
using Random = System.Random;

public class KeypadMaze : MonoBehaviour {
    public int maze;
    public KMBombModule module;
    public KMBombInfo info;
    public KMAudio sfx;
    public int[] coord;
    public int[] yellow = { 0, 0, 0, 0, 0 };
    public char pos;
    public KMSelectable[] keypad;
    public KMSelectable[] directional;
    public GameObject[] walls;
    Random rnd = new Random();
    public TextMesh MazeDisp;
    public TextMesh[] Inputs;
    public int[] usersanswer = {0, 0};
    public int expanswer;
    public bool active;
    public bool solved;
    public Color[] Direccolors = { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 1f, 0f)};
    public string[] directions = { "urdl", "urld", "uldr", "udrl", "ulrd", "udlr",
        "rdlu", "rdul", "rudl", "ruld", "rlud", "rldu",
        "lurd", "ludr", "ldur", "ldru", "lrdu", "lrud",
        "dulr", "durl", "drul", "drlu", "dlur", "dlru"};
    public string currentDirections;
    public int[][] order = { new[]{1, 2, 2, 4, 2, 4, 4, 2, 1, 1,
    2, 2, 1, 4, 3, 3, 4, 2, 4, 2,
    1, 1, 2, 3, 3, 1, 1, 1, 2, 3,
    3, 1, 2, 3, 1, 4, 4, 4, 3, 1,
    2, 3, 4, 3, 4, 4, 4, 3, 4, 1,
    1, 2, 2, 3, 3, 4, 4, 4, 2, 4,
    2, 1, 2, 2, 1, 2, 3, 3, 1, 2,
    3, 3, 3, 1, 2, 3, 2, 1, 1, 2,
    3, 4, 4, 1, 4, 3, 4, 3, 4, 1,
    2, 4, 3, 1, 1, 4, 2, 3, 4, 1},
    new[]{1, 2, 1, 3, 3, 3, 1, 2, 1, 4,
    3, 2, 3, 2, 4, 1, 3, 1, 4, 2,
    1, 4, 3, 2, 3, 3, 4, 2, 2, 2,
    1, 4, 2, 4, 4, 2, 3, 1, 1, 1,
    4, 4, 4, 4, 1, 3, 2, 1, 3, 4,
    3, 1, 3, 2, 4, 3, 3, 1, 4, 4,
    1, 3, 1, 1, 4, 1, 2, 3, 4, 4,
    3, 3, 3, 3, 2, 4, 1, 2, 4, 3,
    2, 1, 1, 3, 2, 1, 1, 2, 2, 3,
    2, 1, 1, 2, 1, 4, 4, 4, 4, 3,},
    new[]{ 4, 1, 4, 4, 2, 1, 1, 1, 3, 3,
    2, 3, 4, 1, 3, 4, 1, 3, 3, 2,
    4, 3, 2, 1, 1, 3, 1, 2, 3, 1,
    2, 4, 3, 3, 3, 2, 4, 2, 2, 4,
    1, 2, 2, 4, 1, 4, 2, 4, 2, 1,
    2, 1, 1, 3, 4, 1, 1, 1, 2, 1,
    1, 1, 4, 2, 3, 4, 3, 1, 2, 4,
    4, 3, 2, 3, 2, 4, 3, 4, 3, 3,
    3, 4, 2, 2, 1, 1, 4, 1, 1, 4,
    3, 3, 2, 3, 4, 2, 3, 4, 2, 4},
    new[]{ 2, 3, 2, 2, 4, 1, 2, 1, 4, 4,
        2, 2, 3, 3, 3, 1, 4, 1, 2, 3,
        1, 3, 2, 3, 2, 3, 4, 2, 1, 4,
        4, 4, 1, 1, 3, 1, 2, 1, 3, 1,
        3, 3, 4, 4, 4, 4, 1, 4, 1, 3,
        3, 4, 1, 2, 3, 3, 4, 2, 3, 3,
        2, 3, 1, 4, 2, 4, 2, 4, 4, 1,
        4, 2, 3, 2, 1, 1, 2, 4, 1, 2,
        2, 3, 1, 1, 4, 1, 3, 1, 3, 4,
        1, 4, 2, 1, 4, 3, 2, 1, 4, 3},
    new[]{ 4, 3, 3, 3, 2, 3, 3, 1, 4, 3,
        2, 1, 3, 4, 1, 4, 2, 1, 2, 4,
        3, 1, 2, 3, 1, 4, 4, 1, 4, 2,
        2, 4, 2, 2, 2, 1, 1, 2, 4, 4,
        3, 1, 4, 3, 2, 1, 4, 2, 1, 3,
        3, 1, 4, 1, 1, 2, 4, 3, 2, 4,
        2, 1, 2, 2, 3, 3, 4, 3, 3, 2,
        3, 1, 4, 4, 1, 3, 4, 3, 3, 3,
        4, 2, 4, 3, 2, 1, 1, 2, 2, 1,
        4, 4, 4, 1, 2, 1, 2, 1, 4, 3},
    new[]{ 1, 1, 4, 1, 3, 3, 3, 2, 4, 1,
    1, 4, 1, 2, 1, 2, 3, 3, 2, 4,
    1, 2, 3, 4, 1, 2, 2, 1, 2, 3,
    2, 3, 3, 4, 4, 3, 4, 4, 2, 4,
    2, 3, 1, 3, 2, 2, 3, 1, 3, 2,
    2, 2, 1, 4, 3, 4, 3, 3, 2, 2,
    3, 1, 2, 4, 3, 1, 2, 4, 2, 2,
    1, 4, 3, 3, 3, 4, 2, 4, 4, 3,
    4, 2, 1, 3, 1, 4, 1, 2, 3, 1,
    4, 2, 1, 1, 2, 2, 2, 4, 4, 4},
    new[]{ 2, 1, 3, 4, 4, 4, 3, 4, 1, 1,
    3, 4, 2, 1, 2, 3, 2, 2, 2, 1,
    1, 3, 1, 3, 2, 2, 1, 4, 4, 2,
    4, 1, 1, 1, 3, 2, 2, 1, 1, 4,
    1, 2, 4, 3, 4, 1, 4, 1, 3, 3,
    3, 2, 2, 2, 4, 3, 4, 1, 3, 4,
    3, 4, 3, 4, 3, 3, 4, 2, 4, 4,
    3, 2, 4, 3, 2, 4, 1, 2, 3, 2,
    1, 3, 4, 4, 1, 4, 1, 4, 2, 3,
    4, 3, 1, 2, 2, 2, 3, 1, 2, 3,},
    new[]{ 3, 3, 4, 1, 2, 4, 2, 2, 1, 4,
    4, 2, 3, 1, 1, 2, 4, 1, 2, 4,
    3, 3, 3, 4, 2, 3, 1, 4, 4, 1,
    2, 1, 1, 3, 3, 1, 4, 2, 3, 2,
    2, 4, 4, 3, 1, 4, 4, 1, 4, 3,
    4, 3, 2, 4, 3, 3, 2, 3, 1, 2,
    2, 1, 1, 2, 3, 3, 2, 2, 1, 2,
    1, 1, 4, 1, 4, 1, 3, 4, 3, 2,
    3, 2, 2, 2, 1, 1, 4, 1, 3, 3,
    1, 4, 4, 3, 1, 2, 3, 4, 2, 4,},
    new[]{ 2, 1, 4, 2, 2, 3, 2, 3, 4, 1,
    2, 4, 3, 4, 1, 2, 4, 3, 3, 3,
    1, 1, 3, 4, 1, 4, 4, 3, 3, 1,
    1, 2, 4, 2, 1, 3, 1, 4, 2, 2,
    1, 1, 3, 4, 4, 1, 1, 4, 1, 4,
    2, 3, 4, 3, 1, 1, 2, 2, 3, 3,
    4, 1, 2, 3, 4, 4, 4, 2, 4, 3,
    2, 3, 2, 2, 1, 3, 2, 3, 1, 2,
    1, 2, 4, 2, 3, 3, 2, 3, 1, 1,
    3, 2, 4, 1, 4, 3, 1, 1, 4, 4,},
    new[]{ 1, 4, 2, 3, 2, 3, 3, 2, 4, 3,
    4, 2, 1, 1, 1, 2, 4, 2, 4, 1,
    4, 1, 4, 2, 3, 4, 3, 3, 1, 2,
    4, 1, 2, 3, 1, 3, 2, 3, 4, 1,
    1, 3, 2, 4, 1, 2, 3, 4, 2, 2,
    2, 4, 3, 2, 4, 4, 3, 3, 4, 1,
    1, 3, 3, 1, 1, 4, 1, 1, 3, 4,
    3, 2, 4, 1, 3, 1, 2, 4, 2, 2,
    2, 1, 2, 4, 4, 3, 2, 1, 3, 4,
    1, 2, 3, 2, 1, 1, 3, 3, 4, 4,}};
    public char[][] mazes = { new[]{'0', '2', '4', '3', '3', '0', '3', '2', '7', '0',
    '6', 'a', 'a', 'c', '9', '2', 'd', '7', '9', '3',
    '1', '6', '7', '5', '8', '6', '4', '1', '5', '8',
    '0', '5', '8', '6', '7', '1', '6', 'a', 'a', '4',
    '2', '7', '6', 'd', '8', '6', '8', '6', '7', '3',
    '3', '9', '5', '7', '6', 'c', '2', '8', '9', '1',
    '1', '5', 'a', '8', '9', '5', 'b', 'a', '8', '3',
    '6', '7', '3', '2', 'd', 'a', '8', '6', '7', '1',
    '1', '9', '9', '6', 'a', '7', '6', '8', '5', '4',
    '0', '1', '1', '1', '2', '8', '1', '2', '4', '0'},
    //---------------------------------------------\\
    new[]{'2', '4', '3', '2', '4', '2', '4', '2', '4', '0',
    '2', 'a', 'd', '4', '6', '7', '2', 'a', 'a', '4',
    '2', 'a', 'a', 'b', '8', '5', 'b', 'a', 'a', '4',
    '0', '2', '7', '9', '2', 'a', '8', '3', '6', '7',
    '2', '7', '9', '5', 'a', 'a', '7', '5', '8', '1',
    '3', '5', 'd', 'a', 'a', '7', 'e', '7', '6', '4',
    '5', '7', '2', '7', '3', 'e', '8', '5', '8', '0',
    '3', '5', 'a', 'c', '5', '8', '6', '7', '6', '4',
    '1', '6', '4', '9', '6', 'a', '8', '1', '9', '3',
    '0', '1', '0', '1', '5', 'a', 'a', '4', '1', '1'},
    //---------------------------------------------\\
    new[]{'0', '3', '2', '4', '2', '4', '3', '2', '4', '0',
    '0', '5', '7', '6', '7', '2', '8', '6', 'b', '4',
    '2', '7', 'e', '8', '5', 'b', 'a', 'd', '8', '3',
    '2', '8', '9', '6', '7', '5', '4', '3', '6', '8',
    '6', '7', '1', '5', '8', '3', '6', '8', '9', '0',
    '1', '1', '6', 'a', '7', '9', '1', '2', 'd', '4',
    '2', 'b', '8', '6', 'c', '9', '2', 'a', '7', '3',
    '3', '5', '7', '9', '9', '5', 'b', 'a', '8', '1',
    '5', 'a', '8', '5', 'd', 'a', '8', '2', 'a', '4',
    '0', '0', '2', '4', '2', 'a', 'a', '4', '2', '4'},
    //---------------------------------------------\\
    new[]{'0', '3', '3', '0', '3', '3', '3', '3', '2', '4',
    '2', 'c', '9', '6', 'd', '8', '5', '8', '3', '3',
    '2', '8', '9', '9', '0', '6', 'b', 'a', '8', '1',
    '0', '6', '8', '9', '3', '9', '5', '4', '6', '7',
    '0', '5', '7', 'e', '8', '5', 'b', '7', '9', '1',
    '6', '7', '1', '5', 'a', '7', '5', 'c', '9', '0',
    '1', '9', '6', 'a', 'a', '8', '3', '9', '5', '4',
    '3', '9', '5', '7', '6', '7', 'e', '8', '6', '7',
    '1', 'e', 'a', 'c', '5', '8', '5', 'a', '8', '1',
    '0', '1', '0', '1', '2', 'a', '4', '0', '2', '4'},
    //---------------------------------------------\\
    new[]{'0', '0', '3', '6', '4', '3', '2', 'a', '4', '0',
    '2', '7', '9', '9', '6', '8', '2', '4', '6', '4',
    '3', '9', '5', 'd', '8', '6', '4', '6', 'd', '4',
    '9', '5', '4', '6', '4', '5', '4', '9', '6', '4',
    '1', '6', 'b', '8', '6', 'a', '7', '5', '8', '3',
    '3', '5', '8', '6', '8', '6', '8', '6', '7', '1',
    '1', '6', 'a', 'd', 'a', 'd', '7', '9', 'e', '4',
    '3', '5', '4', '6', 'a', '7', '9', '5', '8', '3',
    '1', '3', '6', '8', '3', '5', '8', '3', '2', '8',
    '0', '1', '5', '4', '5', 'a', '4', '5', '4', '0'},
    //---------------------------------------------\\
    new[]{'a', '4', '2', '7', '3', '3', '3', '3', '3', '0',
    '3', '6', '4', '9', '9', '9', '9', '1', '9', '3',
    '1', '9', '6', '8', '5', '8', '9', '6', '8', '1',
    '3', '5', '8', '6', 'a', '7', '9', '5', 'b', '4',
    '5', '4', '6', '8', '6', '8', '1', '6', '8', '3',
    '0', '6', '8', '6', 'c', '2', '4', 'e', '7', '1',
    '0', '5', '7', '9', '5', 'a', '7', '9', '5', '4',
    '2', 'a', 'd', '8', '6', 'b', '8', '1', '3', '3',
    '2', '7', '2', '7', '9', '5', 'a', 'a', 'c', '1',
    '0', '5', '4', '1', '5', 'a', 'a', '4', '1', '0'},
   //---------------------------------------------\\
    new[]{'0', '3', '3', '2', 'a', '4', '3', '3', '3', '3',
    '3', '9', '5', '7', '6', 'a', '8', '9', '1', '1',
    '1', '5', '7', '1', '5', '7', '3', 'e', 'a', '4',
    '2', '7', '9', '2', 'a', '8', 'e', '8', '2', '7',
    '6', '8', '1', '6', 'a', '4', '9', '2', '7', '1',
    '1', '6', '7', 'e', 'a', 'a', '8', '6', '8', '3',
    '3', '5', '8', '9', '6', 'a', 'b', '8', '0', '9',
    '5', 'a', 'a', 'd', '8', '6', '8', '6', '4', '1',
    '0', '6', '7', '2', 'a', '8', '6', '8', '6', '4',
    '0', '1', '5', '4', '2', 'a', '8', '2', '8', '0'},
    //---------------------------------------------\\
    new[]{'0', '0', '3', '2', '4', '3', '3', '3', '2', '4',
    '2', 'a', 'd', '4', '2', '8', 'e', '8', '6', '4',
    '2', '7', '6', 'b', 'a', '7', '9', '3', '5', '4',
    '3', '9', '5', '8', '6', '8', '9', '5', '7', '0',
    '5', '8', '2', '7', 'e', '7', '5', '7', '5', '4',
    '2', 'a', '7', '9', '9', '5', 'b', '8', '2', '4',
    '2', 'a', 'd', '8', '9', '3', '5', 'a', 'a', '4',
    '2', '4', '2', '7', '9', '5', 'a', 'b', 'a', '4',
    '2', 'a', '7', '9', '5', 'a', '7', '5', 'a', '4',
    '0', '2', '8', '1', '2', 'a', '8', '2', '4', '0'},
    //---------------------------------------------\\
    new[]{'0', '0', '3', '6', '7', '2', '4', '3', '3', '0',
    '2', 'a', '8', '9', '9', '6', '4', '5', '8', '3',
    '2', '7', '6', '8', 'e', '8', '6', 'a', '7', '1',
    '3', '9', '5', 'a', '8', '3', 'e', '7', '5', '7',
    '1', '5', 'a', '4', '6', 'd', '8', '5', '7', '1',
    '2', '7', '6', '7', '9', '6', 'a', '7', '9', '0',
    '2', 'c', '1', '9', '9', '9', '6', 'd', '8', '3',
    '3', '5', 'a', 'd', '8', '5', '8', '3', '6', '8',
    '1', '6', 'a', 'a', 'a', 'a', 'a', 'c', '9', '0',
    '0', '1', '2', '4', '2', '4', '0', '1', '1', '0'},
    //---------------------------------------------\\
    new[]{'2', '4', '0', '6', 'a', '4', '6', '4', '0', '0',
    '0', '6', '7', '9', '6', '4', '9', '6', 'a', '4',
    '3', '5', 'c', '9', '9', '6', '8', '5', 'a', '7',
    '5', '7', '9', '5', '8', '9', '6', 'a', '7', '1',
    '0', '1', 'e', '7', '6', '8', '5', '7', '9', '0',
    '6', '7', '9', '1', '5', 'a', '4', '9', 'e', '4',
    '1', '9', '5', 'a', 'a', 'a', '7', '9', '9', '3',
    '2', 'd', 'a', 'b', 'a', 'a', '8', '5', 'c', '1',
    '2', '7', '3', '9', '6', '7', '6', '7', '5', '4',
    '0', '1', '1', '1', '1', '1', '1', '1', '0', '0'} };
    public int[] dispNumbers = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    private static int _moduleIDCounter = 1;
    private int _moduleID;
    void Awake()
    {
        maze = rnd.Next(0, 10);
        while (yellow[4] != 1)
        {
            yellow[0] = rnd.Next(0, 100);
            yellow[4] = order[maze][yellow[0]];
        }
        while (yellow[4] != 2)
        {
            yellow[1] = rnd.Next(0, 100);
            yellow[4] = order[maze][yellow[1]];
        }
        while (yellow[4] != 3)
        {
            yellow[2] = rnd.Next(0, 100);
            yellow[4] = order[maze][yellow[2]];
        }
        while (yellow[4] != 4)
        {
            yellow[3] = rnd.Next(0, 100);
            yellow[4] = order[maze][yellow[3]];
        }
        module.OnActivate += delegate
        {
            _moduleID = _moduleIDCounter++;
            for (int i = 0; i < 100; i++)
            {
                dispNumbers[i] = rnd.Next(0, 10);
            }
            active = true;
            coord[0] = rnd.Next(0, 10);
            coord[1] = rnd.Next(0, 10);
            pos = mazes[maze][coord[1] * 10 + coord[0]];
            wallchange();
            currentDirections = directions[rnd.Next(0, 24)];
            foreach (char c in currentDirections)
            {
                directional[currentDirections.IndexOf(c, 0)].GetComponent<MeshRenderer>().material.color = Direccolors[directions[0].IndexOf(c, 0)];
            }
            expanswer = dispNumbers[yellow[0]] * 1000 + dispNumbers[yellow[1]] * 100 + dispNumbers[yellow[2]] * 10 + dispNumbers[yellow[3]];
            Debug.LogFormat("[Keypad Maze #{0}] You are in maze {1}", _moduleID, maze+1);
            Debug.LogFormat("[Keypad Maze #{0}] Coordinates are counted in ''row, column'' format, with top left being 0, 0.", _moduleID);
            Debug.LogFormat("[Keypad Maze #{0}] Initial coordintaes are: {1}, {2}.", _moduleID, coord[1], coord[0]);
            Debug.LogFormat("[Keypad Maze #{8}] Yellow cells are: {1}, {0}; {3}, {2}; {5}, {4}; {7}, {6}.", yellow[0] % 10, (yellow[0]-yellow[0]%10) / 10, yellow[1] % 10, (yellow[1] - yellow[1] % 10) / 10, yellow[2] % 10, (yellow[2] - yellow[2] % 10) / 10, yellow[3] % 10, (yellow[3] - yellow[3] % 10) / 10, _moduleID);
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
        directional[x].OnInteract += delegate { if (!active || solved) { return false; } movecheck(currentDirections[x]); GetComponent<KMSelectable>().AddInteractionPunch(0.1f); sfx.PlaySoundAtTransform("Moved", transform); return false; };
    }

    void NumberPress (int x)
    {
        keypad[x].OnInteract += delegate { if (!active || solved || usersanswer[1] > 3) { return false; } input(x+1); sfx.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); GetComponent<KMSelectable>().AddInteractionPunch(0.1f); return false; };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void movecheck(char c)
    {
        switch (c)
        {
            case 'u':
                if (pos.EqualsAny('1', '5', '8', '9', 'c', 'd', 'e'))
                {
                    Debug.LogFormat("[Keypad Maze #{0}] Strike because of wall hit :/", _moduleID);
                    module.HandleStrike();
                }
                else
                {
                    coord[1] -= 1;
                    while (coord[1] < 0)
                    {
                        coord[1] += 10;
                    }
                    pos = mazes[maze][coord[1] * 10 + coord[0]];
                    wallchange();
                }
                break;
            case 'r':
                if (pos.EqualsAny('2', '5', '6', 'a', 'b', 'd', 'e'))
                {
                    Debug.LogFormat("[Keypad Maze #{0}] Strike because of wall hit :/", _moduleID);
                    module.HandleStrike();
                }
                else
                {
                    coord[0] += 1;
                    coord[0] = coord[0] % 10;
                    pos = mazes[maze][coord[1] * 10 + coord[0]];
                    wallchange();
                }
            break;
            case 'l':
                if (pos.EqualsAny('4', '7', '8', 'a', 'b', 'c', 'd'))
                {
                    Debug.LogFormat("[Keypad Maze #{0}] Strike because of wall hit :/", _moduleID);
                    module.HandleStrike();
                }
                else
                {
                    coord[0] -= 1;
                    while (coord[0] < 0)
                    {
                        coord[0] += 10;
                    }
                    pos = mazes[maze][coord[1] * 10 + coord[0]];
                    wallchange();
                }
                break;
            case 'd':
                if (pos.EqualsAny('3', '6', '7', '9', 'b', 'c', 'e'))
                {
                    Debug.LogFormat("[Keypad Maze #{0}] Strike because of wall hit :/", _moduleID);
                    module.HandleStrike();
                }
                else
                {
                    coord[1] += 1;
                    coord[1] = coord[1] % 10;
                    pos = mazes[maze][coord[1] * 10 + coord[0]];
                    wallchange();
                }
                break;
        }
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
        MazeDisp.text = dispNumbers[coord[1] * 10 + coord[0]].ToString();
        if ((coord[1] * 10 + coord[0]).EqualsAny(yellow[0], yellow[1], yellow[2], yellow[3]))
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
                    Debug.LogFormat("[Keypad Maze #{0}] You submitted {1} which is incorrect. Module striked.", _moduleID, usersanswer[0].ToString().PadLeft(4, '0'));
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
        string yeah = "yeah";
        foreach (TextMesh input in Inputs)
        {
            input.color = new Color(0f, 1f, 0f);
        }
        Inputs[0].text = "N";
        Inputs[1].text = " i";
        Inputs[2].text = "c";
        Inputs[3].text = "e";
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
    private string TwitchHelpMessage = "!{0} abcd0123456789rs / press abcd0123456789rs (pressing every valid button). R - reset, S - submit.";
#pragma warning restore 414
    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant().Trim();
        var split = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (split[0].StartsWith("press") || split.Length == 1)
        {
           
                var input = split.Skip(1).Join("");
            if (split.Length == 1)
            {
                input = split[0];
            }
            foreach (char i in input)
            {
                if (input.Any(letter => !letter.EqualsAny('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'r', 's')))
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
                    case 'a':
                        directional[0].OnInteract();
                        break;
                    case 'b':
                        directional[1].OnInteract();
                        break;
                    case 'c':
                        directional[2].OnInteract();
                        break;
                    case 'd':
                        directional[3].OnInteract();
                        break;
                    case 'r':
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
