namespace MauiFitBase
{
    public class Lift
    {
        public int Id { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public float Weight { get; set; }
        public string Type { get; set; }
        public string OgTraining { get; set; }
        public string OwnerOfLift { get; set; }
        public Lift(int id, string TrainingOg, string ownerOfLift ,string type, int sets, int reps, float weight)
        {
            Id = id;
            OgTraining = TrainingOg;
            OwnerOfLift = ownerOfLift;
            Type = type;
            Sets = sets;
            Reps = reps;
            Weight = weight;
        }
    }
}
