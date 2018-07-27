namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class PublishReviewCommand : ICommand
    {
        private readonly IReviewService reviewService;
        private readonly ICustomerService customerService;
        private readonly IBusCompanyService busCompanyService;

        public PublishReviewCommand(IReviewService reviewService, ICustomerService customerService, IBusCompanyService busCompanyService)
        {
            this.reviewService = reviewService;
            this.customerService = customerService;
            this.busCompanyService = busCompanyService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(4, arguments);
            int customerId = int.Parse(arguments[0]);
            int companyId = int.Parse(arguments[1]);
            string content = arguments[2];
            int grade = int.Parse(arguments[3]);

            bool isCustomerExist = this.customerService.Exists(customerId);
            if (!isCustomerExist)
            {
                throw new ArgumentException(string.Format(AppConstants.CustomerDoesNotExist, customerId));
            }

            bool isCompanyExist = this.busCompanyService.Exists(companyId);
            if (!isCompanyExist)
            {
                throw new ArgumentException(string.Format(AppConstants.BusCompanyDoesNotExist, companyId));
            }

            if (grade <= 0 || grade > 10)
            {
                throw new ArgumentException(AppConstants.InvalidGrade);
            }

            this.reviewService.AddReview(customerId, companyId, content, grade);
            BusCompany busCompany = this.busCompanyService.ById(companyId);
            Customer customer = this.customerService.ById(customerId);
            return string.Format(AppConstants.AddReview, customer.FirstName, customer.LastName, busCompany.Name);
        }
    }
}