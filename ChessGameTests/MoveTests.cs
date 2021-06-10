using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ChessGame;

//namespace ChessGameTests
//{
//    [TestClass]
//    public class MoveTests
//    {
//        protected GameState GenerateNewGameState()
//        {
//            Board board = new Board();
//            var game = new GameState("Svetlana", "Mike", board);
//            return game;
//        }
//        protected GameState GenerateInProgressGameState()
//        {
//            Board board = new Board();
//            var game = new GameState("Svetlana", "Mike", board);
//            game.Move(new Coord(1, 6), new Coord(1, 4)); // W
//            game.Move(new Coord(0, 1), new Coord(0, 3)); // B
//            game.Move(new Coord(1, 4), new Coord(1, 3)); // W
//            game.Move(new Coord(2, 1), new Coord(2, 2)); // B
//            game.Move(new Coord(2, 6), new Coord(2, 4)); // W
          

//            return game;
//        }
//        [TestMethod]
//        public void StartEndCoords_HappyPath()
//        {
//            var game = GenerateNewGameState();
//            var start = new Coord(0, 6);
//            var end = new Coord(0, 5);

//            var pieceAtStart = game.Board.GetPieceFromCoord(start);
//            Assert.IsNull(game.Board.GetPieceFromCoord(end));

//            var moveResult = game.Move(start, end);
//            var pieceAfterMove = game.Board.GetPieceFromCoord(end);
//            Assert.AreEqual(pieceAtStart, pieceAfterMove);
//            Assert.IsNull(game.Board.GetPieceFromCoord(start));

//        }
//        [TestMethod]
//        [ExpectedException(typeof(InvalidMoveException))]
//        public void StartEndCoords_AreTheSame()
//        {
//            var game = GenerateNewGameState();
//            var start = new Coord(0, 6);
//            var end = new Coord(0, 6);

//            var moveResult = game.Move(start, end);
//        }
//        [TestMethod]
//        public void StartEndPiecesOppositeColors_HappyPath()
//        {    //white pawn capturing black pawn
//            var game = GenerateInProgressGameState();
//            var start = new Coord(1, 3);
//            var end = new Coord(2, 2);

//            var pieceOnStartCoord = game.Board.GetPieceFromCoord(start);
//            var pieceToBeCaptured = game.Board.GetPieceFromCoord(end);

//            var moveResult = game.Move(start, end);

//            Assert.AreEqual(pieceToBeCaptured, moveResult.CapturedPiece);

//            Assert.IsTrue(game.PlayerWhite.CapturedList.Contains(pieceToBeCaptured));
//            Assert.IsTrue(game.PlayerBlack.CapturedList.Count ==0); 


//        }
//        [TestMethod]
//        [ExpectedException(typeof(InvalidMoveException))]
//        public void StartEndPiecesOppositeColors_AreTheSame()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(2, 4);
//            var end = new Coord(1, 3);

//            var actual = game.Move(start, end);
//        }
//        #region ValidateMove by Piece type
//        [TestMethod]
//        public void ValidateMove_Pawn2SquaresFromInitialRow()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(7, 6);
//            var end = new Coord(7, 4);
//            var startPiece = game.Board.GetPieceFromCoord(start);
//                game.Move(start, end);
//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(InvalidMoveException))]
//        public void ValidateMove_Pawn2SquaresNotFromInitialRow()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(2, 4);
//            var end = new Coord(2, 2);
    
//            var actual = game.Move(start, end);
       
//        }
//        [TestMethod]
//        public void ValidateMove_PawnSingleSquare()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(3, 1);
//            var end = new Coord(3, 2);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//                            game.Move(start, end);
//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
 
    
//          [TestMethod]
//        [ExpectedException(typeof(InvalidMoveException))]
//        public void ValidateMove_Diagonal_Illegal_NothingToCapture()
//        {
//            var game = GenerateInProgressGameState();
        
//            game.Move(new Coord(2, 2), new Coord(2, 3));
//            var startPiece = game.Board.GetPieceFromCoord(new Coord(2, 3));

