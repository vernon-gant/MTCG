using MTCG.Domain;

namespace MTCG.Persistance.Repositories.Packages;

public interface PackageRepository
{

    ValueTask<Package> Create(Package package);

    ValueTask<Package?> GetFirstNotAcquiredPackage();

    ValueTask<Package> AddPackageToUser(int userId, Package package, int packagePrice);

}