using System;
using System.Windows.Forms;

namespace EnigmaEncrypt
{
    class Program_Test
    {
        // Rotor1 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JGDQOXUSCAMIFRVTPNEWKBLZYH - (Permutation)
        static public int[] encryptArrayRotor1Forward = new int[]
        {
            9, 5, 1, 13, 10, 18, 14, 11, 20, 17,
            2, 23, 19, 4, 7, 4, 25, 22, 12, 3,
            16, 6, 15, 2, 0, 8
        };

        // Rotor1 Backward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JVICSMBZLAUWKREQDNHPGOTFYX - (Permutation) NOT the same as the forward!
        static public int[] encryptArrayRotor1Backward = new int[]
        {
            9, 20, 6, 25, 14, 7, 21, 18, 3, 17,
            10, 11, 24, 4, 16, 1, 13, 22, 15, 22,
            12, 19, 23, 8, 0, 24
        };

        // Rotor2 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = AJDKSIRUXBLHWTMCQGZNPYFVOE - (Permutation)
        static public int[] encryptArrayRotor2Forward = new int[]
        {
            0, 8, 1, 7, 14, 3, 11, 13, 15, 18,
            1, 22, 10, 6, 24, 13, 0, 15, 7, 20,
            21, 3, 9, 24, 16, 5
        };

        // Rotor2 Backward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = AJPCZWRLFBDKOTYUQGENHXMIVS - (Permutation) NOT the same as the forward!
        static public int[] encryptArrayRotor2Backward = new int[]
        {
            0, 8, 13, 25, 21, 17, 11, 4, 23, 18,
            19, 25, 2, 6, 10, 5, 0, 15, 12, 20,
            13, 2, 16, 11, 23, 19
        };

        // Rotor3 Forward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = JVIUBHTCDYAKEQZPOSGXNRMWFL - (Permutation)
        static public int[] encryptArrayRotor3Forward = new int[]
        {
            1, 2, 3, 4, 5, 6, 22, 8, 9, 10,
            13, 10, 13, 0, 10, 15, 18, 5, 14, 7,
            16, 17, 24, 21, 18, 15
        };

        //CHECK ALLE ARRAYS FOR KORREKT POSITIONER !!!
        // Rotor3 Backward configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = TAGBPCSDQEUFVNZHYIXJWLRKOM - (Permutation) NOT the same as the forward!
        static public int[] encryptArrayRotor3Backward = new int[]
        {
            19, 25, 4, 24, 11, 23, 12, 22, 8, 21,
            10, 20, 9, 0, 11, 18, 8, 17, 5, 16,
            2, 16, 21, 13, 16, 13
        };

        // Reflector configuration - ABCDEFGHIJKLMNOPQRSTUVWXYZ = YRUHQSLDPXNGOKMIEBFZCWVJAT - (Transposition)
        static int[] encryptArrayReflector = new int[]
        {
            24, 16, 18, 4, 12, 13, 5, 22, 7, 14,
            3, 21, 2, 23, 24, 19, 14, 10, 13, 6,
            8, 1, 25, 12, 2, 20
        };

        // Rotor scrambling forward
        static public char ForwardEncryption(int[] encryptArrayRotor, char letter, int position)
        {
            // Step 1: Input char to int
            int letterAsInteger = (int)letter - 65;

            // Step 2: Entry from keyboard to position on rotor disc
            int entryAtRotor = (letterAsInteger + position) % 26;

            // Step 3: Output from rotor
            int encryptedInt = (letterAsInteger + encryptArrayRotor[entryAtRotor]) % 26;

            // Step 4: Adding 65 to int
            encryptedInt = encryptedInt + 65;

            // Step 5: Checking that the number (the int of the letter) is between 65-90
            if (encryptedInt < 65)
            {
                encryptedInt = (char)(encryptedInt + 26);
            }

            // Step 6: Converting the int of the letter to a char
            char encryptedChar = (char)encryptedInt;

            Console.WriteLine($"RotorForward:  {letter} ({(int)(letter - 65)}) => {(char)encryptedInt} ({encryptedInt - 65})");
            return encryptedChar;
        }

        // Rotor scrambling backward
        static public char BackwardEncryption(int[] encryptArrayRotor, char letter, int position)
        {
            // Step 1: Input char to int
            int letterAsInteger = (int)letter - 65;

            // Step 2: Entry from keyboard to position on rotor disc
            int entryAtRotor = (letterAsInteger + position) % 26;

            // Step 3: Output from rotor
            int encryptedInt = (letterAsInteger + encryptArrayRotor[entryAtRotor]) % 26;

            // Step 4: Adding 65 to int
            encryptedInt = encryptedInt + 65;

            // Step 5: Checking that the number (the int of the letter) is between 65-90
            if (encryptedInt < 65)
            {
                encryptedInt = (char)(encryptedInt + 26);
            }

            // Step 6: Converting the int of the letter to a char
            char encryptedChar = (char)encryptedInt;

            Console.WriteLine($"RotorBackward: {letter} ({(int)(letter - 65)}) => {encryptedChar} ({(int)(encryptedInt - 65)})");
            return encryptedChar;
        }

        // The scrambling reflector
        static public char Reflector(int[] encryptArrayRotor, char letter)
        {
            // Step 1: Input char to int
            int letterAsInteger = (int)letter - 65;

            // Step 2: Output from reflector
            int encryptedInt = (letterAsInteger + encryptArrayRotor[letterAsInteger]) % 26;

            // Step 3: Adding 65 to int
            encryptedInt = encryptedInt + 65;

            // Step 4: Converting the int of the letter to a char
            char encryptedChar = (char)encryptedInt;

            Console.WriteLine($"Reflector:     {letter} ({(int)(letter - 65)}) => {(char)encryptedInt} ({encryptedInt - 65})");
            return encryptedChar;
        }

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Enigma Starting!");

            // Section for collecting text input
            Console.Write("Please write your message to be encrypted: ");
            string input = Console.ReadLine();

            input = input.ToUpper();

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
            foreach (Char letter in input)
            {
                if (((int)letter < 65) || ((int)letter > 90))
                {
                    outputString = outputString + letter;
                    continue;
                }

                // Scrambling the letter (pathway through the Enigma)
                char output;

                output = ForwardEncryption(encryptArrayRotor1Forward, letter, currentPosition1);
                output = ForwardEncryption(encryptArrayRotor2Forward, output, currentPosition2);
                output = ForwardEncryption(encryptArrayRotor3Forward, output, currentPosition3);
                output = Reflector(encryptArrayReflector, output);
                output = BackwardEncryption(encryptArrayRotor3Backward, output, currentPosition3);
                output = BackwardEncryption(encryptArrayRotor2Backward, output, currentPosition2);
                output = BackwardEncryption(encryptArrayRotor1Backward, output, currentPosition1);

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

            Console.WriteLine("");
            Console.WriteLine("Your message has been copied to your clipboard");
            Clipboard.SetText(outputString);
            Console.WriteLine("Your message is as follows:");
            Console.WriteLine("");
            Console.WriteLine(outputString);
            Console.ReadKey(false);
        }
    }
}