using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JigsawGame
{
    public struct PuzzlePieceSide
    {
        public enum Side
        {
            Top = 0,
            Bottom,
            Left,
            Right
        }
        public PuzzlePieceModifier Modifier { get; set; }
    }
}
