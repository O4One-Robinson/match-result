using Microsoft.AspNetCore.Mvc;
using MatchResult.Services;
using MatchResult.Models;

namespace MatchResult.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchResultController : ControllerBase
    {
        private readonly IMatchResultService _matchResultService;

        public MatchResultController(IMatchResultService matchResultService)
        {
            _matchResultService = matchResultService;
        }

        [HttpGet("QueryMatchResult/{matchId}")]
        public ActionResult<string> QueryMatchResult(int matchId)
        {
            try
            {
                var result = _matchResultService.QueryMatchResult(matchId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("UpdateMatchResult")]
        public ActionResult<string> UpdateMatchResult(int matchId, Event matchEvent)
        {
            try
            {
                var result = _matchResultService.UpdateMatchResult(matchId, matchEvent);
                return Ok(result);
            }
            catch (UpdateMatchResultException ex)
            {
                // 記錄錯誤事件和原始比賽結果
                // 這裡可以加入日誌記錄邏輯
                return BadRequest($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
} 