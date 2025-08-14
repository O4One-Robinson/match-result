using MatchResult.Models;
using MatchResult.Repositories;

namespace MatchResult.Services
{
    public class MatchResultService : IMatchResultService
    {
        private readonly IMatchResultRepository _repository;

        public MatchResultService(IMatchResultRepository repository)
        {
            _repository = repository;
        }

        public string QueryMatchResult(int matchId)
        {
            var rawData = _repository.GetRawData(matchId);
            
            return rawData switch
            {
                "HA" => "1:1 (First Half)",
                "HA;H" => "2:1 (Second Half)",
                _ => "Unknown Result"
            };
        }

        public string UpdateMatchResult(int matchId, Event matchEvent)
        {
            var rawData = _repository.GetRawData(matchId);
            string updatedResult;

            switch (matchEvent)
            {
                case Event.HomeGoal:
                    updatedResult = rawData + "H";
                    break;
                case Event.AwayGoal:
                    updatedResult = rawData + "A";
                    break;
                case Event.Period:
                    updatedResult = rawData + ";";
                    break;
                case Event.HomeCancel:
                    if (rawData.EndsWith("H"))
                    {
                        updatedResult = rawData.Substring(0, rawData.Length - 1);
                    }
                    else
                    {
                        throw new UpdateMatchResultException($"Invalid operation: {matchEvent} on {rawData}");
                    }
                    break;
                case Event.AwayCancel:
                    if (rawData.EndsWith("A") || rawData.EndsWith("A;"))
                    {
                        updatedResult = rawData.Remove(rawData.LastIndexOf("A"), 1);
                    }
                    else
                    {
                        throw new UpdateMatchResultException($"Invalid operation: {matchEvent} on {rawData}");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matchEvent), matchEvent, null);
            }

            return updatedResult;
        }
    }

    public class UpdateMatchResultException : Exception
    {
        public UpdateMatchResultException(string message) : base(message) { }
    }
}