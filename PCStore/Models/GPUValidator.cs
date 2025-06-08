using FluentValidation;

namespace PCStore.Models
{
    public class GPUValidator : AbstractValidator<GPU>
    {
        public GPUValidator()
        {
            RuleFor(gpu => gpu.Name).NotEmpty();
            RuleFor(cpu => cpu.Model).NotEmpty();
            RuleFor(gpu => gpu.Price).GreaterThan(0);
            RuleFor(gpu => gpu.Memory).GreaterThan(0);
        }
    }
}
