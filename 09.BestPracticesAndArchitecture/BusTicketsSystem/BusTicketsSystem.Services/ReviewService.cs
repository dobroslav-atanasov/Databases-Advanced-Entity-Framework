namespace BusTicketsSystem.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;

    public class ReviewService : IReviewService
    {
        private readonly BusTicketsSystemContext dbContext;

        public ReviewService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Review[] PrintReviews(int companyId)
        {
            Review[] reviews = this.dbContext
                .Reviews
                .Where(r => r.BusCompany.Id == companyId)
                .ToArray();

            return reviews;
        }

        public void AddReview(int customerId, int companyId, string content, int grade)
        {
            Review review = new Review()
            {
                CustomerId = customerId,
                BusCompanyId = companyId,
                Content = content,
                Grade = grade,
                Published = DateTime.Now
            };

            this.dbContext.Reviews.Add(review);

            this.dbContext.SaveChanges();
        }
    }
}