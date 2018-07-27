namespace BusTicketsSystem.App.Core.Utilities
{
    internal static class AppConstants
    {
        internal const string Suffix = "Command";

        internal const string InvalidCommandName = "Invalid {0} command name!";

        internal const string InvalidNumberOfArguments = "Invalid number of arguments!";

        internal const string BusCompanyExists = "Bus company {0} already exists!";

        internal const string AddBusCompany = "Bus company {0} was added successfully!";

        internal const string TownExists = "Town {0} already exists!";

        internal const string AddTown = "Town {0} was added successfully!";

        internal const string BusStationExists = "Bus station {0} already exists!";

        internal const string AddBusStation = "Bus station {0} was added successfully!";

        internal const string TownDoesNotExist = "Town {0} does not exist!";

        internal const string NotValidGender = "Invalid gender!";

        internal const string AddCustomer = "Customer {0} {1} was added successfully!";

        internal const string InvalidTowns = "Invalid origin or destination bus stations!";

        internal const string AddTrip = "Trip was added successfully!";

        internal const string BusCompanyDoesNotExist = "Bus company {0} does not exist!";

        internal const string NotValidStatus = "Invalid status!";

        internal const string DateTime = "dd/MM/yyyy HH:mm";

        internal const string InvalidDateTimeFormat = "Format of date time have to dd/MM/yyyy HH:mm";

        internal const string BusStationDoesNotExist = "Bus station does not exist!";

        internal const string CustomerDoesNotExist = "Customer with id {0} does not exist!";

        internal const string InvalidBalance = "Balance cannot be zero or negative!";

        internal const string DepositMoney = "Successfully deposit money!";

        internal const string AddBankAccount = "Bank account was added successfully!";

        internal const string BankAccountDoesNotExist = "Bank account does not exist!";

        internal const string TripDoesNotExist = "Trip with id {0} does not exist!";

        internal const string AddTicket = "Customer {0} {1} bought ticket for trip {2} for {3} on seat {4}!";

        internal const string InvalidSeat = "Invalid seat!";

        internal const string AddReview = "Customer {0} {1} published review for company {2}!";

        internal const string InvalidGrade = "Invalid grade!";

        internal const string ChangeTripStatus ="Trip from {0} to {1} on {2} Status changed from {3} to {4}!";
    }
}