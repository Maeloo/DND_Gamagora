using UnityEngine;
using System.Collections;

public static class Constants
{
    // Movements
    public static readonly KeyCode MoveRightButton = KeyCode.RightArrow | KeyCode.D;
    public static readonly KeyCode MoveLeftButton = KeyCode.LeftArrow | KeyCode.Q;
    public static readonly KeyCode MoveUpButton = KeyCode.UpArrow | KeyCode.Z;
    public static readonly KeyCode CrouchButton = KeyCode.DownArrow | KeyCode.S;

    public static readonly KeyCode RunButton = KeyCode.LeftShift | KeyCode.RightShift;
    public static readonly KeyCode JumpButton = KeyCode.Space;
}
