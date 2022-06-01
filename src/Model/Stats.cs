namespace WordleBlazor.Model
{
    public class Stats
    {
        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int CurrentStreak { get; set; }

        public int BestStreak { get; set; }

        /// <summary>
        /// 'Key' represents a row.
        /// 'Key' -1 refers to lost games.
        /// 'Value' is the number of games won in that row.
        /// </summary>
        public Dictionary<int, int> GamesResultDistribution { get; set; } = new Dictionary<int, int>()
        {
            { -1, 0 },
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 }
        };
    }
}
