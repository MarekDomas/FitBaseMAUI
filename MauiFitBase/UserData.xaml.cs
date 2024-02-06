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
        //šifrovaná komunikace
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();

        
        //Nahraje všechny tréninky uživatele
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
        Trainings.Distinct();
        //Odstranìní duplikátù které mùžou vznika pøi editaci tréninkù a seøazení podle data
        var Treninky = Trainings.DistinctBy(i => i.NameOfTraining);
        Treninky = Treninky.OrderBy(i => i.DateOfTraining);
        Seznam.ItemsSource = Treninky;
	}

    private void SignOut_Clicked(object sender, EventArgs e)
    {
        MainPage MP = new MainPage();
        App.Current.MainPage = MP;
    }

    private void AddTrainnigB_Clicked(object sender, EventArgs e)
    {
        AddTraining AT = new AddTraining(U, null,null,null,false);
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

            //Odstraní trénink a všechny jeho cviky
            command.CommandText = $"DELETE FROM Training WHERE NameOfTraining = ('{Ts.NameOfTraining}') AND OwnerOfTraining = ('{Ts.Owner}')";
            command.ExecuteNonQuery();
            command.CommandText = $"DELETE FROM Lift WHERE NameOfTraining = ('{Ts.NameOfTraining}') AND OwnerOfTraining = ('{Ts.Owner}')";
            command.ExecuteNonQuery();
            connection.Close();
            
        }
    }

  
    private void refresh()
    {
        Seznam.ItemsSource = null;
        Seznam.ItemsSource = Trainings;
    }

    private void EditTrBtn_Clicked(object sender, EventArgs e)
    {
        Training SelTraining= Seznam.SelectedItem as Training;
        if(SelTraining != null )
        {
            AddTraining AT = new AddTraining(U,SelTraining.DateOfTraining.ToDateTime(TimeOnly.Parse("10:00 PM")),SelTraining.NameOfTraining, null,true);
            App.Current.MainPage = AT;
        }
    }

    private void GraphBtn_Clicked(object sender, EventArgs e)
    {
        LiftOV LOV = new LiftOV(U);
        App.Current.MainPage= LOV;
    }
}