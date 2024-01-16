using MTCG.Domain;

namespace MTCG.Persistance.Repositories.Packages;

public interface PackageRepository
{

    ValueTask<CardPackage> Create(CardPackage cardPackage);

    ValueTask<CardPackage> Acquire(string username);

}