//            //confirm no diagonal piece present
//            var pieceToCapture = game.Board.GetPieceFromCoord(new Coord(1, 4));
//            Assert.IsNull(pieceToCapture);

//            game.Move(new Coord(2, 3), new Coord(1, 4));
       

//        }

//        [TestMethod]
//        public void ValidateMove_Diagonal_HappyPath()
//        {
//            var game = GenerateInProgressGameState();

//            game.Move(new Coord(3, 1), new Coord(3, 3));
//            var startPiece = game.Board.GetPieceFromCoord(new Coord(3, 3));
      
//            //confirm there is a diagonal piece elidgible for capture
//            var pieceToCapture = game.Board.GetPieceFromCoord(new Coord(2, 4));
//            Assert.IsNotNull(pieceToCapture);

//            game.Move(new Coord(3, 3), new Coord(2, 4));
//            Assert.AreEqual(startPiece, game.Board.GetPieceFromCoord(new Coord(2, 4)));
//        }

//        [TestMethod]
//        public void ValidateMove_Knight_2Vert1Hor()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(1, 7);
//            var end = new Coord(0,5);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//                            game.Move(start, end);
//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void ValidateMove_Knight_2Hor1Vert()
//        {      
//            var game = GenerateInProgressGameState();
//            //move pawn out of Knight's end coord
//            game.Move(new Coord(3, 6), new Coord(3, 4));
//            var start = new Coord(1, 7);
//            var end = new Coord(3, 6);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            game.Move(start, end);
//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void ValidateMove_Bishop()
//        {
//            var game = GenerateInProgressGameState();
//            //move pawn out of Bishop's path
//            game.Move(new Coord(4, 1), new Coord(4, 3));
//            var start = new Coord(5, 0);
//            var end = new Coord(3, 2);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            game.Move(start, end);
//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void ValidateMove_Rook_Hor()
//        {
//            var game = GenerateInProgressGameState();
//            //move knight
//            game.Move(new Coord(1, 7), new Coord(0, 5));

//            var start = new Coord(0, 7);
//            var end = new Coord(1, 7);
//            var startPiece = game.Board.GetPieceFromCoord(start);
//            game.Move(start, end);
//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        public void ValidateMove_Rook_Vert()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(0, 7);
//            var end = new Coord(0, 5);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            //move pawn
//            game.Move(new Coord(0, 6), new Coord(0, 4));

//            var endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void ValidateMove_King()
//        {
//            var game = GenerateInProgressGameState();
//            //move surrounding pieces out of the way
//            game.Move(new Coord(3, 1), new Coord(3, 3));
//            game.Move(new Coord(3, 3), new Coord(3, 4));
//            game.Move(new Coord(4, 1), new Coord(4, 3));
//            game.Move(new Coord(5, 1), new Coord(5, 3));
//            game.Move(new Coord(5, 0), new Coord(3, 2));
//            game.Move(new Coord(3, 0), new Coord(2, 1));

//           //king
//            var startPiece = game.Board.GetPieceFromCoord(new Coord(4,0));
            
//            //move horizontally 1 forward
//            game.Move(new Coord(4, 0), new Coord(3, 0));
//            //move horizontally 1 back
//            game.Move(new Coord(3, 0), new Coord(4, 0));
//            //move vertically 1 forward 
//            game.Move(new Coord(4, 0), new Coord(4, 1));
//            //move vertically 1 back
//            game.Move(new Coord(4, 1), new Coord(4, 2));
//            game.Move(new Coord(4, 2), new Coord(4, 1));
//            //move diagonally
//            game.Move(new Coord(4, 1), new Coord(3, 0));
//            Assert.AreEqual(startPiece, game.Board.GetPieceFromCoord(new Coord(3, 0)));
//        }

