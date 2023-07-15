using System;
/*************************************************************************/
/* Program Name:     Ch10Ex8.cs                                          */
/* Date:             04/18/2022                                          */
/* Programmer:       Miranda Morris                                      */
/* Class:            CSCI 234                               		     */
/*                                                         			     */
/* Program Description: The purpose of this program is to perform a    	 */
/* variety of different arithmetic with fractons such as addition,       */
/* subtraction, muiltiplication, and division.                           */
/*                                                                       */
/* Input: a rational number that serves as numerator & a rational number */
/* that serves as the denominator.                                       */
/*                                                                       */
/* Output: whether the fractions are equal, less than, greater than,	 */
/* the addition of the two, the subtraction of the two, the              */
/* muiltiplication of the two, and the division of the two.              */
/*                                                                       */
/* Givens: None 							                             */
/*                                                                       */
/* Testing Data:                                                         */
/*                                                                       */
/* List the Testing Input Data:                                          */
/*                  Test 1: rational number r: 1 & rational number s: 2  */
/*                  Test 2: rational number r: 1 & rational number s:0   */
/*                                                                       */
/* List the Testing Output Data:                                         */
/*                  Test 1: r==s: False                                  */
/*                          r<s: True                                    */
/*                          r>s: False                                   */
/*                          r+s: 3                                       */
/*                          r-s: -1                                      */
/*                          r*s: 2                                       */
/*                          r/s: 1/2                                     */
/*                   Test 2: r==s: False                                 */
/*                           r<s: False                                  */
/*                           r>s: True                                   */
/*                           r+s: 1                                      */
/*                           r-s: 1                                      */
/*                           r*s: 0                                      */
/*   Unhandled Exception: System.DivideByZeroException: Rational may not */
/*   have zero denominator                                               */
/*************************************************************************/

public class Rational : IComparable
{
    long numerator; //creating the instance of a numerator
    long denominator; //creating the instance of a denonimator

    //makes the rational a readonly
    public static readonly Rational ZERO;

    //creating the properties of numerator & denonimator
    public long Numerator
    {
        get { return numerator; }
    }

    public long Denominator
    {
        get { return denominator; }
    }

    //creating the constructors for the instances

    //creates a fraction with the denonimator of 1
    public Rational(long numerator)
    {
        this.numerator = numerator;
        denominator = 1;
    }
    //creates a fraction in its lowest terms
    public Rational (long numerator, long denominator)
    {
        //cannot have a 0 in the denominator
        if (denominator == 0) throw new DivideByZeroException("Rational may not have zero denominator");

        //inverts the signs if the denominator is negative
        if (denominator < 0)
        {
            denominator = -denominator;
            numerator = -numerator;
        }

        //find gcd of the numerator and denominator
        long g = Math.Abs(gcd(numerator, denominator));

        //divide them
        this.numerator = numerator / g;
        this.denominator = denominator / g;
    }

    //create a rational from a string
    public Rational(string s)
    {
        //split at the / character
        char[] delim = { '/' };
        string[] numDen = s.Split(delim);

        //only allow 1 or 2 terms to be in this array
        if (numDen.Length < 1 || numDen.Length > 2)
            throw new FormatException("Rational number not in proper m/n format");

        //if there is a denominator then assign both a numerator and denominator
        if (numDen.Length != 1)
        {
            numerator = Int64.Parse(numDen[0]);
            denominator = Int64.Parse(numDen[1]);
        }

        //if there is no denominator specified then set it to 1
        else
        {
            numerator = Int64.Parse(numDen[0]);
            denominator = 1;
        }

        //put the fraction into lowest terms
        if (denominator == 0) throw new DivideByZeroException("Rational may not have a zero denominator");
        if (denominator < 0)
        {
            denominator = -denominator;
            numerator = -numerator;
        }
        long g = Math.Abs(gcd(numerator, denominator));
        numerator = numerator / g;
        denominator = denominator / g;
    }

    //create a static constructor

    static Rational()
    {
        ZERO = new Rational(0, 1);
    }

