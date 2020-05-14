using System;
using System.Collections.Generic;
using EatClean.Models;
using EatClean.Services;
using Xamarin.Forms;

namespace EatClean.Views
{
    public partial class MainPage : ContentPage
    {
        UserService UserService = DependencyService.Get<UserService>();

        public LoginModel LoginModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            LoginModel = new LoginModel();
            BindingContext = this;

            MessagingCenter.Subscribe<RegisterPage, LoginModel>(this, "AddItem", async (obj, loginModel) => {
                try
                {
                    await UserService.Register(loginModel.UserName, loginModel.Password);                    
                }
                catch (Exception)
                {
                    await DisplayAlert("Warning", "Something wrong", "OK");
                }
            });
        }

        async void loginButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(LoginModel.UserName) || string.IsNullOrEmpty(LoginModel.Password))
                {
                    await DisplayAlert("", "Username or Password is require", "OK");
                    return;
                }

                var user = await UserService.Login(LoginModel.UserName, LoginModel.Password);
                Services.Setting.IsLoggedIn = true;
                Services.Setting.UserId = user.Id;
                Services.Setting.UserName = user.UserName;
                Services.Setting.Photo = user.Photo;
                Services.Setting.Name = user.Name;

                App.Instance.MainPage = new AppShell();
            }
            catch (Exception)
            {
                await DisplayAlert("Warning", "Username or Password incorrect", "OK");
            }
        }

        async void registerButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }
    }
}
