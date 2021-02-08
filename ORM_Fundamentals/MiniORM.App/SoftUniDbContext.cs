namespace MiniORM.App
{
    internal class SoftUniDbContext
    {
        private string connectionString;

        public SoftUniDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}