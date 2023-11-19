namespace Noskito.Database.Dto.Accounts
{
    public record AccountDTO
    {
        public long Id { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
    }
}