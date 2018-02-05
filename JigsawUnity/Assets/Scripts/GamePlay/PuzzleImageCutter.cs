using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JigsawGame
{
    public class PuzzleImageCutter
    {
        public static bool CutPieces(Texture2D baseImage, PuzzleBoard board, Texture2D mask)
        {
            if (baseImage == null || board == null || mask == null)
                return false;

            int imgWidth = baseImage.width;
            int imgHeight = baseImage.height;

            int minPieceImageWidth = imgWidth / board.NumCols;
            int minPieceImageHeight = imgHeight / board.NumRows;
            int maskWidth = (int)(minPieceImageWidth * 0.33f);
            int maskHeight = (int)(minPieceImageHeight * 0.33f);

            foreach (var piece in board.Pieces)
            {
                int startX, startY, pieceWidth, pieceHeight;
                GetPieceDimensions(piece, maskWidth, maskHeight, minPieceImageWidth, minPieceImageHeight, out pieceWidth, out pieceHeight);

                GetStartingPositions(piece, maskWidth, maskHeight, pieceWidth, pieceHeight, out startX, out startY);
                //Texture2D pieceImage = CreatePieceImage(baseImage, mask, startX, startY, pieceWidth, pieceHeight);
            }
            return true;
        }

        private static Texture2D CreatePieceImage(Texture2D baseImage,
                                                  Texture2D mask,
                                                  int startX,
                                                  int startY,
                                                  int pieceWidth,
                                                  int pieceHeight)
        {
            Texture2D texture = new Texture2D(pieceWidth, pieceHeight, TextureFormat.RGB24, false);

            for (int y = 0; y < texture.height; ++y)
            {
                for (int x = 0; x < texture.width; ++x)
                {
                    
                }
            }
            return texture;
        }

        private static void GetPieceDimensions(PuzzlePiece piece,
                                               int maskWidth,
                                               int maskHeight,
                                               int minPieceImageWidth,
                                               int minPieceImageHeight,
                                               out int pieceWidth,
                                               out int pieceHeight)
        {
            pieceWidth = minPieceImageWidth;
            pieceHeight = minPieceImageHeight;

            if (piece.Sides[(int)PuzzlePieceSide.Side.Left].Modifier == PuzzlePieceModifier.Protrusion)
            {
                pieceWidth += maskWidth;
            }
            if (piece.Sides[(int)PuzzlePieceSide.Side.Right].Modifier == PuzzlePieceModifier.Protrusion)
            {
                pieceWidth += maskWidth;
            }

            if (piece.Sides[(int)PuzzlePieceSide.Side.Top].Modifier == PuzzlePieceModifier.Protrusion)
            {
                pieceHeight += maskHeight;
            }

            if (piece.Sides[(int)PuzzlePieceSide.Side.Bottom].Modifier == PuzzlePieceModifier.Protrusion)
            {
                pieceHeight += maskHeight;
            }
        }

        private static void GetStartingPositions(PuzzlePiece piece,
                                                        int maskWidth,
                                                        int maskHeight,
                                                        int pieceWidth,
                                                        int pieceHeight,
                                                        out int startX,
                                                        out int startY)
        {
            startX = piece.Col * pieceWidth;
            startY = piece.Row * pieceHeight;

            if (piece.Sides[(int)PuzzlePieceSide.Side.Left].Modifier == PuzzlePieceModifier.Protrusion)
            {
                startX -= maskWidth;
            }

            if (piece.Sides[(int)PuzzlePieceSide.Side.Top].Modifier == PuzzlePieceModifier.Protrusion)
            {
                startY -= maskHeight;
            }
        }


    }
}
