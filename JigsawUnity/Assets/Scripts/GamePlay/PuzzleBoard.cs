using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JigsawGame
{
	public class PuzzleBoard
	{
        public ushort Id { get; private set;  }
        public PuzzlePiece[] Pieces { get; private set; }
        public ushort NumRows { get; private set; }
        public ushort NumCols { get; private set; }

        public PuzzleBoard(ushort numPieces)
        {
            Id = (ushort)UnityEngine.Random.Range(0, uint.MaxValue);

            NumRows = NumCols = (ushort)Mathf.Sqrt(numPieces);

            Pieces = new PuzzlePiece[numPieces];
            for (ushort i = 0; i < NumRows; ++i)
            {
                for (ushort j = 0; j < NumCols; ++j)
                {
                    Pieces[i * NumCols + j] = new PuzzlePiece(i, j);
                }
            }
        }

        /// <summary>
        /// Generates the puzzle formation
        /// </summary>
        public void GeneratePuzzle()
        {
            for (ushort i = 0; i < NumRows; ++i)
            {
                for (ushort j = 0; j < NumCols; ++j)
                {
                    GenerateDents(i, j);
                }
            }
        }

        private void GenerateDents(ushort row, ushort col)
        {
            PuzzlePiece topNeighbor = GetTopNeighbor(row, col);
            PuzzlePiece leftNeighbor = GetLeftNeighbor(row, col);
            PuzzlePiece bottomNeighbor = GetBottomNeighbor(row, col);
            PuzzlePiece rightNeighbor = GetRightNeighbor(row, col);

            PuzzlePiece currentPiece = Pieces[row * NumCols + col];

            //generate modifier on top side
            currentPiece.Sides[(uint)PuzzlePieceSide.Side.Top].Modifier     = GenerateTopModifier(topNeighbor);
            currentPiece.Sides[(uint)PuzzlePieceSide.Side.Left].Modifier    = GenerateLeftModifier(leftNeighbor);
            currentPiece.Sides[(uint)PuzzlePieceSide.Side.Bottom].Modifier  = GenerateModifier(bottomNeighbor);
            currentPiece.Sides[(uint)PuzzlePieceSide.Side.Right].Modifier   = GenerateModifier(rightNeighbor);
        }


        private PuzzlePiece GetTopNeighbor(ushort row, ushort col)
        {
            return row == 0 ? null : Pieces[(row - 1) * NumCols + col];
        }

        private PuzzlePiece GetLeftNeighbor(ushort row, ushort col)
        {
            return col == 0 ? null : Pieces[row * NumCols + col - 1];
        }

        private PuzzlePiece GetBottomNeighbor(ushort row, ushort col)
        {
            return row == NumRows - 1 ? null : Pieces[(row + 1) * NumCols + col];
        }

        private PuzzlePiece GetRightNeighbor(ushort row, ushort col)
        {
            return col == NumCols - 1 ? null : Pieces[row * NumCols + col + 1];
        }

        private PuzzlePieceModifier GenerateTopModifier(PuzzlePiece topNeighbor)
        {
            if (topNeighbor == null)
                return PuzzlePieceModifier.None;

            return GetOppositeModifier(topNeighbor.Sides[(uint)PuzzlePieceSide.Side.Bottom].Modifier);
        }

        private PuzzlePieceModifier GetOppositeModifier(PuzzlePieceModifier modifier)
        {
            return modifier == PuzzlePieceModifier.Dent ? PuzzlePieceModifier.Protrusion : PuzzlePieceModifier.Dent;
        }

        private PuzzlePieceModifier GenerateLeftModifier(PuzzlePiece leftNeighbor)
        {
            if (leftNeighbor == null)
                return PuzzlePieceModifier.None;

            return GetOppositeModifier(leftNeighbor.Sides[(uint)PuzzlePieceSide.Side.Right].Modifier);
        }

        private PuzzlePieceModifier GenerateModifier(PuzzlePiece aNeighbor)
        {
            if (aNeighbor == null)
                return PuzzlePieceModifier.None;

            if ((uint)UnityEngine.Random.Range(0, uint.MaxValue) % 2 == 0)
                return PuzzlePieceModifier.Dent;

            return PuzzlePieceModifier.Protrusion;
        }


        /// <summary>
        /// returns true if the generated puzzle formation is valid
        /// </summary>
        public bool IsValid()
        {
            foreach(var piece in Pieces)
            {
                if (!FitsNeighbors(piece))
                    return false;
            }

            return true;
        }

        private bool FitsNeighbors(PuzzlePiece aPiece)
        {
            if (aPiece == null)
                return false;
            if (aPiece.Row >= NumRows)
                return false;
            if (aPiece.Col >= NumCols)
                return false;
            
            ushort row = aPiece.Row;
            ushort col = aPiece.Col;
            PuzzlePiece topNeighbor = GetTopNeighbor(row, col);
            PuzzlePiece leftNeighbor = GetLeftNeighbor(row, col);
            PuzzlePiece bottomNeighbor = GetBottomNeighbor(row, col);
            PuzzlePiece rightNeighbor = GetRightNeighbor(row, col);

            //if top is valid and top neighbor's Bottom side's modifier is not opposite of requested piece's Top side's modifier 
            //piece doesn't fit
            if (topNeighbor != null && topNeighbor.Sides[(uint)PuzzlePieceSide.Side.Bottom].Modifier != GetOppositeModifier(aPiece.Sides[(uint)PuzzlePieceSide.Side.Top].Modifier))
                return false;

            //if left neighbor is not null and left neighbor's Right side's modifer is not opposite of requested piece's Left side's modifier
            //the piece doesn't fit
            if (leftNeighbor != null && leftNeighbor.Sides[(uint)PuzzlePieceSide.Side.Right].Modifier != GetOppositeModifier(aPiece.Sides[(uint)PuzzlePieceSide.Side.Left].Modifier))
                return false;

            //if right neighbor is not null and right neighbor's Left side's modifer is not opposite of requested piece's Right side's modifier
            //the piece doesn't fit
            if (rightNeighbor != null && rightNeighbor.Sides[(uint)PuzzlePieceSide.Side.Left].Modifier != GetOppositeModifier(aPiece.Sides[(uint)PuzzlePieceSide.Side.Right].Modifier))
                return false;

            //if bottom neighbor is not null and bottom neighbor's Top side's modifer is not opposite of requested piece's Bottom side's modifier
            //the piece doesn't fit
            if (bottomNeighbor != null && bottomNeighbor.Sides[(uint)PuzzlePieceSide.Side.Top].Modifier != GetOppositeModifier(aPiece.Sides[(uint)PuzzlePieceSide.Side.Bottom].Modifier))
                return false;

            return true;
        }
    }
}