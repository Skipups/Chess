using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    class Menu
    {
        /// <summary>
        /// This is an example of how you would turn what the user 
        /// typed into a set of coordinates.  If the user
        /// </summary>
        /// <param name="whatTheUserTyped"></param>
        /// <returns></returns>
        public static Coord ParseMoveString(string whatTheUserTyped)
        {

           try
            {
                string[] userInputAsArray =  whatTheUserTyped.Split(',');
                Coord coord = new Coord(Int32.Parse(userInputAsArray[0]), Int32.Parse(userInputAsArray[1]));

                return coord;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a coordinate value. Error message: {e}");
                return Coord.InvalidCoordinate;

            }
            catch (System.FormatException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter coordinates in the correct format value. Error message: {e}");
                return Coord.InvalidCoordinate;

            }
            catch (System.OverflowException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter coordinates within the coordinates of the board. Error message: {e}");
                return Coord.InvalidCoordinate;

            }
            catch (Exception e)
            {
               Console.WriteLine($"You typed: {whatTheUserTyped}. This is an incorrect input format. Error message: {e}");
                return Coord.InvalidCoordinate;

            }

        }

        public static int ParsePromotionSelectionString(string whatTheUserTyped)
        {

            try
            {
                string[] userInputAsArray = whatTheUserTyped.Split();
                var stringBuilder = new StringBuilder();
                int returnedInt;
               
               
                foreach (var x in userInputAsArray)
                {
                  
                    bool conversionSucceed = int.TryParse(x, out returnedInt);
                    stringBuilder.Append(returnedInt);
                }

               int.TryParse(stringBuilder.ToString(), out returnedInt);

                return returnedInt;
            
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a value. Error message: {e}");
                return -1;

            }
            catch (System.FormatException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a number in the correct format value. Error message: {e}");
                return -1;

            }
            catch (System.OverflowException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a smaller number. Error message: {e}");
                return -1;

            }
            catch (Exception e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. This is an incorrect input format. Error message: {e}");
                return -1;

            }

        }

        public static int ParseUserSavedGameSelection(string whatTheUserTyped)
        {

            try
            {
                int returnedInt;
                int.TryParse(whatTheUserTyped, out returnedInt);

                return returnedInt;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a value. Error message: {e}");
                return -1;

            }
            catch (System.FormatException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a number in the correct format value. Error message: {e}");
                return -1;

            }
            catch (System.OverflowException e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. Please enter a smaller number. Error message: {e}");
                return -1;

            }
            catch (Exception e)
            {
                Console.WriteLine($"You typed: {whatTheUserTyped}. This is an incorrect input format. Error message: {e}");
                return -1;

            }

        }

        //public static ((int, int), (int, int)) ParseMoveString(string whatTheUserTyped)
        //{
        //    // if you wanted to move from 1,2 to 3, 4 you would enter in:
        //    // 1,2;3,4
        //    string[] startAndEnd = whatTheUserTyped.Split(';');
        //    (int, int) startCoord = (Int32.Parse(startAndEnd[0].Split(',')[0]), Int32.Parse(startAndEnd[0].Split(',')[1]));
        //    (int, int) endCoord = (Int32.Parse(startAndEnd[1].Split(',')[0]), Int32.Parse(startAndEnd[1].Split(',')[1]));
        //    return (startCoord, endCoord);
        //}
    }
}
