using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JigsawGame
{
    public class PuzzlePiece
    {
        private const int MaxNumSides = 4;
        public PuzzlePieceSide[] Sides { get; private set; }
        public ushort Row { get; private set; }
        public ushort Col { get; private set; }
        public Texture2D Image { get; set; }

        public PuzzlePiece(ushort row, ushort col)
        {
            Sides = new PuzzlePieceSide[MaxNumSides];
            Row = row;
            Col = col;
        }
    }
}
