using MarketPlace.Shared;

namespace MarketPlace.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ReviewServices(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
    }
}
