using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.Products.Commands;
using FluentValidation;

namespace Alpha.Application
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Barcode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ImageBytes).NotEmpty();
        }
    }
}
