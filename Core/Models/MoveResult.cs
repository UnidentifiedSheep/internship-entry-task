using Core.Entities;

namespace Core.Models;

public readonly record struct MoveResult(Move? Value, bool IsWonMove, bool IsGameEnd);
