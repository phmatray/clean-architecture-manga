// <copyright file="UnitOfWork.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess;

using System;
using System.Threading.Tasks;
using Application.Services;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly MangaContext _context;
    private bool _disposed;

    public UnitOfWork(MangaContext context) => _context = context;

    /// <inheritdoc />
    public void Dispose() => Dispose(true);

    /// <inheritdoc />
    public async Task<int> Save()
    {
        int affectedRows = await _context
            .SaveChangesAsync()
            .ConfigureAwait(false);
        return affectedRows;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}
