using Microsoft.Data.Sqlite;
using System.Globalization;

namespace MauiFitBase;

public partial class AddTraining : ContentPage
{
    User U = new User();
    List<Lift> Lifts= new List<Lift>();
    bool isEdit = false;
	public AddTraining(User u, DateTime? selectedDate, string? nameOfTraining, Lift? l, bool IsEdit)
	{
        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();
        isEdit = IsEdit;

		InitializeComponent();
        //Vypne n�kter� komponenty p�i editu
        if (IsEdit)
        {
            DateSelector.IsEnabled = false;
            TraingNameE.IsEnabled = false;
        }

        U = u;
        if (nameOfTraining != null)
        {
            TraingNameE.Text = nameOfTraining;
        }
        string TrainingName = TraingNameE.Text;
        //Nahraje cviky v tr�ninku
        command.CommandText = "SELECT * FROM Lift";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var OgTraining = reader.GetString(1);
                var ownerOfTraining = reader.GetString(2);
                var Type = reader.GetString(3);
                var sets = reader.GetInt32(4);
                var reps = reader.GetInt32(5);
                var weight = reader.GetFloat(6);
                Lift lift = new Lift(id, OgTraining, ownerOfTraining, Type, sets, reps, weight);

                if (lift.OwnerOfLift == U.Name && lift.OgTraining == TrainingName)
                {
                    Lifts.Add(lift);
                }
            }
        }

        if (selectedDate != null)
        {
            DateSelector.Date = (DateTime)selectedDate;
        }
        
        Seznam.ItemsSource = Lifts;
	}

    

    private void AddLiftsB_Clicked(object sender, EventArgs e)
    {
        AddLift AL = new AddLift(U,TraingNameE.Text,DateSelector.Date, isEdit);
        App.Current.MainPage = AL;
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

        List<string> Trainings = new List<string>();
        command.CommandText = "SELECT * FROM Training";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var NameOfTraining = reader.GetString(1);
                var OwnerOfTraining = reader.GetString(3);
                string Tname =  NameOfTraining;

                if (OwnerOfTraining == U.Name)
                {
                    Trainings.Add(Tname);
                }
            }
        }
        if (DateSelector.Date == null)
        {
            DisplayAlert("DateMissing", "Enter date", "OK");
            IsSuccesfull = false;
        }
        if (String.IsNullOrEmpty(TraingNameE.Text))
        {
            DisplayAlert("NameOfTrainingMissing", "Enter name of training", "OK");
            IsSuccesfull = false;
        }
        if (Trainings.Contains(TraingNameE.Text) && isEdit)
        {
            UserData UD = new UserData(U, null);
            App.Current.MainPage = UD;
        }
        else if (Trainings.Contains(TraingNameE.Text))
        {
            DisplayAlert("TrainingAlreadyExists", "Training with this name already exists, choose different name", "OK");
            TraingNameE.Text = "";
            IsSuccesfull = false;
        }
        if (IsSuccesfull)
        {
            //P�i �sp�chu vytvo�� tr�nink
            Training T = new Training(0,TraingNameE.Text,U.Name, DateOnly.FromDateTime(DateSelector.Date));
            UserData UD = new UserData(U,T);
            command.CommandText = $"INSERT INTO Training( NameOfTraining, DateOfTraining, OwnerOfTraining) VALUES ( '{T.NameOfTraining}', '{T.DateOfTraining}' , '{U.Name}')";
            command.ExecuteNonQuery();
            connection.Close();
            App.Current.MainPage = UD;
        }
    }

    private void RemoveLiftB_Clicked(object sender, EventArgs e)
    {
        Lift deLift = Seznam.SelectedItem as Lift;
        if (deLift != null)
        {
            var connectionString = "Data Source=Users.db;";
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "PRAGMA key='your-secret-key';";
            command.ExecuteNonQuery();

            Lifts.Remove(deLift);
            refresh();

            command.CommandText = $"DELETE FROM Lift WHERE id = ({deLift.Id})";
            command.ExecuteNonQuery();
            connection.Close();

        }
    }

    private void refresh()
    {
        Seznam.ItemsSource = null;
        Seznam.ItemsSource = Lifts;
    }
}