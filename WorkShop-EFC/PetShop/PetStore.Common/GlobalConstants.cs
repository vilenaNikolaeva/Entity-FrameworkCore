using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Common
{
    public static  class GlobalConstants
    {
        //Breed
        public const int BreedNameMinLenght = 5;
        public const int BreedNameMaxLenght = 30;

        //Client
        public const int ClientUserNameMinLenght = 3;
        public const int ClientUserNameMaxLenght = 30;

        public const int ClientEmailMinLenght = 6;
        public const int ClientEmailMaxLenght = 60;

        public const int ClientFirtsNameMinLenght = 3;
        public const int ClientFirtsNameMaxLenght = 30;

        public const int ClientLastNameMinLenght = 3;
        public const int ClientLastNameMaxLenght = 30;

        //ClientProduct
        public const int ClientProductMinQuantity = 1;

        //Order
        public const int OrderTownNameMinLenght = 3;
        public const int OrderTownNameMaxLenght = 50;

        public const int OrderAddressMinLenght = 6;
        public const int OrderAddressMaxLenght = 60;

        public const int OrderPetMinPrice = 1;

        //Pet
        public const int PetNameMinLenght = 3;
        public const int PetNameMaxLenght = 50;

        //Product
        public const int ProductNameMinLenght = 3;
        public const int ProductNameMaxLenght = 30;
    }
}
