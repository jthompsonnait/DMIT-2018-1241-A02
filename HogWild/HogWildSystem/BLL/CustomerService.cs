﻿#nullable disable
using HogWildSystem.DAL;
using HogWildSystem.Entities;
using HogWildSystem.ViewModels;

namespace HogWildSystem.BLL
{
    public class CustomerService
    {

        #region Fields

        private readonly HogWildContext _hogWildContext;

        #endregion

        //  Constructor for the WorkingVersionsService class.
        internal CustomerService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWoldContext instance.
            _hogWildContext = hogWildContext;
        }

        public List<CustomerSearchView> GetCustomers(string lastName, string phone)
        {
            // Business Rules
            // These are processing rules that need to be satisfied
            // for valid data

            // Rule: Both last name and phone number cannot be empty
            // Rule: RemoveFromViewFlag must be false
            if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentNullException("Please provide either a last name and/or phone number");
            }

            // Need to update parameters so we are not searching on an empty value.
            // Otherwise, an empty string will return all records
            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                phone = Guid.NewGuid().ToString();
            }

            return _hogWildContext.Customers
                        .Where(x => (x.LastName.Contains(lastName.Trim())
                                     || x.Phone.Contains(phone.Trim()))
                                    && !x.RemoveFromViewFlag)
                        .Select(x => new CustomerSearchView
                        {
                            CustomerID = x.CustomerID,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            City = x.City,
                            Phone = x.Phone,
                            Email = x.Email,
                            StatusID = x.StatusID,
                            TotalSales = x.Invoices.Sum(x => x.SubTotal + x.Tax)
                        })
                        .OrderBy(x => x.LastName)
                        .ToList();
        }

        public CustomerEditView GetCustomer(int customerID)
        {
            //  Business Rules
            //	These are processing rules that need to be satisfied
            //		for valid data
            //		rule:	customerID must be valid 

            if (customerID == 0)
            {
                throw new ArgumentNullException("Please provide a customer");
            }

            return _hogWildContext.Customers
                .Where(x => (x.CustomerID == customerID
                             && x.RemoveFromViewFlag == false))
                .Select(x => new CustomerEditView
                {
                    CustomerID = x.CustomerID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    ProvStateID = x.ProvStateID,
                    CountryID = x.CountryID,
                    PostalCode = x.PostalCode,
                    Phone = x.Phone,
                    Email = x.Email,
                    StatusID = x.StatusID,
                    RemoveFromViewFlag = x.RemoveFromViewFlag
                }).FirstOrDefault();
        }
    }
}
