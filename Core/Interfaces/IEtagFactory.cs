using Core.Entities;

namespace Core.Interfaces;

public interface IEtagFactory
{
    string GetEtag(Move move);
}