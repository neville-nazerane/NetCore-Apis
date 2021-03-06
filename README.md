# NetCore-Apis
      
      This project is designed to provide a smooth flow from a .net core REST API to any UI such as xamarin.forms. 
      
      
 ### List of projects in this repo:
 
 - **Nugets**:
   - *NetCore.Apis.Consumer*: Provides a simplified way to consume REST API, especially API created using .net core. Key features include:   
     1. A `HttpClient` wrapper class "ApiConsumer" that provides functions for HTTP calls, that can directly get and/or set typed objects.
     2. A `HttpResponseMessage` wrapper class "ApiConsumedResponse" that provides helpful properties including the text response and a collection of model validation errors (where applicable)
     3. Default events can be set to "ApiConsumer" for the various http status codes.
    - *NetCore.Apis.Client.UI*: Binds/maps models to any UI. Note that the word "map" is used to avoid conflicts and confusions xamarin binders. Key features include: 
      1. A generic mapper interface to allow configure any data type to any UI component. The mapper also allows the configuration displaying the list of errors (model validation).
      2. A "ModelHandler" that maps a given model to UI components with the help of the mapper interface. This allows for a strongly typed mapping of the models and UI components. 
      3. A submit function that gets the data from all mapped components anything preset and works with "NetCore.Apis.Consumer" to check for errors (model validation) and populate errors into UI. 
    - *NetCore.Apis.XamarinForms*: Sets up mappers for common xamarin components with common data types. Key features include: 
      1. Mapping components such as Entry, Editor, DatePicker, TimePicker and switch to data types such as string, int, int?, bool, etc. - 
 - **Tests**:
   - *Constants*: Stores constants such as URLs to be used across test projects
   - *Models*: Common models to be used across test projects
   - *TestWebApi*: A web api created on .net core to be consumed and tested bo other projects
   - *Access*: A project that uses `NetCore.Apis.Consumer` nuget to consume the *TestWebApi*, meant for code reuse for some test projects.
   - *NetCore.Apis.Consumer.Test*: Unit testing for `NetCore.Apis.Consumer`. Directly refrences the `NetCore.Apis.Consumer` project.
   - *MobileUI*: A xamarin forms app meant to test the `NetCore.Apis.Client.UI`. This project directly refrences the `NetCore.Apis.Client.UI` in order to test each feature. 
   - *XamarinTest*: A xamarin forms app meant to test the `NetCore.Apis.XamarinForms`. This project directly refrences the `NetCore.Apis.XamarinForms` in order to test each feature. 
    
        
