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
            return CalculateMatchResult(rawData);
        }

        public string UpdateMatchResult(int matchId, Event matchEvent)
        {
            var rawData = _repository.GetRawData(matchId);
            string updatedResult = rawData;

            switch (matchEvent)
            {
                case Event.HomeGoal:
                    updatedResult = AppendEvent(updatedResult, 'H');
                    break;
                case Event.AwayGoal:
                    updatedResult = AppendEvent(updatedResult, 'A');
                    break;
                case Event.Period:
                    updatedResult = AppendEvent(updatedResult, ';');
                    break;
                case Event.HomeCancel:
                    updatedResult = RemoveLastEvent(updatedResult, 'H');
                    break;
                case Event.AwayCancel:
                    updatedResult = RemoveLastEvent(updatedResult, 'A');
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matchEvent), matchEvent, null);
            }

            return CalculateMatchResult(updatedResult);
        }

        private string AppendEvent(string input, char eventChar)
        {
            return input + eventChar;
        }

        private string RemoveLastEvent(string input, char eventChar)
        {
            int index = input.LastIndexOf(eventChar);
            if (index >= 0)
            {
                return input.Remove(index, 1);
            }
            throw new UpdateMatchResultException($"Invalid operation: Cannot remove {eventChar} from {input}");
        }

        private string CalculateMatchResult(string rawData)
        {
            int homeGoals = rawData.Count(c => c == 'H');
            int awayGoals = rawData.Count(c => c == 'A');
            bool isSecondHalf = rawData.Contains(';');

            string half = isSecondHalf ? "Second Half" : "First Half";
            return $"{homeGoals}:{awayGoals} ({half})";
        }
    }

    public class UpdateMatchResultException : Exception
    {
        public UpdateMatchResultException(string message) : base(message) { }
    }
}