//        [TestMethod]
//        [ExpectedException(typeof(InvalidMoveException))]
//        public void ValidateMove_King_InvalidMove_MoreThanOneSquare()
//        {
//            var game = GenerateInProgressGameState();
//            //move surrounding pieces out of the way
//            game.Move(new Coord(3, 1), new Coord(3, 3));
//            game.Move(new Coord(3, 3), new Coord(3, 4));
//            game.Move(new Coord(4, 1), new Coord(4, 3));
//            game.Move(new Coord(5, 1), new Coord(5, 3));
//            game.Move(new Coord(5, 0), new Coord(3, 2));
//            game.Move(new Coord(3, 0), new Coord(2, 1));

//            //king
//            var startPiece = game.Board.GetPieceFromCoord(new Coord(4, 0));

//            //move horizontally 2 forward
//            game.Move(new Coord(4, 0), new Coord(4, 2));
           
//        }

//        [TestMethod]
//        public void Queen_Horizontal_OneSQuare()
//        {
//            var game = GenerateInProgressGameState();
//            //move pawn out of way
//            game.Move(new Coord(3, 1), new Coord(3, 3));
//            game.Move(new Coord(3, 3), new Coord(3, 4));

//            var start = new Coord(3, 0);
//            var end = new Coord(3, 1);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            var endPiece = game.Board.GetPieceFromCoord(end);
//            Assert.IsNull(endPiece);
//            //move forward one square
//            game.Move(start, end);
//            endPiece = game.Board.GetPieceFromCoord(end);
//            Assert.AreEqual(startPiece, endPiece);

//            //move backwards one square
//            startPiece= game.Board.GetPieceFromCoord(new Coord(3, 1));
//            game.Move(new Coord(3, 1), new Coord(3, 0));
//            endPiece = game.Board.GetPieceFromCoord(new Coord(3, 0));
//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void Queen_Horizontal_MultipleSQuares()
//        {
//            var game = GenerateInProgressGameState();
//            //move pawn out of way
//            game.Move(new Coord(3, 1), new Coord(3, 3));
//            game.Move(new Coord(3, 3), new Coord(3, 4));

//            var start = new Coord(3, 0);
//            var end = new Coord(3, 3);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            var endPiece = game.Board.GetPieceFromCoord(end);
//            Assert.IsNull(endPiece);
//            //move forward 3 squares
//            game.Move(start, end);
//            endPiece = game.Board.GetPieceFromCoord(end);
//            Assert.AreEqual(startPiece, endPiece);

//            //move backwards 2 squares
//            startPiece = game.Board.GetPieceFromCoord(new Coord(3, 3));
//            game.Move(new Coord(3, 3), new Coord(3, 1));
//            endPiece = game.Board.GetPieceFromCoord(new Coord(3, 1));
//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void Queen_Diagonal_OneSQuare()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(3, 0);
//            var end = new Coord(2, 1);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            var endPiece = game.Board.GetPieceFromCoord(end);
//            Assert.IsNull(endPiece);

//            game.Move(start, end);
//            endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        public void Queen_Diagonal_MultipleSQuares()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(3, 0);
//            var end = new Coord(1, 2);

//            var startPiece = game.Board.GetPieceFromCoord(start);
//            var endPiece = game.Board.GetPieceFromCoord(end);
//            Assert.IsNull(endPiece);

//            game.Move(start, end);
//            endPiece = game.Board.GetPieceFromCoord(end);

//            Assert.AreEqual(startPiece, endPiece);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(InvalidMoveException))]
//        public void Queen_InvalidMove_MultipleSQuares()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(3, 0);
//            var end = new Coord(1, 3);

//            game.Move(start, end);
//        }
//        //IsPathClear
//        //GetPiece
//#endregion

//        #region Capture
//        [TestMethod]
//        public void CaputuredPiece_ByWhitePlayer_HappyPath()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(1, 3);
//            var end = new Coord(2, 2);

        
//            var pieceAtStartCoord = game.Board.GetPieceFromCoord(start);
//            var pieceAtEndCoord = game.Board.GetPieceFromCoord(end);
//            Assert.IsFalse(game.PlayerWhite.CapturedList.Contains(pieceAtEndCoord));

