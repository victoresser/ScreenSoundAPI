﻿using ScreenSound.Dominio.Interfaces;

namespace ScreenSound.Dados;

public class UnitOfWork : IUnitOfWork
{
    private readonly ScreenSoundContext _context;

    public UnitOfWork(ScreenSoundContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}
