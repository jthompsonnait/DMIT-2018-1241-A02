using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using static MudBlazor.Icons;

namespace HogWildWebApp.Components.Pages.SamplePages
{

    public partial class CustomerEdit
    {
        #region Fields

        // The Customer
        private CustomerEditView customer = new();
        //  The provinces
        private List<LookupView> provinces = new();
        //  The countries
        private List<LookupView> countries = new();
        //  The status lookup
        private List<LookupView> statusLookup = new();

        #endregion

        #region Feedback & Error Messages

        // The feedback message
        private string feedbackMessage;

        // The error message
        private string errorMessage;

        // has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        // has error
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);

        // error details
        private List<string> errorDetails = new();

        #endregion

        #region Properties

        //  The customer service
        [Inject] protected CustomerService CustomerService { get; set; }

        //  The category lookup service
        [Inject] protected CategoryLookupService CategoryLookupService { get; set; }
        //  Customer ID used to create or edit a customer
        [Parameter] public int CustomerID { get; set; } = 0;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                // check to see if we are navigating using a valid customer CustomerID
                //      or arw we going to create a new customer
                if (CustomerID > 0)
                {
                    customer = CustomerService.GetCustomer(CustomerID);
                }

                // lookups
                provinces = CategoryLookupService.GetLookups("Province");
                countries = CategoryLookupService.GetLookups("Country");
                statusLookup = CategoryLookupService.GetLookups("Customer Status");

                await InvokeAsync(StateHasChanged);
            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }

                errorMessage = $"{errorMessage}Unable to search for customer";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }
    }
}

