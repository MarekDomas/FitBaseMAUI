using Microsoft.Data.Sqlite;

namespace MauiFitBase;

public partial class RegisterPage : ContentPage
{
     

	public RegisterPage()
	{
		InitializeComponent();
    }

    private void DoneB_Clicked(object sender, EventArgs e)
    {
        //Vytvo�� datab�zy s u�ivateli a nahraje je do listu
        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();
               
        List<string> UserNames = new List<string>();

        // Query the data
        command.CommandText = "SELECT * FROM Users";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var UserName = reader.GetString(1);
                var UserPasswd = reader.GetString(2);
                User U = new User(id, UserName, UserPasswd);

                UserNames.Add(UserName);
            }
        }

        if (UserNames.Contains(NewUserNameE.Text))
        {
            DisplayAlert("User name exists", "Username already exists, use different name", "OK");
            ClearEntry();
        }
        if(String.IsNullOrWhiteSpace(NewUserNameE.Text))
        {
            DisplayAlert("Fill in user name", "Enter user name", "OK");
            ClearEntry();
        }
        if (NewPasswdE.Text != AgainPasswdE.Text)
        {
            DisplayAlert("Bad password", "Passwords are not identical", "OK");
            ClearEntry();
        }
        if(String.IsNullOrWhiteSpace(NewPasswdE.Text) || String.IsNullOrWhiteSpace(AgainPasswdE.Text))
        {
            DisplayAlert("Fill in passwords", "Enter your password", "OK");
            ClearEntry();
        }
        //Usp�n� vytvo�en� ��tu
        if (!UserNames.Contains(NewUserNameE.Text) && NewPasswdE.Text == AgainPasswdE.Text && !String.IsNullOrWhiteSpace(NewUserNameE.Text) && !String.IsNullOrWhiteSpace(NewPasswdE.Text) && !String.IsNullOrWhiteSpace(AgainPasswdE.Text))
        {
            User NewUser = new User(UserNames.Count,NewUserNameE.Text, AgainPasswdE.Text);
            command.CommandText = $"INSERT INTO Users( UserName, UserPasswd) VALUES ( '{NewUser.Name}', '{NewUser.Passwd}')";
            command.ExecuteNonQuery();
            connection.Close();

            MainPage MP = new MainPage();
            App.Current.MainPage = MP;
        }
    }

    private void CancelB_Clicked(object sender, EventArgs e)
    {
        MainPage MP = new MainPage();
        App.Current.MainPage = MP;
    }

    //Nuluje vstupy 
    public void ClearEntry()
    {
        NewPasswdE.Text = "";
        AgainPasswdE.Text = "";
        NewUserNameE.Text = "";
    }
}