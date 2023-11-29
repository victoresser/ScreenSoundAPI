using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using FluentValidation.Internal;

namespace ScreenSound.Dominio.Models;

public abstract class Entity<TEntity> : AbstractValidator<TEntity> where TEntity : Entity<TEntity>
{
    public int Id { get; protected set; }
    public string Nome { get; protected set; }
    [NotMapped] public FluentValidation.Results.ValidationResult ValidationResult { get; protected set; }

    protected Entity()
    {
        Id = default;
        ValidationResult = new FluentValidation.Results.ValidationResult();
    }

    public abstract bool Validar();

    public static bool operator ==(Entity<TEntity> a, Entity<TEntity> b)
    {
        if ((object)a == null && (object)b == null)
        {
            return true;
        }

        if ((object)a == null || (object)b == null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator  !=(Entity<TEntity> a, Entity<TEntity> b)
    {
        return !(a == b);
    }

    public new FluentValidation.Results.ValidationResult Validate(TEntity instance)
    {
        return Validate(new ValidationContext<TEntity>
            (instance, new PropertyChain(),
            ValidatorOptions.Global.ValidatorSelectors.DefaultValidatorSelectorFactory()));
    }
}
