using System.Xml.Serialization;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Data.Sqlite;

namespace MauiFitBase;

public partial class LiftOV : ContentPage
{

    string liftsFile = $@"C:\Users\{Environment.UserName}\Documents\Lifts.xml";
    string[] liftTypes = new string[] { };
    User U ;
    List<Lift> Lifts = new List<Lift>();
    public LiftOV(User u)
	{
        U = u;
        XmlSerializer serializer = new XmlSerializer(typeof(string[]));
        using (StreamReader reader = new StreamReader(liftsFile))
        {
            liftTypes = (string[])serializer.Deserialize(reader);
        }

        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();

        command.CommandText = $"SELECT * FROM Lift ";
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
                if (lift.OwnerOfLift == U.Name)
                {
                    Lifts.Add(lift);
                }
                //Lifts.Add(lift);
            }
        }

        ViewModel VM = new ViewModel();

        double[] WeightArr = { };
        for(int i = 0; i <Lifts.Count; i++)
        {
            WeightArr[i] = (double)Lifts[i].Weight;
        }
        VM.ChangeData(WeightArr);
        InitializeComponent();
        LiftPicker.ItemsSource = liftTypes;
    }
}