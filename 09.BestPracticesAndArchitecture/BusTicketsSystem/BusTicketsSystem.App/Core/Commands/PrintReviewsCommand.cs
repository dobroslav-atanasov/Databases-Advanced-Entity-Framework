namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using System.Text;
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class PrintReviewsCommand : ICommand
    {
        private readonly IBusCompanyService busCompanyService;
        private readonly IReviewService reviewService;

        public PrintReviewsCommand(IBusCompanyService busCompanyService, IReviewService reviewService)
        {
            this.busCompanyService = busCompanyService;
            this.reviewService = reviewService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            int companyId = int.Parse(arguments[0]);

            bool isCompanyExist = this.busCompanyService.Exists(companyId);
            if (!isCompanyExist)
            {
                throw new ArgumentException(string.Format(AppConstants.BusCompanyDoesNotExist, companyId));
            }

            BusCompany busCompany = this.busCompanyService.ById(companyId);
            Review[] reviews = this.reviewService.PrintReviews(companyId);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Bus company: {busCompany.Name}");
            foreach (Review review in reviews)
            {
                sb.AppendLine($"{review.Customer.FirstName} {review.Customer.LastName}");
                sb.AppendLine($"{review.Grade} {review.Published}");
                sb.AppendLine($"{review.Content}");
            }

            return sb.ToString().Trim();
        }
    }
}