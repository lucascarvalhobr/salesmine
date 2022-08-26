﻿using SalesMine.Core.DomainObjects;
using System;

namespace SalesMine.Customers.API.Models
{
    public class Address : Entity
    {
        public string PublicPlace { get; private set; }

        public string Number { get; private set; }

        public string Complement { get; private set; }

        public string Neighborhood { get; private set; }

        public string Cep { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public Guid CustomerId { get; private set; }

        //EF Relation
        public Customer Customer { get; private set; }

        protected Address()
        {

        }

        public Address(string publicPlace, string number, string complement, string neighborhood, string cep, string city, string state)
        {
            PublicPlace = publicPlace;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            Cep = cep;
            City = city;
            State = state;
        }
    }
}
