using MobileUI.Consume;
using MobileUI.Mapping;
using Models;
using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var mapper = new ModelMapper<Employee>();
            mapper.Bind(new EntryMapping(firstName, fnameErr), e => e.FirstName);
            mapper.Bind(new EntryMapping(lname, lnameErr), e => e.LastName);

            submitBtn.Clicked += async delegate {

                await mapper.SubmitAsync(e => EmployeeAccess.Post(e), s => display.Text = s);

                //display.Text = $"First name: {m.FirstName} and last name: {m.LastName} with age {m.Age}";
                //mapper.Model = new Employee { FirstName = "Try", LastName = "Another", Age = 55 };
            };

		}
	}
}
