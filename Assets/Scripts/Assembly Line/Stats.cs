using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stats {
    private static int goodphones, badphones, droppedphones, droppedscrews, droppedscrewdriver;
    private static bool victory;

    public static int Goodphones
    {
        get
        {
            return goodphones;
        }
        set
        {
            goodphones = value;
        }
    }
    public static int Badphones
    {
        get
        {
            return badphones;
        }
        set
        {
            badphones = value;
        }
    }

    public static int Droppedphones
    {
        get
        {
            return droppedphones;
        }
        set
        {
            droppedphones = value;
        }
    }

    public static int Droppedscrews
    {
        get
        {
            return droppedscrews;
        }
        set
        {
            droppedscrews = value;
        }
    }
    public static int Droppedscrewdriver
    {
        get
        {
            return droppedscrewdriver;
        }
        set
        {
            droppedscrewdriver = value;
        }
    }
    public static bool Victory
    {
        get
        {
            return victory;
        }
        set
        {
            victory = value;
        }
    }


}
