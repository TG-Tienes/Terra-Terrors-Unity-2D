using System;
using UnityEngine;
using UnityEngine.UI;

public class NumberFormatter
{
    public string FormatNumber(int number)
    {
        return number.ToString("N0");
    }
}