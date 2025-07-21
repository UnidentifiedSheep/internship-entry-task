namespace Core.Abstract;

public abstract class RandomizerBase
{
    /// <summary>
    /// Генератор случайного числа от 0.0 до 1.0
    /// </summary>
    /// <returns>Число от 0.0 до 1.0</returns>
    protected abstract double NextDouble();
    
    /// <summary>
    /// Проверяет успешность события с заданной вероятностью.
    /// </summary>
    /// <param name="probability">Вероятность успеха от 0.0 (0%) до 1.0 (100%).</param>
    /// <returns>True, если событие произошло, иначе false.</returns>
    public virtual bool TryChance(double probability)
    {
        if (probability is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(probability), "Вероятность должна быть в диапазоне от 0.0 до 1.0");
        return NextDouble() < probability;
    }
}