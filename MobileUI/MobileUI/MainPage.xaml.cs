using MobileUI.Mapping;
using Models;
using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Access;

namespace MobileUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var mapper = new ModelHandler<Employee>();
            mapper.Bind(e => e.FirstName, firstName, fnameErr);
            mapper.Bind(e => e.LastName, lname, lnameErr);
            mapper.Bind(e => e.Age, age, ageErr);
            mapper.Bind(e => e.ToBeFiredOn, fireDate, fireTime, dateErr);

            submitBtn.Clicked += async delegate {
                await mapper.SubmitAsync(e => EmployeeAccess.Post(e), s => display.Text = s);
            };

            showBtn.Clicked += async delegate {
                if (mapper.TryGetModel(out Employee emp))
                {
                    var employee = await EmployeeAccess.Get(emp.Age);
                    mapper.ClearErrors();
                    if (employee.IsSuccessful) mapper.SetModel(employee);
                    else await DisplayAlert("Nop", $"Something failed with the error: {employee.StatusCode}", "Oh damit!!!");
                }
            };

            invalidBtn.Clicked += async delegate {
                await mapper.SubmitAsync(async e => await EmployeeAccess.GetNoErrors());

                await DisplayAlert("No errors!", "See ma! no errors!!!", "Nothing is better than something");
            };

		}
	}
}
