using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Parichko.Data;
using Parichko.Models;
using DataAccess.Data;

namespace Parichko.Views;

public partial class Register : ContentPage
{
	private readonly ParichkoDbContext _context;
	public Register()
	{
		InitializeComponent();
        _context = new ParichkoDbContext();
        //_context = context ?? throw new ArgumentNullException(nameof(context));
    }

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		string userEmail = EmailEntry.Text.ToLower();
		string userPass = PassEntry.Text.ToString();
		string userPass2 = PassRepeat.Text.ToString();

        if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPass) || string.IsNullOrWhiteSpace(userPass2))
		{
            await DisplayAlert("�������� ������", "�������� �� ��������� �� ������������!", "�����");
            return;
        }
		else
		{
            //await DisplayAlert("����", "�������� �� �� ���", "�����");
            if (userPass == userPass2)
            {
                if(_context == null)
                {
                    await DisplayAlert("�������� ������", "���������� � ��� :(", "�����");
                }

                Login userFromDb = _context.Logins.FirstOrDefault(l => l.Email == userEmail);
                if (userFromDb == null)
                {
                    try
                    {
                        Login newLogin = new Login()
                        {
                            Email = userEmail,
                            PasswordHash = userPass
                        };

                        //kak da polzvam CreatAsync tuk?

                        //dobavqne na userprofile?
                        UserProfile newProfile = new UserProfile()
                        {
                            Login = newLogin
                        };
                        newLogin.UserProfile = newProfile;
                        _context.Logins.Add(newLogin);
                        _context.UserProfiles.Add(newProfile);

                        await _context.SaveChangesAsync();

                        Preferences.Set("LoggedUserId", newProfile.Id);

                        await Shell.Current.GoToAsync("///HomePage");

                        await DisplayAlert("���", "�������� �� � �������� �������!", "�����");

                    }
                    catch(Exception ex)
                    {
                        await DisplayAlert("�������� ������", ex.Message, "�����");
                    }
                }
                else
                {
                    await DisplayAlert("�������� ������", "���������� � ����� ����� ���� ����������.", "�����");
                }
            }
            else
            {
                await DisplayAlert("�������� ������", "�������� ������ �� ��������!", "�����");
                return;
            }
        }
	}
}