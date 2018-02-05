using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using JigsawGame;
using System.IO;

public class PuzzleCreationTests 
{
	[Test]
	public void PuzzleCreator_CanCreatePuzzleBoard() 
    {
		PuzzleBoard board = PuzzleCreator.CreateBoard(4);
		Assert.IsNotNull(board);
	}

    [Test]
    public void PuzzleCreator_CreatesPuzzleBoardWithCorrectNumberOfPieces()
    {
        for (ushort i = PuzzleCreator.MinPiecesInOneCol; i <= PuzzleCreator.MaxPiecesInOneCol; ++i)
        {
            PuzzleBoard board = PuzzleCreator.CreateBoard(i);
            Assert.AreEqual(board.Pieces.Length, i * i);
            Assert.AreEqual(board.NumRows, i);
            Assert.AreEqual(board.NumCols, i);
        }
    }

    [Test]
    public void PuzzleCreator_PuzzlePiecesAreNotNull()
    {
        PuzzleBoard board = PuzzleCreator.CreateBoard(4);
        foreach(var piece in board.Pieces)
        {
            Assert.IsNotNull(piece);
        }
    }

    [Test]
    public void PuzzleCreator_PuzzlePiecesHasFourSides()
    {
        PuzzleBoard board = PuzzleCreator.CreateBoard(4);
        foreach (var piece in board.Pieces)
        {
            Assert.AreEqual(piece.Sides.Length, 4);
        }
    }

    [Test]
    public void PuzzleCreator_PuzzlePieceSidesHaveValidTypeOfDents()
    {
        PuzzleBoard board = PuzzleCreator.CreateBoard(4);
        foreach (var piece in board.Pieces)
        {
            foreach(var side in piece.Sides)
            {
                Assert.IsTrue(side.Modifier == PuzzlePieceModifier.None 
                              || side.Modifier == PuzzlePieceModifier.Dent 
                              || side.Modifier == PuzzlePieceModifier.Protrusion);
            }
        }
    }

    [Test]
    public void PuzzleCreator_DoesNotAllowLessThanMinPieces()
    {
        for (ushort i = 0; i < PuzzleCreator.MinPiecesInOneCol; ++i)
        {
            PuzzleBoard board = PuzzleCreator.CreateBoard(i);
            Assert.IsNull(board);
        }
    }

    [Test]
    public void PuzzleCreator_AllowsUptoMaxPuzzlePieces()
    {
        PuzzleBoard board = PuzzleCreator.CreateBoard(PuzzleCreator.MaxPiecesInOneCol);
        Assert.IsNotNull(board);
    }

    [Test]
    public void PuzzleCreator_RestrictsToMaxNumberOfPieces()
    {
        PuzzleBoard board = PuzzleCreator.CreateBoard(PuzzleCreator.MaxPiecesInOneCol + 1);
        Assert.IsNull(board);
    }

    [Test]
    public void PuzzleCreator_ValidateA2ColumnBoardIsValid()
    {
        PuzzleBoard board = PuzzleCreator.CreateBoard(2);
        board.GeneratePuzzle();
        Assert.IsTrue(board.IsValid());
    }

    [Test]
    public void PuzzleCreator_AllGeneratedBoardArrangementsAreValid()
    {
        for (ushort i = PuzzleCreator.MinPiecesInOneCol; i <= PuzzleCreator.MaxPiecesInOneCol; ++i)
        {
            PuzzleBoard board = PuzzleCreator.CreateBoard(i);
            board.GeneratePuzzle();
            Assert.IsTrue(board.IsValid());
        }
    }

    [Test]
    public void BinaryMaskGenerator_ChecksForNullsProperly()
    {
        bool[][] binaryMask = BinaryMaskGenerator.GenerateBinaryMask(null);
        Assert.IsNull(binaryMask);
    }

    [Test]
    public void BinaryMaskGenerator_GeneratesNonNullMasksWhenGivenValidData()
    {
        Texture2D mask = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/modifier_mask.png", typeof(Texture2D));
        bool[][] binaryMask = BinaryMaskGenerator.GenerateBinaryMask(mask);
        Assert.IsNotNull(binaryMask);
    }

    [Test]
    public void BinaryMaskGenerator_MasksAreProperSize()
    {
        Texture2D mask = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/modifier_mask.png", typeof(Texture2D));
        bool[][] binaryMask = BinaryMaskGenerator.GenerateBinaryMask(mask);
        Assert.AreEqual(mask.width, binaryMask[0].Length);
        Assert.AreEqual(mask.height, binaryMask.Length);
    }

    [Test]
    public void BinaryMaskGenerator_GeneratesMasksWithValidValues()
    {
        Texture2D mask = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/modifier_mask.png", typeof(Texture2D));
        bool[][] binaryMask = BinaryMaskGenerator.GenerateBinaryMask(mask);
        for (int row = 0; row < mask.height; ++row)
        {
            for (int col = 0; col < mask.width; ++col)
            {
                Color colorInTexture = mask.GetPixel(col, row);
                Assert.IsTrue(colorInTexture == Color.black ? binaryMask[row][col] == false : binaryMask[row][col] == true);
            }
        }
    }

    [Test]
    public void BinaryMaskGenerator_ChecksForNullParamsBeforeTextureGeneration()
    {
        Texture2D texture = BinaryMaskGenerator.GenerateTextureFromBinaryMask(null);
        Assert.IsNull(texture);
    }

    [Test]
    public void BinaryMaskGenerator_ChecksForValidDimensionBeforeTextureGeneration()
    {
        bool[][] mask = new bool[10][];
        Texture2D texture = BinaryMaskGenerator.GenerateTextureFromBinaryMask(mask);
        Assert.IsNull(texture);
    }

    [Test]
    public void BinaryMaskGenerator_GeneratesTextureWhenValidValuesArePassed()
    {
        bool[][] mask = new bool[10][];
        for (int i = 0; i < mask.Length; ++i)
        {
            mask[i] = new bool[10];
        }
        Texture2D texture = BinaryMaskGenerator.GenerateTextureFromBinaryMask(mask);
        Assert.IsNotNull(texture);
    }

    [Test]
    public void BinaryMaskGenerator_GeneratesValidTextures()
    {
        Texture2D originalMask = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/modifier_mask.png", typeof(Texture2D));
        bool[][] binaryMask = BinaryMaskGenerator.GenerateBinaryMask(originalMask, 0);
        Texture2D generatedMask = BinaryMaskGenerator.GenerateTextureFromBinaryMask(binaryMask);

        var bytes = generatedMask.EncodeToPNG();
        var file = File.Open("Assets/Tests/Editor/Test_mask.png", FileMode.OpenOrCreate);
        var binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
    }

    // [Test]
    // public void PuzzlePieceCutter_CutsPiecesCorrectly()
    // {
    //     Texture2D targetImage = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/kitten.png", typeof(Texture2D));
    //     Texture2D originalMask = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/modifier_mask.png", typeof(Texture2D));
    //     PuzzleBoard board = PuzzleCreator.CreateBoard(2);
    //     Assert.IsTrue(PuzzleImageCutter.CutPieces(targetImage, board, originalMask));
    // }

}
