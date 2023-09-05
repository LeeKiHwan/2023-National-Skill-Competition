using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESpecialBlock
{
    JumpBlock,
    TempBlock,
    TrapBlock
}

public class SpecialBlock : MonoBehaviour
{
    public ESpecialBlock thisBlockType;
}
