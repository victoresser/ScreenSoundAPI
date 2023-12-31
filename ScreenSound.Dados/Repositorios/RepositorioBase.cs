﻿using Microsoft.EntityFrameworkCore;
using ScreenSound.Dominio;
using ScreenSound.Dominio.Models;
using ScreenSound.Dominio.Services;

namespace ScreenSound.Dados.Repositorios;

public class RepositorioBase<TEntity> : IRepositorio<TEntity> where TEntity : Entity<TEntity>
{
    protected readonly ScreenSoundContext Context;

    public RepositorioBase(ScreenSoundContext context)
    {
        Context = context;
    }

    public async Task Adicionar(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<List<TEntity>> Consultar()
    {
        var query = Context.Set<TEntity>().ToListAsync();

        return await query;
    }

    public async Task Deletar(int id)
    {
        var query = Context.Set<TEntity>().Where(t => t.Id == id);
        Context.Set<TEntity>().RemoveRange(query);
        await Context.SaveChangesAsync();
    }
}
