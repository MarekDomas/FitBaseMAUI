namespace MauiFitBase;

public partial class UserData : ContentPage
{
	public UserData(User u)
	{
		InitializeComponent();
		UserInfo.Text = u.Name + u.Passwd + u.Id;
	}

    private void SignOut_Clicked(object sender, EventArgs e)
    {
        MainPage MP = new MainPage();
        App.Current.MainPage = MP;
    }

    private void AddTrainnigB_Clicked(object sender, EventArgs e)
    {

    }
}