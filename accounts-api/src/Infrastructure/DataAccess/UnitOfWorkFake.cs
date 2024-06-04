// <copyright file="UnitOfWorkFake.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess;

using System.Threading.Tasks;
using Application.Services;

/// <summary>
///     Unit of Work Fake.
/// </summary>
public sealed class UnitOfWorkFake : IUnitOfWork
{
    /// <summary>
    ///     Saves changes.
    /// </summary>
    /// <returns>Number of affected rows.</returns>
    public async Task<int> Save()
        => await Task.FromResult(0).ConfigureAwait(false);
}
