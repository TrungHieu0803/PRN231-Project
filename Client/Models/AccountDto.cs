namespace Client.Models
{
    public class AccountDto
    {
        public int accountId { get; set; }

        public int? role { get; set; }
    }

    public class TokenDto
    {
        public string token { get; set; }
    }
}
