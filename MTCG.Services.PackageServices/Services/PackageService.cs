﻿using MTCG.Services.PackageServices.Dto;
using MTCG.Services.PackageServices.ViewModels;

namespace MTCG.Services.PackageServices.Services;

public interface PackageService
{

    ValueTask<int> CreatePackageAsync(CardPackageCreationDto cardPackageCreationDto, string createdBy);

    ValueTask<CardPackageViewModel> AcquirePackageAsync(string userName);

}