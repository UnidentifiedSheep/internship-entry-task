using Core.Abstract;

namespace Infrastructure.Services;

public class SystemRandomizer : RandomizerBase
{
    protected sealed override double NextDouble() => Random.Shared.NextDouble();
    
}