using MatchResult.Models;

namespace MatchResult.Services
{
    public interface IMatchResultService
    {
        string QueryMatchResult(int matchId);
        string UpdateMatchResult(int matchId, Event matchEvent);
    }
} 