    //create the methods
    //creates the gcd method
    private static long gcd(long a, long b)
    {
        if (b == 0) return a;
        return gcd(b, a % b);
    }
    //creates the method to find the least common muiltiple
    private static long lcm(long a, long b)
    {
        return a * b / gcd(a, b);
    }
    //test whether they are equal
    public bool Equals(Rational r)
    {
        return numerator == r.numerator && denominator == r.denominator;
    }

    //creates the comparing of two objects using the IComparable
    public int CompareTo(Object o)
    {
        Rational r = (Rational)o;

        //find the gcd of the 2 denominators
        long g = gcd(denominator, r.denominator);

        //cross muiltiply them by the gcd
        long n1 = numerator * r.denominator / g;
        long n2 = denominator * r.numerator / g;

        //compare the two values
        //if this is smaller
        if (n1 < n2) return -1;
        //if o is smaller
        if (n1 > n2) return 1;
        //if this and o are equal
        return 0;
    }

    //creating the methods to do rational arithmetic

    private Rational Add(Rational r)
    {
        //gives the common denominator of the two fractions
        //use cross muiltiplying
        //add the numerators 
        //reduce to the lowest terms using a constructor
        return new Rational(numerator * r.denominator + r.numerator * denominator, denominator * r.denominator);
    }

    private Rational Subtract(Rational r)
    {
        //similar to the add, but the numerators are subtracted
        return new Rational(numerator * r.denominator - r.numerator * denominator, denominator * r.denominator);
    }

    private Rational Muiltiply(Rational r)
    {
        //muiltiply the numerators and denominators together
        return new Rational(numerator * r.numerator, denominator * r.denominator);
    }

    private Rational Divide(Rational r)
    {
        //flip the second fraction and muiltiply the two
        return new Rational(numerator * r.denominator, denominator * r.numerator);
    }

    //parses a string into a Rational by calling the Rational(String) constructor
    public static Rational Parse (String s)
    {
        return new Rational(s);
    }
    //converts a string into a Rational which overrides the ToString() from an object
    public override string ToString()
    {
        if (denominator == 1) return numerator.ToString();
        else return numerator + "/" + denominator;
    }

    //creating the operators

    //comparing with CompareTo or Equals

    public static bool operator <(Rational r, Rational s)
    {
        return (r.CompareTo(s) < 0);
    }

    public static bool operator >(Rational r, Rational s)
    {
        return (r.CompareTo(s) > 0);
    }

    public static bool operator <=(Rational r, Rational s)
    {
        return !(r.CompareTo(s) > 0);
    }

    public static bool operator >=(Rational r, Rational s)
    {
        return !(r.CompareTo(s) < 0);
    }
     public static bool operator ==(Rational r, Rational s)
    {
        return r.Equals(s);
    }

    public static bool operator !=(Rational r, Rational s)
    {
        return !r.Equals(s);
    }

    //the arithmetic operators, calling the add,subtract, muiltiply, & divide 
    public static Rational operator +(Rational r, Rational s)
    {
        return r.Add(s);
    }

    public static Rational operator -(Rational r, Rational s)
    {
        return r.Subtract(s);
    }

    public static Rational operator *(Rational r, Rational s)
    {
        return r.Muiltiply(s);
    }

    public static Rational operator /(Rational r, Rational s)
    {
        return r.Divide(s);
    }

}

public class TestRational
{
    static void Main()
    { 
        Console.Write("Enter a rational number r: ");
        Rational r = new Rational(Console.ReadLine());
        Console.WriteLine("r:" + r);
        Console.Write("Enter a rational number s: ");
        Rational s = new Rational(Console.ReadLine());
        Console.WriteLine("s: " + s);
        Console.WriteLine("r==s: " + (r == s));
        Console.WriteLine("r<s: " + (r < s));
        Console.WriteLine("r>s: " + (r > s));
        Console.WriteLine("r+s: " + (r + s));
        Console.WriteLine("r-s: " + (r - s));
        Console.WriteLine("r*s: " + r * s);
        Console.WriteLine("r/s: " + r / s);
    }
}


