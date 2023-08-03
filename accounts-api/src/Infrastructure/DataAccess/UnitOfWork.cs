// <copyright file="UnitOfWork.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

using System;
using System.Threading.Tasks;
using Application.Services;

public sealed class UnitOfWork(DbContext context)
    : IUnitOfWork, IDisposable
{
    private bool _disposed;

    /// <inheritdoc />
    public void Dispose() => Dispose(true);

    /// <inheritdoc />
    public async Task<int> Save()
    {
        int affectedRows = await context
            .SaveChangesAsync()
            .ConfigureAwait(false);
        return affectedRows;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            context.Dispose();
        }

        _disposed = true;
    }
}
