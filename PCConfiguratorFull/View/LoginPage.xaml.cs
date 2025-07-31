namespace MauiApp3.View;

 public partial class LoginPage : ContentPage
 {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorLabel.Text = "������� ����� � ������!";
                ErrorLabel.IsVisible = true;
                return;
            }

            // ���� ������ �������� �����
            if (username == "admin" && password == "1234")
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());

            }
            else
            {
                ErrorLabel.Text = "�������� ����� ��� ������!";
                ErrorLabel.IsVisible = true;
            }
        }
 }

