using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ChessGame
{

    public class CoordTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }

            return false;
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if(value is string)
            {
                string valueAsString = (string)value;
                return new Coord(int.Parse(valueAsString.Substring(1, 1)), int.Parse(valueAsString.Substring(3, 1)));
            }

            return null;
        }
    }

    [TypeConverter(typeof(CoordTypeConverter))]
    [DataContract()]
    public class Coord : IEquatable<Coord>
    {
        [DataMember]
        public int Y;
        [DataMember]
        public int X;
        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Coord( Coord other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }


        public static Coord InvalidCoordinate = new Coord(-1, -1);

        public bool Equals(Coord other)
        {
            if (other == null)
            {
                return false;
            }
            return this.X == other.X && this.Y == other.Y;
        }

        public override string ToString()
        {
            return $"({this.X},{this.Y})";
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Coord)
            {
                return this.Equals((Coord)obj);
            }

            return false;
        }

        public static bool operator == (Coord coord1, Coord coord2)
        {
            if (((object)coord1) == null || ((object)coord2) == null) 
                return Object.Equals(coord1, coord2); 
            return coord1.Equals(coord2);
        }
        public static bool operator !=(Coord coord1, Coord coord2)
        {
            if (((object)coord1) == null || ((object)coord2) == null)
                return !Object.Equals(coord1, coord2);
            return !coord1.Equals(coord2);
        }

        /// <summary>
        /// This returns all elements in the path starting at endpoint1 and ending at endpoint2.
        /// Results include endpoint1 and endpoint2. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IList<Coord> CalculatePath(Coord start, Coord end)
        {
            // first validate that it's a path
            // vertical line means the X's are equal
            // horizontal line means the Y's are equal
            // diagonal line means the abs(x1-x2) == abs(y1-y2)
            if ((start.X != end.X) && (start.Y != end.Y) && (Math.Abs(start.X-end.X) != Math.Abs(start.Y - end.Y)))
            {
                throw new InvalidMoveException("Path must be vertical, diagonal, or horizontal");
            }

            // now calculate the path
            List<Coord> results = new List<Coord>();
            results.Add(start);
            Coord lastEndPoint = start;
            while (!lastEndPoint.Equals(end))
            {
                Coord currentEndPoint = new Coord(lastEndPoint);
                if (currentEndPoint.X < end.X)
                {
                    currentEndPoint.X++;
                }
                else if (currentEndPoint.X > end.X)
                {
                    currentEndPoint.X--;
                }

                if (currentEndPoint.Y < end.Y)
                {
                    currentEndPoint.Y++;
                }
                else if (currentEndPoint.Y > end.Y)
                {
                    currentEndPoint.Y--;
                }

                results.Add(currentEndPoint);
                lastEndPoint = currentEndPoint;
            }

            return results;
        }
    }
    public class CastlePieces
    {
        public Piece King;
        public Piece Rook;
        public CastlePieces(Piece king, Piece rook)
        {
            King = king;
            Rook = rook;
        }
    }


}