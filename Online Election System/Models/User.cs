namespace Online_Election_System.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? NationalIDNumber { get; set; }
        public string? VotingDistrict { get; set; }
        public string? HashedPassword { get; set; }
        public string? Salt { get; set; }
        public bool IsVerified { get; set; }
        public bool HasVoted { get; set; }

    }
}
