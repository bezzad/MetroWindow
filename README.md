# MetroWindow

WPF light Metro Window

### Sample View

### Sample Window XAML

```xaml
<metroWindow:MetroWindow x:Class="MetroWindowSample.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:metroWindow="clr-namespace:MetroWindow;assembly=MetroWindow"
        mc:Ignorable="d"
        Icon="Resources/watermelon.ico"
        Title="Window Title" 
        Height="450" Width="800"
        ShowTitleBar="True" 
        TitleBarTextVisibility="Visible"
        WindowTitleBrush="Chartreuse">
    <Grid>
        <Path Data="M50,0L100,50 50,100 0,50z" Fill="White" Stretch="Fill" Stroke="Black" StrokeThickness="2" />
        <ToggleButton Content="Is FullScreen ?" IsChecked="{Binding Fullscreen}" Width="100"  Height="100"/>
    </Grid>
</metroWindow:MetroWindow>
```