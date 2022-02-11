# Not Another MVVM Framework
NinjaMvvm is not so much an MVVM Framework as it is a few helpful classes and converters.  A major goal is to not get in your way, and not require you do do things a certain way.  It provides 2 basic things that nearly all MVVM users need.
1. A base ViewModel class the provides easy use of `INotifyPropertyChanged`
2. An implementation of `ICommand` very commonly called `RelayCommand`

It does not provide Navigation.  Reason being with View to View, ViewModel to ViewModel, Messenger, Events, there are so many strong opinions about the _right_ way to navigate in your MVVM application.  You can use NinjaMvvm and still choose whatever Navigation strategy you'd like. 

Along with providing an implementation of `INotifyPropertyChanged`, the `ViewModelBase` class also provides some extras that are useful in many MVVM applications.

## Very Clean INotifyPropertyChanged
By not passing in the PropertyName you defininitely cut down on copy/paste errors.  I know this is not revolutionary, there are dozens of implementations that do it, I like this one because you also don't have to perform the value changed comparison here, it's done in the base.
```csharp
public string SomeTextValue
{
    get { return GetField<string>(); }
    set { SetField(value); }
}
```
if you need to know if the field value differed from the original
```csharp
public string SomeTextValue
{
    get { return GetField<string>(); }
    set {
        if (SetField(value))
        {
            //do something else here because the value was different than the original
        }
    }
}
```
## Design Data
The `ViewModelBase` provides `OnLoadDesignData` which your ViewModel can override to put static/hardcoded data into the ViewModel for presenting in a designer.
An example of this can be found in the [Wpf Samples](https://github.com/DumpsterNinja/NinjaMvvm/tree/master/src/NinjaMvvm/Samples/NinjaMvvm.Wpf.Samples)
```csharp
protected override void OnLoadDesignData()
    {
        this.StampMessage = "This message is from design data";
        this.IsStampAllowed = true;
    }
```
then in your xaml
```xaml
mc:Ignorable="d" 
xmlns:designViewModel="clr-namespace:NinjaMvvm.Wpf.Samples.ViewModels"
d:DataContext ="{d:DesignInstance {x:Type designViewModel:MainViewModel}, IsDesignTimeCreatable=True}"
```

## Load/Reload
The ViewModelBase provides `OnReloadDataAsync` which will be called when the ViewModel is bound, and anytime `ReloadDataAsync` is called.  The goal is to give a standard way for loading data throughout your ViewModels.  `OnReloadDataAsync` accepts a cancellation token and can be cancelled with a call to `CancelReloadDataAsync`.
```csharp
protected override async Task<bool> OnReloadDataAsync(CancellationToken cancellationToken)
{
    try
    {
        var data = await _service.DownloadAllDataFromAllTheInternets();

        //now populate your properties with the data 
        this.DataX = data.X;
        this.DataY = data.Y;

        return true;
    }
    catch
    {
        //log the error maybe

        return false; //returning false will simply set the LoadFailed property which maybe you tell your UI to bind to so that the input controls are all disabled when loading failed
    }
}
```
You can control whether or not `ReloadDataAsync` is automatically called when the ViewModel gets bound to the UI by utilizing the `LoadWhenBound` property.

## IsBusy
A simply utility property for your controls/views to bind to so they can handle the cases where the view is busy doing something and you either want to maybe disable everything, or maybe just show some status.
`IsBusy` automatically used by `OnReloadDataAsync`.

## WPF FluentValidation
The WPF package provides WpfViewModelBase which implements `IDataErrorInfo` and uses FluentValidation to provide an easy way to include your property validation which will automatically be recognized by WPF.

For example, for your viewmodel create a FluentValidation Validator
```csharp
        class MainViewModelValidator : AbstractValidator<MainViewModel>
        {
            public MainViewModelValidator()
            {
                RuleFor(obj => obj.SomeTextValue)
                    .NotEmpty()
                    .Length(min: 1, max: 4);
            }
        }

        protected override IValidator GetValidator()
        {
            return new MainViewModelValidator();
        }
```
 and in your ViewModel's save command
 ```csharp
var validateResult = this.GetValidationResult();
if (!validateResult.IsValid)
{
    ShowErrors = true; //this tells the base ViewModel to populate errors in IDataErrorInfo so that Wpf can show the errors
    return;
}
```
# References
I had an Implementation of `INotifyPropertyChanged` that I had been using for more than a decade;  I was inspired by the solution in [xamvvm](https://github.com/xamvvm/xamvvm), so I am now using a slightly modified version.

[FluentValidation](https://github.com/JeremySkinner/FluentValidation) if you aren't using it for ViewModel validation you should definitely check it out.

# Want to say thank you?
If something in this repo has helped you solve a problem, or made you more efficient, I welcome your support!

<a href="https://www.buymeacoffee.com/timburris" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
