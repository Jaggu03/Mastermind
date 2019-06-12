using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Mastermind
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool result = false;

        static void Main(string[] args)
        {
            int attempts = 0;
            const int maximumAttempts = 10;
            string valueEntered;
            int valueConverted;

            NotifyUser($"Let's start the Mastermind Game. \nGuess the correct 4 digit number to win the contest.\nEach individual digit must be between 0 and 7", "Info");

            while (maximumAttempts > attempts)
            {
                NotifyUser($"Number of attempts left to make a correct guess are {10 - attempts}", "Debug");
                NotifyUser("Please enter a combination (example: 2341) and press enter: ", "Debug");

                valueEntered = Console.ReadLine();

                if (valueEntered.Length != 4)
                {
                    NotifyUser("The combination must be exactly 4 digits.", "Warn");
                }
                else if(valueEntered.Contains("0") || valueEntered.Contains("7") || valueEntered.Contains("8") || valueEntered.Contains("9"))
                {
                    NotifyUser("Digits in the combination must be between 0 and 7.", "Warn");
                }
                else
                {
                    try
                    {
                        bool cast = int.TryParse(valueEntered, out valueConverted);
                        if (cast)
                        {
                            bool status = GetResponse(valueEntered);
                            if(status)
                            {
                                NotifyUser("Congratulations, You won the contest!!!", "Info");
                                break;
                            }
                            else
                            {
                                attempts++;
                                if(attempts >=  maximumAttempts)
                                {
                                    NotifyUser("Sorry, You lost the contest!!!", "Error");
                                    break;
                                }
                                else
                                {
                                    NotifyUser("Sorry, Wrong guess!!!", "Error");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            throw new FormatException("The combination should only contain numbers.");
                        }
                    }
                    catch (FormatException Ex)
                    {
                        NotifyUser(Ex.Message, "Warn");
                    }

                }
            }

            Console.ReadLine();
        }

        public static bool GetResponse(string value)
        {
            Random generateRandomValue = new Random();

            int position = -1;

            int firstDigit = generateRandomValue.Next(1, 6);
            int secondDigit = generateRandomValue.Next(1, 6);
            int thirdDigit = generateRandomValue.Next(1, 6);
            int fourthDigit = generateRandomValue.Next(1, 6);

            StringBuilder sb = new StringBuilder();
            sb.Append(Convert.ToString(firstDigit));
            sb.Append(Convert.ToString(secondDigit));
            sb.Append(Convert.ToString(thirdDigit));
            sb.Append(Convert.ToString(fourthDigit));

            string buildAnswer = Convert.ToString(sb);


            StringBuilder buildOutput = new StringBuilder();

            while(position !=3)
            {
                position++;
                if(buildAnswer[position] == value[position])
                {
                    buildOutput.Append("+");
                }
                else if (buildAnswer.Contains(value[position]))
                {
                    buildOutput.Append("-");
                }
                else
                {
                    buildOutput.Append("_");
                }
            }

            NotifyUser($"(+) Sign indicates - Digit matched both in position and value\n(-) Sign indicates - Digit exist in generated value but mispositioned\n(_) Sign indicates - Digit mismatched both in position and value\n{buildOutput}", "Warn");

            if (buildAnswer == value)
            {
                result = true;
            }

            return result;
        }

        public static void NotifyUser(string message, string level)
        {
            switch(level)
            {
                case "Info":
                    log.Info(message);
                    break;
                case "Debug":
                    log.Debug(message);
                    break;
                case "Warn":
                    log.Warn(message);
                    break;
                case "Error":
                    log.Error(message);
                    break;
                default:
                    break;
            }
        }
    }
}
