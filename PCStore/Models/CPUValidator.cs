using FluentValidation;

namespace PCStore.Models
{
    public class CPUValidator : AbstractValidator<CPU>
    {
        public CPUValidator()
        {
            RuleFor(cpu => cpu.Name).NotEmpty();
            RuleFor(cpu => cpu.Model).NotEmpty();
            RuleFor(cpu => cpu.Price).GreaterThan(0);
            RuleFor(cpu => cpu.Cores).GreaterThan(0);
            RuleFor(cpu => cpu.ClockSpeed).GreaterThan(0);
        }
    }
}
