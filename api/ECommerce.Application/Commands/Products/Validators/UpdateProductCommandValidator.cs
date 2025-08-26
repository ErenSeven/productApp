using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace ECommerce.Application.Commands.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().When(x => x.Name != null).WithMessage("Ürün adı boş olamaz.")
                .MinimumLength(3).When(x => x.Name != null).WithMessage("Ürün adı en az 3 karakter olmalıdır.")
                .MaximumLength(100).When(x => x.Name != null).WithMessage("Ürün adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().When(x => x.Description != null).WithMessage("Açıklama boş olamaz.")
                .MinimumLength(10).When(x => x.Description != null).WithMessage("Açıklama en az 10 karakter olmalıdır.")
                .MaximumLength(1000).When(x => x.Description != null).WithMessage("Açıklama en fazla 1000 karakter olabilir.");

            RuleFor(x => x.Price)
                .GreaterThan(0).When(x => x.Price != null).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.Category)
                .NotEmpty().When(x => x.Category != null).WithMessage("Kategori boş olamaz.")
                .MaximumLength(50).When(x => x.Category != null).WithMessage("Kategori en fazla 50 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().When(x => x.ImageUrl != null).WithMessage("Resim URL'si boş olamaz.")
                .MaximumLength(200).When(x => x.ImageUrl != null).WithMessage("Resim URL'si en fazla 200 karakter olabilir.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).When(x => x.Stock != null).WithMessage("Stok miktarı negatif olamaz.");
        }
    }
}