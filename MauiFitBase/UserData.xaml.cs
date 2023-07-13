using Microsoft.Data.Sqlite;
using System.Globalization;

namespace MauiFitBase;

public partial class UserData : ContentPage
{
    User U = new User();
    List<Training> Trainings = new List<Training>();
    public UserData(User u, Training? t)
	{
        U = u;
        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();

        

        command.CommandText = "SELECT * FROM Training";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var NameOfTraining = reader.GetString(1);
                var DateOfTraining = reader.GetString(2);
                var date = DateOnly.FromDateTime(DateTime.Now);
                try
                {
                    date = DateOnly.ParseExact(DateOfTraining, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                catch
                {
                    date = DateOnly.ParseExact(DateOfTraining, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                


                var OwnerOfTraining = reader.GetString(3);
                Training T = new Training(id, NameOfTraining,OwnerOfTraining, date);

                if(OwnerOfTraining == u.Name)
                {
                    Trainings.Add(T);
                }
            }
        }

        if(t != null)
        {
            Trainings.Add(t);
        }

        InitializeComponent();
		UserInfo.Text ="Welcome " + u.Name;
        Seznam.ItemsSource = Trainings;
	}

    private void SignOut_Clicked(object sender, EventArgs e)
    {
        MainPage MP = new MainPage();
        App.Current.MainPage = MP;
    }

    private void AddTrainnigB_Clicked(object sender, EventArgs e)
    {
        AddTraining AT = new AddTraining(U);
        App.Current.MainPage = AT;
    }

    private void DeleteTrainingB_Clicked(object sender, EventArgs e)
    {
        Training Ts = Seznam.SelectedItem as Training;
        if(Ts != null)
        {
            var connectionString = "Data Source=Users.db;";
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "PRAGMA key='your-secret-key';";
            command.ExecuteNonQuery();

            Trainings.Remove(Ts);
            refresh();

            command.CommandText = $"DELETE FROM Training WHERE NameOfTraining = ('{Ts.NameOfTraining}') AND OwnerOfTraining = ('{Ts.Owner}')";
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

    private void CreateExcersiseB_Clicked(object sender, EventArgs e)
    {

    }

    private void DeleteExcersiseB_Clicked(object sender, EventArgs e)
    {

    }

    private void refresh()
    {
        Seznam.ItemsSource = null;
        Seznam.ItemsSource = Trainings;
    }
}