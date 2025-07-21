using Core.Entities;
using Core.Helpers;
using Core.Interfaces;

namespace Application.Factory;

public class ETagFactory : IEtagFactory
{
    public string GetEtag(Move move)
    {
        var hash = HashCodeGenerator.GenerateHash(move.GameId, move.X, move.Y);
        return hash;
    }
}