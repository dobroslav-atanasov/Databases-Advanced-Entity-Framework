namespace VillainNames
{
    public class Minion
    {
        public Minion(string minionName, string villainName)
        {
            this.MinionName = minionName;
            this.VillainName = villainName;
        }

        public string MinionName { get; set; }

        public string VillainName { get; set; }
    }
}