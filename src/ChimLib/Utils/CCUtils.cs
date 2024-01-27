using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChimLib.Utils
{
    public static class CCUtils
    {

        public static string CreditCardType(string CardNo)
        {
            string functionReturnValue = null;

            //*CARD TYPES            *PREFIX           *WIDTH
            //American Express       34, 37            15
            //Diners Club            300 to 305, 36    14
            //Carte Blanche          38                14
            //Discover               6011              16
            //EnRoute                2014, 2149        15
            //JCB                    3                 16
            //JCB                    2131, 1800        15
            //Master Card            51 to 55          16
            //Visa                   4                 13, 16

            //Just in case nothing is found
            functionReturnValue = "Unknown";

            //Remove all spaces and dashes from the passed string
            CardNo = CardNo.Replace(" ", "").Replace( "-", "");

            //Check that the minimum length of the string isn't less
            //than fourteen characters and -is- numeric
            if (CardNo.Length < 14 | !Regex.IsMatch(CardNo, @"^\d+$"))
                return functionReturnValue;

            //Check the first two digits first
            switch (Convert.ToInt32(CardNo.Substring(0, 2)))
            {
                case 34:
                case 37:
                    functionReturnValue = "American Express";
                    break;
                case 36:
                    functionReturnValue = "Diners Club";
                    break;
                case 38:
                    functionReturnValue = "Carte Blanche";
                    break;
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                    functionReturnValue = "Master Card";
                    break;
                default:

                    //None of the above - so check the
                    //first four digits collectively
                    switch (Convert.ToInt32(CardNo.Substring(0, 4)))
                    {

                        case 2014:
                        case 2149:
                            functionReturnValue = "EnRoute";
                            break;
                        case 2131:
                        case 1800:
                            functionReturnValue = "JCB";
                            break;
                        case 6011:
                            functionReturnValue = "Discover";
                            break;
                        default:

                            //None of the above - so check the
                            //first three digits collectively
                            switch (Convert.ToInt32(CardNo.Substring(0, 3)))
                            {
                                case 300:
                                case 301:
                                case 302:
                                case 303:
                                case 304:
                                case 305:
                                    functionReturnValue = "American Diners Club";
                                    break;
                                default:

                                    //None of the above -
                                    //so simply check the first digit
                                    switch (Convert.ToInt32(CardNo.Substring(0, 1)))
                                    {
                                        case 3:
                                            functionReturnValue = "JCB";
                                            break;
                                        case 4:
                                            functionReturnValue = "Visa";
                                            break;
                                    }

                                    break;
                            }
                            break;

                    }
                    break;

            }
            return functionReturnValue;
        }
    }
}
