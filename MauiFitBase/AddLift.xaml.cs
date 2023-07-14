using Microsoft.Data.Sqlite;
using System.Xml.Serialization;

namespace MauiFitBase;

public partial class AddLift : ContentPage
{

    User U = new User();
    string? NameOfTraining = string.Empty;
    DateTime? DatumTreninku = null;
    string liftsFile = $@"C:\Users\{Environment.UserName}\Documents\Lifts.xml";
    string[] liftTypes = new string[] { };

    public AddLift(User u, string? TrainingOg, DateTime? Datum)
	{
        XmlSerializer serializer = new XmlSerializer(typeof(string[]));
        using (StreamReader reader = new StreamReader(liftsFile))
        {
            liftTypes= (string[])serializer.Deserialize(reader);
        }
        U = u;
        DatumTreninku = Datum;
        NameOfTraining = TrainingOg;
		InitializeComponent();
        LiftPicker.ItemsSource = liftTypes;
	}

    private void DoneB_Clicked(object sender, EventArgs e)
    {
        bool IsSuccesfull = true;
        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();

        command.CommandText = "SELECT * FROM Lift";

        List<Lift> Lifts = new List<Lift>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var ogTraining = reader.GetString(1);
                var ownerOfTraining = reader.GetString(2);
                var Type = reader.GetString(3);
                var sets = reader.GetInt32(4);
                var reps = reader.GetInt32(5);
                var weight = reader.GetFloat(6);
                Lift lift = new Lift(id, ogTraining, ownerOfTraining, Type, sets, reps, weight);

                if(lift.OwnerOfLift == U.Name && lift.OgTraining == NameOfTraining)
                {
                    Lifts.Add(lift);
                }
            }
        }

        if (LiftPicker.SelectedItem == null)
        {
            DisplayAlert("NoLiftSelected", "Select lift", "OK");
            IsSuccesfull = false;
        }
        if (String.IsNullOrWhiteSpace(SetsE.Text)|| !int.TryParse(SetsE.Text, out int number))
        {
            DisplayAlert("NoSetsEntered", "Enter number of sets", "OK");
            SetsE.Text = "";
            IsSuccesfull = false;
        }
        if (String.IsNullOrWhiteSpace(RepsE.Text) || !int.TryParse(RepsE.Text, out int number2))
        {
            DisplayAlert("NoRepsEntered", "Enter number of reps", "OK");
            RepsE.Text = "";
            IsSuccesfull = false;
        }
        if (String.IsNullOrWhiteSpace(WeightE.Text) || !int.TryParse(SetsE.Text, out int number3))
        {
            DisplayAlert("NoWeightEntered", "Enter amount of weiht", "OK");
            WeightE.Text = "";
            IsSuccesfull = false;
        }
        if (IsSuccesfull)
        {
            Lift l = new Lift(0,NameOfTraining,U.Name,LiftPicker.SelectedItem.ToString(),int.Parse(SetsE.Text),int.Parse(RepsE.Text),float.Parse(WeightE.Text));
            command.CommandText = $"INSERT INTO Lift(NameOfTraining, OwnerOfTraining, TypeOfLift, Sets, Reps, Weight) VALUES ('{l.OgTraining}', '{l.OwnerOfLift}', '{l.Type}', '{l.Sets}', '{l.Reps}', '{l.Weight}')";
            command.ExecuteNonQuery();
            connection.Close();
            AddTraining AT = new AddTraining(U, DatumTreninku,NameOfTraining,l);
            App.Current.MainPage = AT;
        }        
    }
}