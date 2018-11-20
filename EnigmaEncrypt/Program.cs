using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaEncrypt
{
    class Program
    {
        //// Rotor1 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JGDQOXUSCAMIFRVTPNEWKBLZYH - (Permutation)
        //static int[] encryptArrayRotor1 = new int[]
        //{
        //    9, 6, 3, 16, 14, 23, 20, 18, 2, 0,
        //    12, 8, 5, 17, 21, 19, 15, 13, 4, 22,
        //    10, 1, 11, 25, 24, 7
        //};

        // Rotor1 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JGDQOXUSCAMIFRVTPNEWKBLZYH - (Permutation)
        static char[] encryptArrayRotor1Forward = new char[]
        {
            'E','K','M','F','L','G','D','Q','V','Z',
            'N','T','O','W','Y','H','X','U','S','P',
            'A','I','B','R','C','J'
        };

        // Rotor1 backward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JGDQOXUSCAMIFRVTPNEWKBLZYH - (Permutation)
        static char[] encryptArrayRotor1Backward = new char[]
        {
            'E','K','M','F','L','G','D','Q','V','Z',
            'N','T','O','W','Y','H','X','U','S','P',
            'A','I','B','R','C','J'
        };

        //// Rotor2 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = NTZPSFBOKMWRCJDIVLAEYUXHGQ - (Permutation)
        //static int[] encryptArrayRotor2 = new int[]
        //{
        //    13, 19, 25, 15, 18, 5, 1, 14, 10, 12,
        //    22, 17, 2, 9, 3, 8, 21, 11, 0, 4,
        //    24, 20, 23, 7, 6, 16
        //};

        // Rotor2 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = AJDKSIRUXBLHWTMCQGZNPYFVOE - (Permutation)
        static char[] encryptArrayRotor2 = new char[]
        {
            'A','J','D','K','S','I','R','U','X','B',
            'L','H','W','T','M','C','Q','G','Z','N',
            'P','Y','F','V','O','E'
        };

        //// Rotor3 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JVIUBHTCDYAKEQZPOSGXNRMWFL - (Permutation)
        //static int[] encryptArrayRotor3 = new int[]
        //{
        //    9, 21, 8, 20, 1, 7, 19, 2, 3, 24,
        //    0, 10, 4, 16, 25, 15, 14, 18, 6, 23,
        //    13, 17, 12, 22, 5, 11
        //};

        // Rotor3 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JVIUBHTCDYAKEQZPOSGXNRMWFL - (Permutation)
        static char[] encryptArrayRotor3 = new char[]
        {
            'B','D','F','H','J','L','C','P','R','T',
            'X','V','Z','N','Y','E','I','W','G','A',
            'K','M','U','S','Q','O'
        };

        //// Reflector configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = EJMZALYXVBWFCRQUONTSPIKHGD - (Permutation transposition)
        //static int[] encryptArrayReflector = new int[]
        //{
        //    4, 9, 12, 25, 0, 11, 24, 23, 21, 1,
        //    22, 5, 2, 17, 16, 20, 14, 13, 19, 18,
        //    15, 8, 10, 7, 6, 3
        //};

        // Reflector configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = YRUHQSLDPXNGOKMIEBFZCWVJAT - (Permutation transposition)
        static char[] encryptArrayReflector = new char[]
        {
            'Y','R','U','H','Q','S','L','D','P','X',
            'N','G','O','K','M','I','E','B','F','Z',
            'C','W','V','J','A','T'
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Enigma Starting!");

            // Calling function to change char array to int Array
            int[] intArrayRotor1 = CharArrayToIntArray(encryptArrayRotor1Forward, "intArrayRotor1");
            int[] intArrayRotor2 = CharArrayToIntArray(encryptArrayRotor2, "intArrayRotor2");
            int[] intArrayRotor3 = CharArrayToIntArray(encryptArrayRotor3, "intArrayRotor3");
            int[] intArrayReflector = CharArrayToIntArray(encryptArrayReflector, "intArrayReflector");

            // Testing for errors in reflector (tranposition)
            bool testResult;
            Console.WriteLine("Testing Enigma configuration for reflector");
            testResult = TesterOfArray(intArrayReflector);
            if (testResult == false)
            {
                Console.ReadKey(false);
                return;
            }
            Console.WriteLine("Transposition in reflector is working");

            // Writing department
            Console.Write("Please write your message to be encrypted:");
            string input = Console.ReadLine();

            input = input.ToLower();

            Console.Write("Please write rotor 1's start position (0-25): ");
            string startPosition1AsString = Console.ReadLine();
            int startPosition1;
            int.TryParse(startPosition1AsString, out startPosition1);

            Console.Write("Please write rotor 2's start position (0-25): ");
            string startPosition2AsString = Console.ReadLine();
            int startPosition2;
            int.TryParse(startPosition2AsString, out startPosition2);

            Console.Write("Please write rotor 3's start position (0-25): ");
            string startPosition3AsString = Console.ReadLine();
            int startPosition3;
            int.TryParse(startPosition3AsString, out startPosition3);

            string outputString = "";
            int currentPosition1 = startPosition1;
            int currentPosition2 = startPosition2;
            int currentPosition3 = startPosition3;

            // testing for other chars than a-z
            foreach (Char c in input)
            {
                if (((int)c < 97) || ((int)c > 122))
                {
                    outputString = outputString + c;
                    continue;
                }

                // Scrambling the letter (pathway through the Enigma)
                char output;
                output = RotorForward(intArrayRotor1, currentPosition2, c);
                //output = RotorForward(intArrayRotor2, currentPosition2, output);
                //output = RotorForward(intArrayRotor3, currentPosition3, output);
                output = Reflector(intArrayReflector, output);
                //output = RotorBackward(intArrayRotor3, currentPosition3, output);
                //output = RotorBackward(intArrayRotor2, currentPosition2, output);
                output = RotorBackward(intArrayRotor1, currentPosition1, output);

                outputString = outputString + output;

                // Turn the rotors
                currentPosition1++;
                currentPosition1 = currentPosition1 % 26;
                if (currentPosition1 == 0)
                {
                    currentPosition2++;
                    currentPosition2 = currentPosition2 % 26;
                    if (currentPosition2 == 0)
                    {
                        currentPosition3++;
                        currentPosition3 = currentPosition3 % 26;
                    }
                }

            }
            Console.WriteLine(outputString);
            Console.ReadKey(false);
        }

        //Testing for errors in rotorconfiguration
        static bool TesterOfArray(int[] encryptArray)
        {
            for (int i = 0; i < 26; i++)
            {
                int number = encryptArray[i];
                int returnNumber = encryptArray[number];
                if (i != returnNumber)
                {
                    Console.WriteLine($"Error in i: {i}, returnumber: {returnNumber}, number: {number}");
                    return false;
                }
            }
            return true;
        }

        // Change array from char to integer
        static int[] CharArrayToIntArray(char[] encryptArray, string name)
        {
            int[] intArray = new int[26];
            int i = 0;

            foreach (char c in encryptArray)
            {
                intArray[i] = (int)c - 65;
                Console.WriteLine($" {name} - {intArray[i]} = {c}");
                i++;
            }
            return intArray;
        }

        // The scrambling rotor forward
        static char RotorForward(int[] encryptArray, int position, char letter)
        {
            int letterAsInteger = (int)letter - 97;
            int rotatedPosition = (letterAsInteger + position) % 26;
            char encryptedChar = (char)(encryptArray[rotatedPosition] + 97);
            Console.WriteLine($"RotorForward: {letter} ({(int)letter}) => {encryptedChar} ({(int)encryptedChar})");
            return encryptedChar;
        }

        // The scrambling reflector
        static char Reflector(int[] encryptArray, char letter)
        {
            int letterAsInteger = (int)letter - 97;
            char encryptedChar = (char)(encryptArray[letterAsInteger] + 97);
            Console.WriteLine($"Reflector: {letter} ({(int)letter}) => {encryptedChar} ({(int)encryptedChar})");
            return encryptedChar;
        }

        // The scrambling rotor backwards  --- Der er en fejl i denne del - måske noget med tælleren i forhold til rotorposition.
        static char RotorBackward(int[] encryptArray, int position, char letter)
        {
            int letterAsInteger = (int)letter - 97;
            int letterPosition = (letterAsInteger) % 26;
            int i = 0;

            foreach (int currentInt in encryptArray)
            {
                if (currentInt == letterPosition)
                {
                    int offset = i - position;
                    if (offset < 0)
                    {
                        offset = 26 + offset;
                    }
                    char encryptedChar = (char)(offset + 97);
                    Console.WriteLine($"Rotor Backward: {letter} ({(int)letter}) => {encryptedChar} ({(int)encryptedChar})");
                    return encryptedChar;
                }
                i++;
            }
            Console.WriteLine("Error in RotorBackward");
            return 'c';
        }
    }
}