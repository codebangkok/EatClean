using System;
using System.Collections.Generic;
using EatClean.Models;
using Xamarin.Forms;

namespace EatClean.Views
{
    public partial class RegisterPage : ContentPage
    {
        public LoginModel LoginModel { get; set; }

        public RegisterPage()
        {
            InitializeComponent();
            LoginModel = new LoginModel();
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", LoginModel);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        
    }
}
