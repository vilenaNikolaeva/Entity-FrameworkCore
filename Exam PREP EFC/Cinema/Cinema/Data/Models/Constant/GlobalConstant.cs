using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Data.Models.Constant
{
    public class GlobalConstants
    {
        // Movie 
        public const int MinLenghtTitle = 3;
        public const int MaxLenghtTitle = 20;
        public const int MinLenghtDirector = 3;
        public const int MaxLenghtDirector = 20;
        public const int MinLRating = 1;
        public const int MaxLRating = 10;

        //Hall 
        public const int MinLenghtName = 3;
        public const int MaxLenghtName = 20;
        public const int MinSeatsCount = 0;

        //Customer
        public const int MinLenghtFirstName = 3;
        public const int MaxLenghtFistName = 20;
        public const int MinLenghtLasttName = 3;
        public const int MaxLenghtLastName = 20;
        public const int MinAge = 12;
        public const int MaxAge = 110;
        public const double MinBalanceValue = 0.01;

        //Ticket
        public const double MinPrice = 0.01;
    }
}
