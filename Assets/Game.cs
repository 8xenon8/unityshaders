﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Player player;

    public bool isFlipped = false;

    private static Game current;

    void Start()
    {
        if (current)
        {
            Destroy(this);
        }

        current = this;

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public static Game Current()
    {
        return current;
    }

    public void MirrorSwap()
    {
        isFlipped = !isFlipped;
        GL.invertCulling = isFlipped;
    }
}