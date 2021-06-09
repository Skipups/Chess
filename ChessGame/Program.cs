using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessGame
{
    class Program
    {
        // private static string userInput;
        static void Main(string[] args)
        {
            var ui = new ConsoleUI();
            var gameEngine = new GameEngine(ui, ui);
            gameEngine.Run();
        }
    }
}
        //static void OldMain(string[] args)
        //{
           
        //   string userInput = "abc";
        //    Board board = new Board();
        //    var game = new GameState("Svetlana","Mike", board);
        //    string moveStatusForDisplay = null;

        //    do
        //    {
        //        Console.Clear();
        //        DrawCapturedList(game.PlayerBlack.Name, game.PlayerBlack.CapturedList);
        //        game.Board.DrawBoard();


        //        DrawCapturedList(game.PlayerWhite.Name, game.PlayerWhite.CapturedList);
        //        DrawMoveUpdate(moveStatusForDisplay);
        //        DrawMenu();
        //        userInput = GetInput();
        //        if (userInput != "q")
        //        {
        //            game.ShowWhoseTurn();
        //            Console.WriteLine("Type the start coordinate of the piece you want to move: \nExample type 0,6 ");
        //            var startCoord = Console.ReadLine();
        //            var IsStartCoord = true;
        //            Coord startCoordParsed = Menu.ParseMoveString(startCoord);
             
        //            var startCoordCantBeParsed = startCoordParsed.Equals(Coord.InvalidCoordinate);
        //            while(startCoordCantBeParsed || !game.Board.IsValidMoveCoord(startCoordParsed, IsStartCoord, game.TurnWhite))
        //            {
        //                Console.WriteLine($"{startCoordParsed} is not a valid input. Type the start coordinate of the piece you want to move: \nExample type 0,6");
        //                startCoord = Console.ReadLine();
                        
                        
        //                startCoordParsed = Menu.ParseMoveString(startCoord);
        //                startCoordCantBeParsed = startCoordParsed.Equals(Coord.InvalidCoordinate);
        //            }
        //            IsStartCoord = false;
        //            Console.WriteLine("Type the destination coordinate.\nExample type 0,5 ");
        //            var endCoord = Console.ReadLine();
        //            Coord endCoordParsed = Menu.ParseMoveString(endCoord);

        //            var endCoordCantBeParsed = startCoordParsed.Equals(Coord.InvalidCoordinate);
        //            while (endCoordCantBeParsed || !game.Board.IsValidMoveCoord(endCoordParsed, IsStartCoord, game.TurnWhite))
        //            {
                        
        //                Console.WriteLine("Invalid input. Type the destination coordinate.\nExample type 0,5 ");
        //                endCoord = Console.ReadLine();
        //                endCoordParsed = Menu.ParseMoveString(endCoord);
        //                endCoordCantBeParsed = startCoordParsed.Equals(Coord.InvalidCoordinate);
        //            }
                  
        //            try
        //            {
        //                var moveResult = game.Move(startCoordParsed, endCoordParsed);

        //                //check for capture
        //                var capturedPieceForDisplayIfNotNull = (moveResult.CapturedPiece != null) ? $" A {moveResult.CapturedPiece.DisplayPieceInfo} was captured"  : null;
        //                moveStatusForDisplay = $"Move from {startCoordParsed} to {endCoordParsed} was successfully executed.{capturedPieceForDisplayIfNotNull}";

        //                //check for promotion opportunity
        //                if (moveResult.PawnToPromote != null)
        //                {
        //                    //tell player they can promote this piece -> need to know coord, end
        //                    //show menu for them to select piece from other players captured list
                           
        //                    //draw list of white turn drawy player black captured list

        //                    var listToUpdate = game.TurnWhite ? game.PlayerBlack.CapturedList : game.PlayerWhite.CapturedList;

        //                    var pieceToPromote = GetPieceToPromote(listToUpdate);
        //                     game.Promote(endCoordParsed, pieceToPromote, moveResult);
        //                    //display message promotion was successful
        //                }
        //                if (moveResult.CapturedPiece.IsPieceCheckable != null)
        //                {
        //                    Console.WriteLine("You win");
        //                    userInput = "q";
        //                }
                     
        //                game.TurnWhite = !game.TurnWhite;
        //            }
        //            catch(InvalidMoveException ex)
        //            {
        //                moveStatusForDisplay = ex.Message; 
        //            }
        //        }
        //    }
        //    while (userInput != "q");
        //}
       
