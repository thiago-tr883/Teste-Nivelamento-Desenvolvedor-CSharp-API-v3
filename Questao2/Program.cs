using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int page = 1;
        int total_pages = 1;
        int goals = 0;
        bool team1 = true;

        while (page <= total_pages)
        {
            var footballMatches = GetFootballMatches($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{(team1 ? "team1" : "team2")}={team}&page={page}");

            goals += SomaGoals(footballMatches.data, team1);

            total_pages = footballMatches.total_pages;
            page++;

            if (team1 && page > total_pages)
            {
                team1 = false;
                page = 1;
                total_pages = 1;
            }
        }

        return goals;
    }

    public static FootballMatches GetFootballMatches(string requestUri)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            return JsonConvert.DeserializeObject<FootballMatches>(response.Content.ReadAsStringAsync().Result);
        }
    }

    public static int SomaGoals(List<FootballMatchesData> data, bool team1)
    {
        int goals = 0;

        for (int i = 0; i < data.Count; i++) {
            if (team1)
                goals += data[i].team1goals;
            else
                goals += data[i].team2goals;
        }

        return goals;
    }

    public class FootballMatches
    {
        public int total_pages { get; set; }
        public List<FootballMatchesData> data { get; set; }
    }
    public class FootballMatchesData
    {
        public int team1goals { get; set; }
        public int team2goals { get; set; }
    }

}