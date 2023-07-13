namespace MauiFitBase
{
    public class Training
    {
        public int? id { get; set; }
        public string NameOfTraining { get; set; }
        public string Owner{ get; set; }
        public DateOnly DateOfTraining { get; set; }

        public Training(int aID, string aNameOfTraining, string aOwner, DateOnly aDateOfTraning) 
        {
            id = aID;
            NameOfTraining = aNameOfTraining;
            Owner = aOwner;
            DateOfTraining = aDateOfTraning;
        }
    }
}
