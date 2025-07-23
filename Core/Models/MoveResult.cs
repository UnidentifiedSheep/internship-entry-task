using Core.Dtos;
using Core.Entities;

namespace Core.Models;

public readonly record struct MoveResult(MoveDto? Value, bool IsWonMove, bool IsGameEnd);
