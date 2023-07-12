using Microsoft.Data.Sqlite;

namespace MauiFitBase;

public partial class MainPage : ContentPage , MyFunctions
{
	

	public MainPage()
	{
		InitializeComponent();
	}

    public void ClearEntry()
    {
        LoginE.Text = string.Empty;
        PasswdE.Text = string.Empty;
    }

    private void Login_Clicked(object sender, EventArgs e)
    {
        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Users(id INTEGER PRIMARY KEY, UserName TEXT, UserPasswd TEXT)";
        command.ExecuteNonQuery();

        List<User> Users = new List<User>();
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
                Users.Add(U);

                DebugL.Text += U + " ";

                UserNames.Add(UserName);
            }
        }

        bool loginSuccefull = false;
        foreach (User user in Users)
        {
            loginSuccefull = false;
            if(LoginE.Text == user.Name && PasswdE.Text == user.Passwd)
            {
                UserData UD = new UserData(user);
                App.Current.MainPage = UD;
                loginSuccefull = true;
                break;
            }
        }
        if (!loginSuccefull)
        {
            DisplayAlert("Cannot sign in", "User name or password is wrong","OK");
            ClearEntry();
        }
    }

    private void RegisterB_Clicked(object sender, EventArgs e)
    {
        RegisterPage RP = new RegisterPage();
        App.Current.MainPage = RP;
    }
}
