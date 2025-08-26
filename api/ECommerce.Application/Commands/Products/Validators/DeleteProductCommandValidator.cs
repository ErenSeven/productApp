using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace ECommerce.Application.Commands.Products.Validators
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Ürün ID'si boş olamaz.")
                .Must(id => id != Guid.Empty).WithMessage("Geçersiz ürün ID'si.");
        }
    }
}