using Microsoft.Data.Sqlite;
using System.Globalization;

namespace MauiFitBase;

public partial class AddTraining : ContentPage
{
    User U = new User();
	public AddTraining(User u)
	{
        U = u;
		InitializeComponent();
	}

    

    private void AddLiftsB_Clicked(object sender, EventArgs e)
    {

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
        if (Trainings.Contains(TraingNameE.Text))
        {
            DisplayAlert("TrainingAlreadyExists", "Training with this name already exists, choose different name", "OK");
            TraingNameE.Text = "";
            IsSuccesfull = false;
        }
        if (IsSuccesfull)
        {
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

    }
}