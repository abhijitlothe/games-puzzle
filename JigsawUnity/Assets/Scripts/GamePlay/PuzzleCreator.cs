using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JigsawGame
{
    public class PuzzleCreator
    {
        public const ushort MinPiecesInOneCol = 2;
        public const ushort MaxPiecesInOneCol = 25;

        public static PuzzleBoard CreateBoard(ushort maxInOneColumn)
        {
            if (maxInOneColumn < MinPiecesInOneCol || maxInOneColumn > MaxPiecesInOneCol)
                return null;
            
            return new PuzzleBoard((ushort)(maxInOneColumn * maxInOneColumn));
        }
    }
}
