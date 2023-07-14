using Microsoft.Data.Sqlite;
using System.Text;
using System.Xml.Serialization;

namespace MauiFitBase;

public partial class MainPage : ContentPage , MyFunctions
{

    string[] cviky = {
                        "Dřep s činkou",
                        "Mrtvý tah",
                        "Benčpress",
                        "Ramenní tlak",
                        "Veslování",
                        "Log lift",
                        "Curl s činkami",
                        "Tricepsová extenze",
                        "Hammer curl",
                        "Kabelový fly",
                        "Leg press",
                        "Stahování na lýtkové svaly",
                        "Hip thrust",
                        "Glute bridge",
                        "Výpady",
                        "Step-up",
                        "Rumunský mrtvý tah",
                        "Good morning",
                        "Cable pull-through",
                        "Rows v sedě",
                        "Pulldown",
                        "Přístroj na tlak hrudníku",
                        "Přístroj na ramenní tlak",
                        "Předkopy",
                        "Přístroj na hamstringy",
                        "Přístroj na svaly břicha",
                        "Russian twist",
                        "Medicinbalový slam",
                        "Bitevní provaz",
                        "Švihadlo",
                        "Kettlebell swing",
                        "Farmářská chůze",
                        "Tahání/tlačení saní",
                        "Převrácení pneumatiky"
                    };
    

    string liftsFile= $@"C:\Users\{Environment.UserName}\Documents\Lifts.xml";
    public MainPage()
	{
        if (!File.Exists(liftsFile))
        {
            using (FileStream fs = File.Create(liftsFile))
            {
                byte[] content = Encoding.UTF8.GetBytes("");
                fs.Write(content, 0, content.Length);
            }
        }

        XmlSerializer serializer = new XmlSerializer(typeof(string[]));
        using (TextWriter writer = new StreamWriter(liftsFile))
        {
            serializer.Serialize(writer, cviky);
        }
        InitializeComponent();
	}

    public void ClearEntry()
    {
        LoginE.Text = string.Empty;
        PasswdE.Text = string.Empty;
    }

    private void Login_Clicked(object sender, EventArgs e)
    {
        //Vytvoření DB a připojení k ní
        var connectionString = "Data Source=Users.db;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA key='your-secret-key';";
        command.ExecuteNonQuery();

        //Vytvoří tabulku s uživateli 
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users(id INTEGER PRIMARY KEY, UserName TEXT, UserPasswd TEXT)";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Training(id INTEGER PRIMARY KEY, NameOfTraining TEXT, DateOfTraining DATE, OwnerOfTraining TEXT)";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Lift(id INTEGER PRIMARY KEY, NameOfTraining TEXT, OwnerOfTraining TEXT, TypeOfLift TEXT, Sets INTEGER, Reps INTEGER, Weight FLOAT)";
        command.ExecuteNonQuery();

        command.CommandText = "DELETE FROM Training WHERE NameOfTraining = 'Trenink1'";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Training( NameOfTraining, DateOfTraining, OwnerOfTraining) VALUES ( 'Trenink1', '2023-07-13' , 'Marek')";
        command.ExecuteNonQuery();

        List<User> Users = new List<User>();
        
        // Načte data do listu
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
            }
        }

        //Samotné přihlášení. Proiteruje se celý list a pokud se tam uživatel najde tak je přihlášní úspěšné
        bool loginSuccefull = false;
        foreach (User user in Users)
        {
            loginSuccefull = false;
            if(LoginE.Text == user.Name && PasswdE.Text == user.Passwd)
            {
                UserData UD = new UserData(user,null);
                App.Current.MainPage = UD;
                loginSuccefull = true;
                break;
            }
        }

        //Kdyby to nebylo v podmínce, alert by vyskočil i po úspěšném přihlášení
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
