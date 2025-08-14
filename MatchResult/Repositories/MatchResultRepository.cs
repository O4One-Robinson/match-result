namespace MatchResult.Repositories
{
    public class MatchResultRepository : IMatchResultRepository
    {
        public string GetRawData(int matchId)
        {
            // 這裡模擬從資料庫取得資料
            // 實際實作時會連接真實的資料庫
            return matchId switch
            {
                1 => "HA",
                2 => "HA;H",
                _ => "HA" // 預設值
            };
        }
    }
} 