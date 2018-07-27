namespace BusTicketsSystem.Services.Contracts
{
    using Models;

    public interface IReviewService
    {
        Review[] PrintReviews(int companyId);

        void AddReview(int customerId, int companyId, string content, int grade);
    }
}