//       private static string GetInput()
//        {
//            Console.WriteLine();
          
//            var userInput = Console.ReadLine();
//            Console.WriteLine();
//            return userInput;
//        }
//        private static void DrawMenu()
//        {
//            Console.WriteLine(" Menue:");
//            Console.WriteLine(" q-- quit   m-- move");

//        }
//        private static void DrawMoveUpdate(string moveStatus =null)
//        {
//            Console.WriteLine(moveStatus);
          

//        }
//        private static Piece GetPieceToPromote(List<Piece> capturedPieces)
//        {
//            var pieceDict = new SortedDictionary<int, Piece>();
//            foreach (var capPiece in capturedPieces)
//            {
//                if(capPiece.IsPiecePromotable == null)
//                {
//                    pieceDict.Add(pieceDict.Count + 1, capPiece);
//                }
//            }
//            Piece toReturn = null;
//            while (toReturn == null)
//            {
//               // int count = this.CapturedList.Count();
//                StringBuilder line = new StringBuilder();
//                line.Append("    "); // 

//                //line.Append(($"{ this.Name}'s Captured Piece List: "));
//                foreach (var kvp in pieceDict)
//                {
//                    line.Append($"{kvp.Key}. {kvp.Value.DisplayPieceInfo} ");
//                }
//                Console.WriteLine(line);
//                Console.WriteLine("Pawn Promotion! Type the number of the piece you want to promote:");
//                var pieceNumberSelection = Console.ReadLine();
//                int pieceNumberParsed = Menu.ParsePromotionSelectionString(pieceNumberSelection);
//                if(!pieceDict.TryGetValue(pieceNumberParsed, out toReturn))
//                {
//                    Console.WriteLine("Invalid selection");
//                }
//            }
//            return toReturn;
            
//        }
//        private static void DrawCapturedList(string name, List<Piece> capturedList)
//        {
//            int count = capturedList.Count();
//            int numOfPawns = 0;
//            StringBuilder line = new StringBuilder();
//            line.Append("    "); // 
//            line.Append(($"{name}'s Captured Piece List: "));
            
//            for (int i = 0; i < count; ++i)
//            {
//                if (capturedList[i].IsPiecePromotable !=null)
//                {
//                    numOfPawns++;
//                }
//                else
//                {
//                    line.Append($"{i + 1}. {capturedList[i].DisplayPieceInfo} ");
//                }
               
//            }
//            line.Append($" (Number of captured pawns {numOfPawns})");

//            Console.WriteLine(line);
//        }

//    }
//}
/*
 * 
 * questions:
 * paesePromotinSelectionString - good practice to make another?
 * dictionary/ list for the capturedList
 *  - keeping track of the number user selected and updating 
TODO:
capture:
start piece takes end piece position
caputured piece added to List<Piece> capturedWhite
caputured piece added to List<Piece> capturedBlack
Lists belong to player of opposing color

Board.Move =>   return Piece or null or throws an exception
UI displays - "Move successful" "Piece xyz captured"
gameState.Capture(Piece capturedPiece) => adds to List
UI redraw and display captured pieces on side of board

Menu-> add all exceptions you can think of, then Exception
Test cases

1. Pawn Capture- 
in Board.Move if Pawn reaches opposite row.
2. King capture
4. check detection- call move on each piece and check if king is in it's path, don't execute the actual move though

*/
//var startCoord = Console.ReadLine();
//(int, int) coords = Menu.ParseMoveString(userInput);
//((int, int), (int, int)) coords = Menu.ParseMoveString(userInput);
//(int, int) startCoord = coords.Item1;
//(int, int) endCoord = coords.Item2;


//Console.WriteLine("Welcome to Chess! Please enter your name and the second players name:");
//string player1Name = Console.ReadLine();
//string player2Name = Console.ReadLine();



//game.showPuzzle();
//game.timeToGuess();
//string guess = Console.ReadLine();
//game.validateInput(guess);


//initiate Game
//var myGame = new GameState(player1Name, player2Name);
//myGame.InitiateGame();
//DrawMenu();
//GetInput();

//var game = new GameState(player1Name, player2Name);