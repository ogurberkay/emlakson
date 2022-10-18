using DataAccess.Repositories.Abstract;

namespace Business.Service.Concrete;

public class BaseService
{
    protected IUnitOfWork UnitOfWork { get; }
    public BaseService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}