//            var moveResult = game.Move(start, end);
            
//            Assert.IsTrue(game.PlayerWhite.CapturedList.Contains(moveResult.CapturedPiece));
//            Assert.IsTrue(game.PlayerBlack.CapturedList.Count == 0);
//            Assert.AreEqual(pieceAtStartCoord, game.Board.GetPieceFromCoord(end));

//        }

//        [TestMethod]
//        public void CaputuredPiece_ByBlackPlayer_HappyPath()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(2, 2);
//            var end = new Coord(1, 3);

//            var pieceAtStartCoord = game.Board.GetPieceFromCoord(start);
//            var pieceAtEndCoord = game.Board.GetPieceFromCoord(end);
//            Assert.IsFalse(game.PlayerBlack.CapturedList.Contains(pieceAtEndCoord));

//            var moveResult = game.Move(start, end);

//            Assert.IsTrue(game.PlayerBlack.CapturedList.Contains(moveResult.CapturedPiece));
//            Assert.IsTrue(game.PlayerWhite.CapturedList.Count == 0);
//            Assert.AreEqual(pieceAtStartCoord, game.Board.GetPieceFromCoord(end));
//        }

//        [TestMethod]
//        public void NoPieceCaputured()
//        {
//            var game = GenerateInProgressGameState();
//            var start = new Coord(2, 2);
//            var end = new Coord(2, 3);

//            var moveResult = game.Move(start, end);

//            Assert.IsTrue(game.PlayerWhite.CapturedList.Count == 0);
//            Assert.IsTrue(game.PlayerBlack.CapturedList.Count ==0);
//            Assert.IsNull(moveResult.CapturedPiece);
//        }

//        [TestMethod]
//        public void PawnPromotion_HappyPath()
//        {
//            var game = GenerateInProgressGameState();

//            Assert.IsTrue(game.PlayerWhite.CapturedList.Count == 0);
//            Assert.IsTrue(game.PlayerBlack.CapturedList.Count == 0);

//            // w pawn capture b pawn
//            game.Move(new Coord(7, 6), new Coord(7, 4));
//            game.Move(new Coord(6, 1), new Coord(6, 3));
//            var pawnToCapture = game.Board.GetPieceFromCoord(new Coord(6, 3));
//           var moveResult = game.Move(new Coord(7, 4), new Coord(6, 3));
//            Assert.AreSame(moveResult.CapturedPiece, pawnToCapture);
//            Assert.IsTrue(game.PlayerWhite.CapturedList.Contains(pawnToCapture));

//            game.Move(new Coord(6, 0), new Coord(7, 2)); //black knight moved
//            game.Move(new Coord(6, 3), new Coord(7, 2)); // white pawn capture black knight
//            game.Move(new Coord(6, 7), new Coord(5, 5)); // white  knight
//            game.Move(new Coord(5, 1), new Coord(5, 2)); // black pawn
//            game.Move(new Coord(5, 5), new Coord(6, 3)); // white  knight
//            game.Move(new Coord(5, 2), new Coord(5, 3)); // black pawn 
//            game.Move(new Coord(5,3), new Coord(5, 4)); // black pawn
//            game.Move(new Coord(5, 4), new Coord(5, 5)); //black pawn
//            game.Move(new Coord(5, 5), new Coord(6, 6)); //black pawn capture white pawn
//           var promotionMoveResult = game.Move(new Coord(6, 6), new Coord(6, 7)); // back pawn promotion

//            Assert.IsNull(promotionMoveResult.CapturedPiece);
//            Assert.AreSame(promotionMoveResult.PawnToPromote, game.Board.GetPieceFromCoord(new Coord(6, 7)));
//            Assert.AreEqual(game.PlayerWhite.CapturedList.Count, 2);
//            Assert.AreEqual(game.PlayerBlack.CapturedList.Count, 1);

//        }
//        #endregion
//    }
//}

