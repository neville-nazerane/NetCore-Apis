using Access;
using Models;
using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTest
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var handler = new ModelHandler<Employee>();
            handler.Bind(e => e.FirstName, fname, fnameErr);
            handler.Bind(e => e.LastName, lname);
            handler.Bind(e => e.Age, age, ageErr);
            handler.Bind(e => e.IsDead, deadSwitch);
            handler.Bind(e => e.ToBeFiredOn, fireDate, fireTime);

            postBtn.Clicked += async delegate {
                await handler.SubmitAsync(
                    e => EmployeeAccess.Post(e),
                    async s => await DisplayAlert("Success", $"Success with message {s}", "Ok"),
                    onError: async e => await DisplayAlert("Failed", $"Failed with status: '{e.StatusCode}' and error '{e.TextResponse}'", "Cancel")
                    );
            };

        }
	}
}
