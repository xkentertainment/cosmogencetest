using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionModifier : MonoBehaviour
{
    [SerializeField]
    int start;
    [SerializeField]
    int end;

    public int Start => start - 1;
    public int End => end - 1;

    public bool IsLadder => end > start;
}
