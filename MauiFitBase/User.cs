namespace MauiFitBase
{
    public class User
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Passwd{ get; set; }

        public User(int aId, string aName, string aPasswd)
        {
            Id = aId;
            Name = aName;
            Passwd = aPasswd;
        }

        public User() { }

        public override string ToString()
        {
            return $"id: {Id} Name: {Name} Passwd: {Passwd}";
        }
    }
}
